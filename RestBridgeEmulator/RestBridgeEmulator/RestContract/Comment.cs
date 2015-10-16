namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Комментарии к анодам для замены.
    /// </summary>
    [DataContract]
    public class Comment
    {
        [DataMember(Order = 1, Name = "anodeNumber")]
        public int AnodeNumber { get; set; }

        [DataMember(Order = 2, Name = "comment")]
        public string Text { get; set; }
    }
}
