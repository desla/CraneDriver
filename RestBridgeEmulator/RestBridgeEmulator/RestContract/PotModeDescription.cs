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
    internal class PotModeDescription
    {
        [DataMember(Order = 1, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }

        [DataMember(Order = 2, Name = "potNumber")]
        public int PotNumber { get; set; }

        [DataMember(Order = 3, Name = "currentPotMode")]
        public string PotModeString
        {
            get { return mode.ToString(); }
            set
            {
                if (!Enum.TryParse(value, out mode)) {
                    var names = Enum.GetNames(typeof (PotMode));
                    var validValues = string.Empty;
                    for (var i = 0; i < names.Length; ++i) {
                        validValues += names[0];
                        if (i < names.Length - 1) {
                            validValues += ", ";
                        }
                    }
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' к PotMode. " +
                                      "Допустимые значения: {1}", value, validValues),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        [NonSerialized]
        public PotMode mode;
    }
}
