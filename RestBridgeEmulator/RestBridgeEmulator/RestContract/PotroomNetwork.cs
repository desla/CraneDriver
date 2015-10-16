namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Описание связки SSID точки досутпа - номер корпуса.
    /// </summary>
    [DataContract]
    public class PotroomNetwork
    {
        /// <summary>
        /// SSID точки доступа.
        /// </summary>
        [DataMember(Order = 1, Name = "ssid")]
        public string Ssid { get; set; }

        /// <summary>
        /// Номер корпуса.
        /// </summary>
        [DataMember(Order = 2, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }
    }
}
