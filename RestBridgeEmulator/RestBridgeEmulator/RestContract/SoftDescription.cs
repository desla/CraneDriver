namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Описание версии ПО и ссылка для скачивания новой прошивки.
    /// </summary>
    [DataContract]
    public class SoftDescription
    {
        [DataMember(Order = 1, Name = "version")]
        public int Version { get; set; }

        [DataMember(Order = 2, Name = "source")]
        public string Source { get; set; }
    }
}
