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
            get { return time.ToString(TimeFormates.iso8601); }
            set
            {
                if (!DateTime.TryParse(value, out time)) {
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' ко времени.", value),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        /// <summary>
        /// Плановое время.
        /// </summary>
        [DataMember(Order = 3, Name = "operationTime", EmitDefaultValue = false)]
        public string OperationTimeString
        {
            get 
            { 
                return operationTime == DateTime.MinValue ?
                        null:
                        operationTime.ToString(TimeFormates.iso8601); 
            }
            set
            {
                if (!DateTime.TryParse(value, out operationTime)) {
                    throw new WebFaultException<string>(
                        string.Format("Не удалось привести '{0}' ко времени.", value),
                        HttpStatusCode.NotAcceptable);
                }
            }
        }

        [IgnoreDataMember]
        public DateTime time;

        [IgnoreDataMember]
        public DateTime operationTime;
    }
}
