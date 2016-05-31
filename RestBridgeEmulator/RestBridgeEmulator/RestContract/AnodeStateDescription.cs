namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.ServiceModel.Web;

    /// <summary>
    /// Описание состояния анода.
    /// </summary>
    [DataContract]
    public class AnodeStateDescription
    {
        [DataMember(Order = 1, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }

        [DataMember(Order = 2, Name = "potNumber")]
        public int PotNumber { get; set; }

        [DataMember(Order = 3, Name = "anodeNumber")]
        public int AnodeNumber { get; set; }

        [DataMember(Order = 4, Name = "currentAnodeState")]
        public string AnodeStateString
        {
            get { return state.ToString(); }
            set
            {                
                var validNames = Enum.GetNames(typeof(AnodeState));
                foreach (var name in validNames) {
                    if (name.Equals(value)) {
                        Enum.TryParse(value, out state);
                        return;
                    }
                }
                var validValues = string.Empty;
                for (var i = 0; i < validNames.Length; ++i) {
                    validValues += validNames[i];
                    if (i < validNames.Length - 1) {
                        validValues += ", ";
                    }
                }
                throw new WebFaultException<string>(
                    string.Format("Не удалось привести '{0}' к AnodeState. " +
                                    "Допустимые значения: {1}", value, validValues),
                    HttpStatusCode.NotAcceptable);             
            }
        }
        
        [DataMember(Order = 5, Name = "operationTime", EmitDefaultValue = false)]
        public string OperationTimeString
        {
            get
            {
                return operationTime == DateTime.MinValue ? 
                    null : 
                    operationTime.ToString(TimeFormates.iso8601);
            }

            set
            {
                if (!DateTime.TryParse(value, out operationTime)) {
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' ко времени.", value),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        [DataMember(Order = 4, Name = "currentCoveringState")]
        public string CoveringStateString {
            get { return coveringState.ToString(); }
            set {
                var validNames = Enum.GetNames(typeof(CoveringState));
                foreach (var name in validNames) {
                    if (name.Equals(value)) {
                        Enum.TryParse(value, out coveringState);
                        return;
                    }
                }
                var validValues = string.Empty;
                for (var i = 0; i < validNames.Length; ++i) {
                    validValues += validNames[i];
                    if (i < validNames.Length - 1) {
                        validValues += ", ";
                    }
                }
                throw new WebFaultException<string>(
                    string.Format("Не удалось привести '{0}' к CoveringState. " +
                                    "Допустимые значения: {1}", value, validValues),
                    HttpStatusCode.NotAcceptable);
            }
        }

        [DataMember(Order = 5, Name = "coveringTime", EmitDefaultValue = false)]
        public string CoveringTimeString {
            get {
                return coveringTime == DateTime.MinValue ?
                    null :
                    coveringTime.ToString(TimeFormates.iso8601);
            }

            set {
                if (!DateTime.TryParse(value, out coveringTime)) {
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' ко времени.", value),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        [IgnoreDataMember]
        public DateTime operationTime;

        [IgnoreDataMember]
        public AnodeState state;

        [IgnoreDataMember]
        public DateTime coveringTime;

        [IgnoreDataMember]
        public CoveringState coveringState;
    }
}
