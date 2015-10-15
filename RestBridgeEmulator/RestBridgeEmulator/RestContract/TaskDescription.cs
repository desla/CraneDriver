namespace RestBridgeEmulator.RestContract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Полное задание на смену в корпусе.
    /// </summary>
    [DataContract]
    internal class TaskDescription
    {
        /// <summary>
        /// Номер корпуса.
        /// </summary>
        [DataMember(Order = 1, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }

        /// <summary>
        /// Задания для замены анодов.
        /// </summary>
        [DataMember(Order = 2, Name = "anodesReplace")]
        public AnodesReplaceTask[] AnodesReplaceTasks { get; set; }

        /// <summary>
        /// Задания для операции "Перетяжка".
        /// </summary>
        [DataMember(Order = 3, Name = "frameChange")]
        public PotTask[] FrameChangeTasks { get; set; }

        /// <summary>
        /// Задания для операции "Засыпка".
        /// </summary>
        [DataMember(Order = 4, Name = "potFill")]
        public PotTask[] PotFillTasks { get; set; }

        /// <summary>
        /// Задания для операции "Засыпка бункеров".
        /// </summary>
        [DataMember(Order = 5, Name = "hopperFill")]
        public PotTask[] HopperFillTasks { get; set; }
    }
}
