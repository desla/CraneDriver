namespace RestBridgeEmulator.Emulator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
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
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            return potroomNetwork;
        }

        public ShiftTaskDescription GetTaskDescription(string aCraneNumber, string aPotroomNumber)
        {
            Console.WriteLine("Запрос сменного задания для корпуса " + aPotroomNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber);

            foreach (var taskDescription in shiftTasks) {
                if (taskDescription.PotroomNumber == potroomNumber) {
                    return taskDescription;
                }
            }

            throw new WebFaultException<string>(
                    String.Format("Корпус с номером {0} не найден.", potroomNumber),
                    HttpStatusCode.NotFound);
        }

        public PotModeDescription GetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber)
        {
            Console.WriteLine("Запрос режима работы электролизера {0} в корпусе {1}", aPotNumber, aPotroomNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber);
            var potNumber = ArgumentConverter.ToInt32(aPotNumber);

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
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber);
            var potNumber = ArgumentConverter.ToInt32(aPotNumber);      

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
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber);
            var potNumber = ArgumentConverter.ToInt32(aPotNumber);

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
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber);
            var potNumber = ArgumentConverter.ToInt32(aPotNumber);
            var anodeNumber = ArgumentConverter.ToInt32(aAnodeNumber);

            for (var i = 0; i < anodesStates.Length; ++i) {             
                var anodeStateDescription = anodesStates[i];
                if (anodeStateDescription.PotroomNumber == potroomNumber &&
                    anodeStateDescription.PotNumber == potNumber &&
                    anodeStateDescription.AnodeNumber == anodeNumber) {

                    anodeStateDescription.AnodeStateString = aAnodeState;                    
                    anodeStateDescription.operationTime = DateTime.Now;                    

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

        public PotroomPotes GetPotroomPotes(string aCraneNumber, string aPotroomNumber)
        {
            Console.WriteLine("Запрос электролизеров в корпусе {0} ", aPotroomNumber);
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var potroomNumber = ArgumentConverter.ToInt32(aPotroomNumber);            

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

            return new PotroomPotes {
                PotroomNumber = potroomNumber,
                Potes = potes.ToArray()
            };
        }

        public AnodeStatesColors GetAnodeStatesColors(string aCraneNumber)
        {
            Console.WriteLine("Запрос цветов для состояний анодов.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var properties = ColorsSettings.Default;
            return new AnodeStatesColors {
                Empty = properties.EMPTY,
                AnodeReplaced = properties.ANODE_REPLACED,
                CanceledAnodeReplace = properties.CANCELED_ANODE_REPLACE,
                NeedAnodeReplace = properties.NEED_ANODE_REPLACE
            };
        }

        public SoftDescription GetSoftDescription(string aCraneNumber)
        {
            Console.WriteLine("Запрос версии ПО и ссылки для скачивания APK файла.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var properties = VersionSettings.Default;
            return new SoftDescription {
                Version = Convert.ToInt32(properties.Version),
                Source = properties.SourceUrl
            };
        }

        public ShiftTaskInterval GetShiftTaskInterval(string aCraneNumber)
        {
            Console.WriteLine("Запрос интервала получения заданий на смену.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            var properties = VersionSettings.Default;
            return new ShiftTaskInterval {
                Interval = Convert.ToInt32(properties.ShiftTaskRequestInterval)
            };
        }

        public string GetCurrentTime(string aCraneNumber)
        {
            Console.WriteLine("Запрос текущего времени сервера.");
            var cranNumber = ArgumentConverter.ToInt32(aCraneNumber);
            Console.WriteLine("Номер крана: " + cranNumber);

            return DateTime.Now.ToString("o");
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
