namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Runtime.Serialization;
    using System.ServiceModel.Web;

    /// <summary>
    /// Данные для операции "Перетяжка".
    /// </summary>
    [DataContract]
    public class PotTask
    {
        /// <summary>
        /// Номер электролизера.
        /// </summary>
        [DataMember(Order = 1, Name = "potNumber")]
        public int PotNumber { get; set; }

        /// <summary>
        /// Плановое время.
        /// </summary>
        [DataMember(Order = 2, Name = "time")]
        public string TimeString
        {
            get { return time.ToString("o"); }
            set
            {
                if (!DateTime.TryParse(value, out time)) {
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' ко времени.", value),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        [IgnoreDataMember]
        public DateTime time;
    }
}
