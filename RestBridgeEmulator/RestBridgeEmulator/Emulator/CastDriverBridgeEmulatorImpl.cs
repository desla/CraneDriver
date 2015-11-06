namespace RestBridgeEmulator.Emulator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Web;
    using System.Xml.Serialization;
    using RestContract;

    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single)]
    internal class CastDriverBridgeEmulatorImpl : IRestBridgeContract
    {
        private PotroomNetwork[] potroomNetwork;
        private ShiftTaskDescription[] shiftTasks;
        private PotModeDescription[] potsModes;
        private AnodeStateDescription[] anodesStates;

        public CastDriverBridgeEmulatorImpl()
        {
            LoadPotroomsNetwork();
            LoadTasks();
            LoadPotsModes();
            LoadAnodesStates();
        }

        public PotroomNetwork[] GetPotroomsNetwork(string aCraneNumber)
        {
            Console.WriteLine("Запрос списка соответствия ssid точек доступа номерам корпусов.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            return potroomNetwork;
        }

        public ShiftTaskDescription GetTaskDescription(string aCraneNumber, string aPotroomNumber)
        {
            Console.WriteLine("Запрос сменного задания для корпуса " + aPotroomNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");

            foreach (var taskDescription in shiftTasks) {
                if (taskDescription.PotroomNumber == potroomNumber) {
                    return taskDescription;
                }
            }

            throw new WebFaultException<string>(
                    String.Format("Корпус с номером {0} не найден.", potroomNumber),
                    HttpStatusCode.NotFound);
        }

        public PotTask AddPotTask(string aCraneNumber, string aPotroomNumber, string aPotNumber, string aTaskType)
        {
            Console.WriteLine("Добавление электролизера в сменное задание.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");
            var potNumber = ArgumentConverter.ToInt32(aPotNumber, "Номер электролизера");

            var task = shiftTasks.FirstOrDefault(t => t.PotroomNumber == potroomNumber);
            if (task == null) {
                throw new WebFaultException<string>(
                    "Не удалось добавить задачу в сменное задание для корпуса " + potroomNumber,
                    HttpStatusCode.InternalServerError);
            }

            switch (aTaskType.ToUpper()) {
                case "ANODES_REPLACE":
                    foreach (var anodesReplaceTask in task.AnodesReplaceTasks) {
                        if (anodesReplaceTask.PotNumber == potNumber) {
                            throw new WebFaultException<string>(
                                "Сменное задание на замену анодов уже содержит электролизер с номером " + potNumber, 
                                HttpStatusCode.Conflict);
                        }
                    }

                    var tasks = new List<AnodesReplaceTask>(task.AnodesReplaceTasks);
                    tasks.Add(new AnodesReplaceTask {
                        PotNumber = potNumber,
                        time = DateTime.Now,
                        AnodeNumbers = new int[0],
                        Comments = new Comment[0]
                    });

                    task.AnodesReplaceTasks = tasks.ToArray();
                    break;
                case "FRAME_CHANGE":
                    foreach (var frameTask in task.FrameChangeTasks) {
                        if (frameTask.PotNumber == potNumber) {
                            throw new WebFaultException<string>(
                                "Сменное задание на перетяжку уже содержит электролизер с номером " + potNumber, 
                                HttpStatusCode.Conflict);
                        }
                    }

                    var frameTasks = new List<PotTask>(task.FrameChangeTasks);
                    frameTasks.Add(new PotTask {
                        PotNumber = potNumber,
                        time = DateTime.Now                        
                    });

                    task.FrameChangeTasks = frameTasks.ToArray();
                    break;
                case "POT_FILL":
                    foreach (var potFillTask in task.PotFillTasks) {
                        if (potFillTask.PotNumber == potNumber) {
                            throw new WebFaultException<string>(
                                "Сменное задание на засыпку уже содержит электролизер с номером " + potNumber, 
                                HttpStatusCode.Conflict);
                        }
                    }

                    var potFillTasks = new List<PotTask>(task.PotFillTasks);
                    potFillTasks.Add(new PotTask {
                        PotNumber = potNumber,
                        time = DateTime.Now                        
                    });

                    task.PotFillTasks = potFillTasks.ToArray();
                    break;
                case "HOPPER_FILL":
                    foreach (var hopperFillTask in task.HopperFillTasks) {
                        if (hopperFillTask.PotNumber == potNumber) {
                            throw new WebFaultException<string>(
                                "Сменное задание на заправку бункеров уже содержит электролизер " +
                                "с номером " + potNumber, 
                                HttpStatusCode.Conflict);
                        }
                    }

                    var hopperFillTasks = new List<PotTask>(task.HopperFillTasks);
                    hopperFillTasks.Add(new PotTask {
                        PotNumber = potNumber,
                        time = DateTime.Now                        
                    });

                    task.HopperFillTasks = hopperFillTasks.ToArray();
                    break;
                default:
                    throw new WebFaultException<string>("Тип операции не найден. Возможные значения: " +
                                                        "ANODES_REPLACE, FRAME_CHANGE, POT_FILL, HOPPER_FILL.",
                                                        HttpStatusCode.InternalServerError);
            }

            return new PotTask {
                time = DateTime.Now,
                PotNumber = potNumber
            };
        }

        public PotModeDescription GetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber)
        {
            Console.WriteLine("Запрос режима работы электролизера {0} в корпусе {1}", aPotNumber, aPotroomNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");
            var potNumber = ArgumentConverter.ToInt32(aPotNumber, "Номер электролизера");

            foreach (var potModeDescription in potsModes) {
                if (potModeDescription.PotroomNumber == potroomNumber &&
                    potModeDescription.PotNumber == potNumber) {
                    return potModeDescription;
                }
            }

            throw new WebFaultException<string>(
                    String.Format("Электролизер с номером корпуса {0} " +
                                  "и номером электролизера {1} не найден.",
                                  potroomNumber, potNumber),
                    HttpStatusCode.NotFound);
        }

        public PotModeDescription SetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber, string aPotMode)
        {
            Console.WriteLine("Запрос на изменение режима работы электролизера {0} " +
                              "в корпусе {1}, режим: {2}", aPotNumber, aPotroomNumber, aPotMode);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");
            var potNumber = ArgumentConverter.ToInt32(aPotNumber, "Номер электролизера");      

            foreach (var potModeDescription in potsModes) {
                if (potModeDescription.PotroomNumber == potroomNumber &&
                    potModeDescription.PotNumber == potNumber) {
                    potModeDescription.PotModeString = aPotMode;
                    return potModeDescription;
                }
            }

            throw new WebFaultException<string>(
                    String.Format("Электролизер с номером корпуса {0} " +
                                  "и номером электролизера {1} не найден.",
                                  potroomNumber, potNumber),
                    HttpStatusCode.NotFound);
        }

        public AnodeStateDescription[] GetAnodesStates(string aCraneNumber, string aPotroomNumber, string aPotNumber)
        {
            Console.WriteLine("Запрос состояния анодов в корпусе {0} " +
                              "на электролизере {1}", aPotroomNumber, aPotNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");
            var potNumber = ArgumentConverter.ToInt32(aPotNumber, "Номер электролизера");

            var result = new List<AnodeStateDescription>();
            foreach (var anodeStateDescription in anodesStates) {
                if (anodeStateDescription.PotroomNumber == potroomNumber &&
                    anodeStateDescription.PotNumber == potNumber) {
                    result.Add(anodeStateDescription);
                }
            }

            if (result.Count == 0) {
                throw new WebFaultException<string>(
                    String.Format("Анодов с номером корпуса {0} " +
                                  "и номером электролизера {1} не найден.",
                                  potroomNumber, potNumber),
                    HttpStatusCode.NotFound);
            }            

            return result.ToArray();            
        }

        public AnodeStateDescription SetAnodeState(string aCraneNumber, string aPotroomNumber, string aPotNumber, string aAnodeNumber, string aAnodeState)
        {
            Console.WriteLine("Запрос на изменение состояния анода {0} " +
                              "в корпусе {1} на электролизере {2}, состояние: {3}", 
                              aAnodeNumber, aPotroomNumber, aPotNumber, aAnodeState);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");
            var potNumber = ArgumentConverter.ToInt32(aPotNumber, "Номер электролизера");
            var anodeNumber = ArgumentConverter.ToInt32(aAnodeNumber, "Номер анода");

            for (var i = 0; i < anodesStates.Length; ++i) {             
                var anodeStateDescription = anodesStates[i];
                if (anodeStateDescription.PotroomNumber == potroomNumber &&
                    anodeStateDescription.PotNumber == potNumber &&
                    anodeStateDescription.AnodeNumber == anodeNumber) {

                    anodeStateDescription.AnodeStateString = aAnodeState;                    
                    anodeStateDescription.operationTime = DateTime.Now;

                    // Все аноды в ванне.
                    var potAnodesStates = GetAnodesStates(aCraneNumber, aPotroomNumber, aPotNumber);
                    var shiftTask = shiftTasks.FirstOrDefault(t => t.PotroomNumber == potroomNumber);
                    var findEmptyAnode = false;
                    if (shiftTask != null) {
                        if (shiftTask.AnodesReplaceTasks != null) {
                            var replaceTask = shiftTask.AnodesReplaceTasks.FirstOrDefault(r => r.PotNumber == potNumber);
                            if (replaceTask != null) {
                                if (replaceTask.AnodeNumbers != null) {
                                    foreach (var number in replaceTask.AnodeNumbers) {
                                        foreach (var stateDescription in potAnodesStates) {
                                            if (number == stateDescription.AnodeNumber) {
                                                if (stateDescription.OperationTimeString == null) {
                                                    findEmptyAnode = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }                                    
                                }

                                if (!findEmptyAnode) {
                                    replaceTask.operationTime = anodeStateDescription.operationTime;
                                }
                            }
                        }                        
                    }                    

                    return anodeStateDescription;
                }
            }

            throw new WebFaultException<string>(
                    String.Format("Анода с номером корпуса {0}, " +
                                  "номером электролизера {1} и " +
                                  "номером анода {2} не найдено.",
                                  aPotroomNumber, aPotNumber, anodeNumber),
                    HttpStatusCode.NotFound);
        }

        public PotroomPots GetPotroomPots(string aCraneNumber, string aPotroomNumber)
        {
            Console.WriteLine("Запрос электролизеров в корпусе {0} ", aPotroomNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber, "Номер корпуса");            

            var potes = new List<int>();
            foreach (var potModeDescription in potsModes) {
                if (potModeDescription.PotroomNumber == potroomNumber) {
                    potes.Add(potModeDescription.PotNumber);
                }
            }

            if (potes.Count == 0) {
                throw new WebFaultException<string>(
                    string.Format("Корпус с номером {0} не найден.", potroomNumber),
                    HttpStatusCode.NotFound);
            }

            return new PotroomPots {
                PotroomNumber = potroomNumber,
                Pots = potes.ToArray()
            };
        }        

        public BridgeConfiguration GetConfiguration(string aCraneNumber)
        {
            Console.WriteLine("Запрос конфигурации.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            var properties = VersionSettings.Default;
            var colors = ColorsSettings.Default;
            return new BridgeConfiguration {
                Version = properties.Version,
                Source = properties.SourceUrl,
                ShiftTaskInterval = Convert.ToInt32(properties.ShiftTaskRequestInterval),
                Colors = new AnodeStateColor[] {
                    new AnodeStateColor{ Name = "EMPTY", Color  = colors.EMPTY },
                    new AnodeStateColor{ Name = "ANODE_REPLACED", Color  = colors.ANODE_REPLACED },                    
                    new AnodeStateColor{ Name = "NEED_ANODE_REPLACE", Color  = colors.NEED_ANODE_REPLACE },
                    new AnodeStateColor{ Name = "UNSCHEDULED_ANODE_REPLACEMENT", Color  = colors.UNSCHEDULED_ANODE_REPLACEMENT },
                }
            };
        }

        public string GetCurrentTime(string aCraneNumber)
        {
            Console.WriteLine("Запрос текущего времени сервера.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber, "Номер крана");
            Console.WriteLine("Номер крана: " + cranNumber);

            return DateTime.Now.ToString(TimeFormates.iso8601);
        }
        
        #region Инициализация        

        private void LoadAnodesStates()
        {
            var states = new List<AnodeStateDescription>();
            for (var i = 0; i < potsModes.Length; ++i) {
                for (var j = 0; j < 36; ++j) {
                    states.Add(new AnodeStateDescription {
                        PotroomNumber = potsModes[i].PotroomNumber,
                        PotNumber = potsModes[i].PotNumber,
                        AnodeNumber = j+1,
                        state = AnodeState.EMPTY,
                        operationTime = DateTime.MinValue
                    });
                }
            }

            foreach (var task in shiftTasks) {
                foreach (var anodesReplaceTask in task.AnodesReplaceTasks) {
                    foreach (var anodeNumber in anodesReplaceTask.AnodeNumbers) {
                        foreach (var anodeStateDescription in states) {
                            if (anodeStateDescription.PotroomNumber == task.PotroomNumber &&
                                anodeStateDescription.PotNumber == anodesReplaceTask.PotNumber &&
                                anodeStateDescription.AnodeNumber == anodeNumber) {
                                anodeStateDescription.state = AnodeState.NEED_ANODE_REPLACE;                                
                            }
                        }
                    }
                }
            }
            
            anodesStates = states.ToArray();
        }

        private void LoadPotsModes()
        {
            var modes = new List<PotModeDescription>();
            for (var i = 0; i < 2; ++i) {
                for (var j = 0; j < 10; ++j) {
                    modes.Add(new PotModeDescription {
                        PotroomNumber = i+1,
                        PotNumber = j+1,
                        mode = PotMode.AUTO
                    });
                }
            }

            potsModes = modes.ToArray();
        }

        private void LoadTasks()
        {
            var serializer = new XmlSerializer(typeof(ShiftTaskDescription[]));
            using (var file = new FileStream("xml/shiftTask.xml", FileMode.Open)) {
                shiftTasks = (ShiftTaskDescription[])serializer.Deserialize(file);
            }
        }

        private void LoadPotroomsNetwork()
        {
            var serializer = new XmlSerializer(typeof(PotroomNetwork[]));
            using (var file = new FileStream("xml/potroomNetwork.xml", FileMode.Open)) {
                potroomNetwork = (PotroomNetwork[])serializer.Deserialize(file);
            }
        }

        #endregion
    }
}
