namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Интервал запроса заданий на смену.
    /// </summary>
    [DataContract]
    public class ShiftTaskInterval
    {
        [DataMember(Order = 1, Name = "interval")]
        public int Interval { get; set; }
    }
}
