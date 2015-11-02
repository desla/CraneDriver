namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Описание версии ПО и ссылка для скачивания новой прошивки.
    /// </summary>
    [DataContract]
    public class BridgeConfiguration
    {
        [DataMember(Order = 1, Name = "version")]
        public string Version { get; set; }

        [DataMember(Order = 2, Name = "source")]
        public string Source { get; set; }

        [DataMember(Order = 3, Name = "shiftTaskInterval")]
        public int ShiftTaskInterval { get; set; }

        [DataMember(Order = 4, Name = "colors")]
        public AnodeStateColor[] Colors { get; set; }
    }
}
