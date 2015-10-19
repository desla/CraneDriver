namespace RestBridgeEmulator.Emulator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Xml.Serialization;
    using RestContract;

    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Single)]
    internal class CastDriverBridgeEmulatorImpl : IRestBridgeContract
    {
        private PotroomNetwork[] potroomNetwork;
        private TaskDescription[] tasks;
        private PotModeDescription[] potsModes;
        private AnodeStateDescription[] anodesStates;

        public CastDriverBridgeEmulatorImpl()
        {
            LoadPotroomsNetwork();
            LoadTasks();
            LoadPotsModes();
            LoadAnodesStates();
        }        

        public PotroomNetwork[] GetPotroomsNetwork()
        {
            Console.WriteLine("Запрос списка соответствия ssid точек доступа номерам корпусов.");
            return potroomNetwork;
        }

        public TaskDescription GetTaskDescription(string aPotroomNumber)
        {
            Console.WriteLine("Запрос сменного задания для корпуса " + aPotroomNumber);
            int potroomNumber;
            if (!int.TryParse(aPotroomNumber, out potroomNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotroomNumber), 
                    HttpStatusCode.InternalServerError);
            }

            foreach (var taskDescription in tasks) {
                if (taskDescription.PotroomNumber == potroomNumber) {
                    return taskDescription;
                }
            }

            throw new WebFaultException<string>(
                    String.Format("Корпус с номером {0} не найден.", potroomNumber),
                    HttpStatusCode.NotFound);
        }

        public PotModeDescription GetPotMode(string aPotroomNumber, string aPotNumber)
        {
            Console.WriteLine("Запрос режима работы электролизера {0} в корпусе {1}", aPotNumber, aPotroomNumber);
            int potroomNumber;
            if (!int.TryParse(aPotroomNumber, out potroomNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotroomNumber),
                    HttpStatusCode.InternalServerError);
            }

            int potNumber;
            if (!int.TryParse(aPotNumber, out potNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotNumber),
                    HttpStatusCode.InternalServerError);
            }

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

        public PotModeDescription SetPotMode(string aPotroomNumber, string aPotNumber, string aPotMode)
        {
            Console.WriteLine("Запрос на изменение режима работы электролизера {0} " +
                              "в корпусе {1}, режим: {2}", aPotNumber, aPotroomNumber, aPotMode);
            int potroomNumber;
            if (!int.TryParse(aPotroomNumber, out potroomNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotroomNumber),
                    HttpStatusCode.InternalServerError);
            }

            int potNumber;
            if (!int.TryParse(aPotNumber, out potNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotNumber),
                    HttpStatusCode.InternalServerError);
            }            

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

        public AnodeStateDescription[] GetAnodesStates(string aPotroomNumber, string aPotNumber)
        {
            Console.WriteLine("Запрос состояния анодов в корпусе {0} " +
                              "на электролизере {1}", aPotroomNumber, aPotNumber);
            int potroomNumber;
            if (!int.TryParse(aPotroomNumber, out potroomNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotroomNumber),
                    HttpStatusCode.InternalServerError);
            }

            int potNumber;
            if (!int.TryParse(aPotNumber, out potNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotNumber),
                    HttpStatusCode.InternalServerError);
            }
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

        public AnodeStateDescription SetAnodeState(string aPotroomNumber, string aPotNumber, string aAnodeNumber, string aAnodeState)
        {
            Console.WriteLine("Запрос на изменение состояния анода {0} " +
                              "в корпусе {1} на электролизере {2}, состояние: {3}", 
                              aAnodeNumber, aPotroomNumber, aPotNumber, aAnodeState);
            int potroomNumber;
            if (!int.TryParse(aPotroomNumber, out potroomNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotroomNumber),
                    HttpStatusCode.InternalServerError);
            }

            int potNumber;
            if (!int.TryParse(aPotNumber, out potNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aPotNumber),
                    HttpStatusCode.InternalServerError);
            }

            int anodeNumber;
            if (!int.TryParse(aAnodeNumber, out anodeNumber)) {
                throw new WebFaultException<string>(
                    String.Format("Не удалось привести {0} к типу int.", aAnodeNumber),
                    HttpStatusCode.InternalServerError);
            }

            for (var i = 0; i < anodesStates.Length; ++i) {             
                var anodeStateDescription = anodesStates[i];
                if (anodeStateDescription.PotroomNumber == potroomNumber &&
                    anodeStateDescription.PotNumber == potNumber &&
                    anodeStateDescription.AnodeNumber == anodeNumber) {

                    anodeStateDescription.AnodeStateString = aAnodeState;                    
                    anodeStateDescription.operationTime = DateTime.Now;

                    if (anodeStateDescription.state == AnodeState.CANCELED_ANODE_REPLACE) {
                        anodeStateDescription.state = AnodeState.NEED_ANODE_REPLACE;
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

            foreach (var task in tasks) {
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
            var serializer = new XmlSerializer(typeof(TaskDescription[]));
            using (var file = new FileStream("xml/shiftTask.xml", FileMode.Open)) {
                tasks = (TaskDescription[])serializer.Deserialize(file);
            }
        }

        private void LoadPotroomsNetwork()
        {
            var serializer = new XmlSerializer(typeof(PotroomNetwork[]));
            using (var file = new FileStream("xml/potroomNetwork.xml", FileMode.Open)) {
                potroomNetwork = (PotroomNetwork[])serializer.Deserialize(file);
            }
        }
    }
}
