namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.Globalization;
    using System.Net;
    using System.Runtime.Serialization;
    using System.ServiceModel.Web;

    /// <summary>
    /// Задание для операции "Замена анодов".
    /// </summary>
    [DataContract]
    internal class AnodesReplaceTask
    {
        /// <summary>
        /// Номер электролизера.
        /// </summary>
        [DataMember(Order = 1, Name = "potNumber")]
        public int PotNumber { get; set; }
        
        /// <summary>
        /// Плановое время в строковом виде.
        /// </summary>
        [DataMember(Order = 2, Name = "time")]
        public string TimeString {
            get { return time.ToString(CultureInfo.CurrentCulture); }
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
        /// Номера анодов для замены.
        /// </summary>
        [DataMember(Order = 3, Name = "anodes")]
        public int[] AnodeNumbers { get; set; }

        /// <summary>
        /// Комментарии к анодам.
        /// </summary>
        [DataMember(Order = 4, Name = "anodesComments")]
        public Comment[] Comments { get; set; }

        [NonSerialized]
        public DateTime time;
    }
}
