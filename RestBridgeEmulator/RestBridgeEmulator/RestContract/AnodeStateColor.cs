namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Список цветов для состояний анодов.
    /// </summary>
    [DataContract]
    public class AnodeStateColor
    {
        [DataMember(Order = 1, Name = "name")]
        public string Name { get; set; }

        [DataMember(Order = 2, Name = "color")]
        public string Color { get; set; }        
    }
}
