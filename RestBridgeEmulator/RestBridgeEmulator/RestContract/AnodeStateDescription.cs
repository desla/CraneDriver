namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.ServiceModel.Web;

    /// <summary>
    /// Описание состояния анода.
    /// </summary>
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
                if (!Enum.TryParse(value, out state)) {
                    var names = Enum.GetNames(typeof(AnodeState));
                    var validValues = string.Empty;
                    for (var i = 0; i < names.Length; ++i) {
                        validValues += names[0];
                        if (i < names.Length - 1) {
                            validValues += ", ";
                        }
                    }
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' к AnodeState. " +
                                      "Допустимые значения: {1}", value, validValues),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        [IgnoreDataMember]
        public AnodeState state;
    }
}
