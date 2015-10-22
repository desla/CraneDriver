namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Список цветов для состояний анодов.
    /// </summary>
    [DataContract]
    public class AnodeStatesColors
    {
        [DataMember(Order = 1, Name = "EMPTY")]
        public string Empty { get; set; }

        [DataMember(Order = 2, Name = "NEED_ANODE_REPLACE")]
        public string NeedAnodeReplace { get; set; }

        [DataMember(Order = 3, Name = "ANODE_REPLACED")]
        public string AnodeReplaced { get; set; }

        [DataMember(Order = 4, Name = "CANCELED_ANODE_REPLACE")]
        public string CanceledAnodeReplace { get; set; }
    }
}
