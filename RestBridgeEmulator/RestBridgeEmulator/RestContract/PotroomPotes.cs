namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Список электролизеров корпуса.
    /// </summary>
    [DataContract]
    public class PotroomPotes
    {
        [DataMember(Order = 1, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }

        [DataMember(Order = 2, Name = "potes")]
        public int[] Potes { get; set; }
    }
}
