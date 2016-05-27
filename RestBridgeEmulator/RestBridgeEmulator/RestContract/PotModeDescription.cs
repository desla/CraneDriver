namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.ServiceModel.Web;

    /// <summary>
    /// Описание режима работы электролизера.
    /// </summary>
    [DataContract]
    public class PotModeDescription
    {
        [DataMember(Order = 1, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }

        [DataMember(Order = 2, Name = "potNumber")]
        public int PotNumber { get; set; }

        [DataMember(Order = 3, Name = "currentPotMode")]
        public string PotModeString
        {
            get {
                if (mode == null) {
                    return null;
                }                
                return mode.ToString();
            }
            set
            {                
                if (value == null) {
                    mode = null;
                    return;
                }                
                var validNames = Enum.GetNames(typeof(PotMode));
                foreach (var name in validNames) {
                    if (name.Equals(value)) {
                        PotMode tMode;
                        Enum.TryParse(value, out tMode);
                        mode = tMode;
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
                    string.Format("Не удалось привести '{0}' к PotMode. " +
                                    "Допустимые значения: {1}", value, validValues),
                    HttpStatusCode.NotAcceptable);                
            }
        }

        [IgnoreDataMember]
        public PotMode? mode;
    }
}
