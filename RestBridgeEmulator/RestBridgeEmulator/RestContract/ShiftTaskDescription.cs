namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.Net;
    using System.Runtime.Serialization;
    using System.ServiceModel.Web;

    /// <summary>
    /// Полное задание на смену в корпусе.
    /// </summary>
    [DataContract]
    public class ShiftTaskDescription
    {
        /// <summary>
        /// Номер корпуса.
        /// </summary>
        [DataMember(Order = 1, Name = "potroomNumber")]
        public int PotroomNumber { get; set; }

        [DataMember(Order = 2, Name = "lastUpdateTime")]
        public string LastUpdateTimeString
        {
            get
            {
                return lastUpdateTime == DateTime.MinValue 
                    ? null : 
                    lastUpdateTime.ToString(TimeFormates.iso8601);
            }

            set
            {
                if (!DateTime.TryParse(value, out lastUpdateTime)) {
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' ко времени.", value),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }        

        /// <summary>
        /// Задания для замены анодов.
        /// </summary>
        [DataMember(Order = 3, Name = "anodesReplace")]
        public AnodesReplaceTask[] AnodesReplaceTasks { get; set; }

        /// <summary>
        /// Задания для операции "Перетяжка".
        /// </summary>
        [DataMember(Order = 4, Name = "frameChange")]
        public PotTask[] FrameChangeTasks { get; set; }

        /// <summary>
        /// Задания для операции "Засыпка".
        /// </summary>
        [DataMember(Order = 5, Name = "potFill")]
        public PotTask[] PotFillTasks { get; set; }

        /// <summary>
        /// Задания для операции "Засыпка бункеров".
        /// </summary>
        [DataMember(Order = 6, Name = "hopperFill")]
        public PotTask[] HopperFillTasks { get; set; }

        [IgnoreDataMember]
        public DateTime lastUpdateTime;
    }
}
