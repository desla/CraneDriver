namespace RestBridgeEmulator.Emulator
{
    using System.Net;
    using System.ServiceModel.Web;

    public static class ArgumentConverter
    {
        public static int ToInt32(string aSource, string aDescription = "")
        {
            if (string.IsNullOrEmpty(aSource)) {
                throw new WebFaultException<string>(
                    "Значение '" + aDescription + "' не может быть неопределенным", 
                    HttpStatusCode.InternalServerError);
            }
            
            int result;
            if (!int.TryParse(aSource, out result)) {
                throw new WebFaultException<string>(
                    string.Format("Нельзя преобразовать {0} в Int32 '" + aDescription + "'.", aSource), 
                    HttpStatusCode.InternalServerError);
            }

            return result;
        }
    }
}
