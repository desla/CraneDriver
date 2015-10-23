namespace RestBridgeEmulator.RestContract
{
    using System;
    using System.ComponentModel;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    /// Контракт - API для rest-сервиса.
    /// </summary>
    [ServiceContract]
    public interface IRestBridgeContract
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotroomsNetwork/?cranNumber={aCraneNumber}")]
        [Description("Возвращает список соответствия ssid точек доступа wi-fi к номеру корпуса завода.")]
        PotroomNetwork[] GetPotroomsNetwork(string aCraneNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getShiftTask/?cranNumber={aCraneNumber}" +
                                               "&potroomNumber={aPotroomNumber}")]
        [Description("Возвращает полное описание задания на смену.")]
        ShiftTaskDescription GetTaskDescription(string aCraneNumber, string aPotroomNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotMode/?cranNumber={aCraneNumber}" +
                                             "&potroomNumber={aPotroomNumber}&potNumber={aPotNumber}")]
        [Description("Возвращает текущий режим работы электролизера.")]
        PotModeDescription GetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/setPotMode/?cranNumber={aCraneNumber}" +
                                             "&potroomNumber={aPotroomNumber}" +
                                             "&potNumber={aPotNumber}" +
                                             "&potMode={aPotMode}")]
        [Description("Выполняет запрос на изменение режима работы электролизера.")]
        PotModeDescription SetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber, string aPotMode);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getAnodesStates/?cranNumber={aCraneNumber}" +
                                                  "&potroomNumber={aPotroomNumber}" +
                                                  "&potNumber={aPotNumber}")]
        [Description("Возвращает текущие статусы всех анодов в корпусе на электролизере.")]
        AnodeStateDescription[] GetAnodesStates(string aCraneNumber, string aPotroomNumber, string aPotNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/setAnodeState/?cranNumber={aCraneNumber}" +
                                                "&potroomNumber={aPotroomNumber}" +
                                                "&potNumber={aPotNumber}" +
                                                "&anodeNumber={aAnodeNumber}" +
                                                "&anodeState={aAnodeState}")]
        [Description("Возвращает текущие статусы всех анодов в корпусе на электролизере.")]
        AnodeStateDescription SetAnodeState(string aCraneNumber, string aPotroomNumber, string aPotNumber, 
            string aAnodeNumber, string aAnodeState);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotroomPotes/?cranNumber={aCraneNumber}" +
                                                  "&potroomNumber={aPotroomNumber}")]
        [Description("Возвращает список всех анодов в корпусе.")]
        PotroomPotes GetPotroomPotes(string aCraneNumber, string aPotroomNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getAnodeStatesColors/?cranNumber={aCraneNumber}")]
        [Description("Возвращает список цветов для состояний анодов.")]
        AnodeStatesColors GetAnodeStatesColors(string aCraneNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getSoftDescription/?cranNumber={aCraneNumber}")]
        [Description("Возвращает текущее описание ПО.")]
        SoftDescription GetSoftDescription(string aCraneNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getShiftTaskInterval/?cranNumber={aCraneNumber}")]
        [Description("Возвращает интервал запроса задания на смену.")]
        ShiftTaskInterval GetShiftTaskInterval(string aCraneNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getCurrentTime/?cranNumber={aCraneNumber}")]
        [Description("Возвращает текущее время сервера.")]
        string GetCurrentTime(string aCraneNumber);
    }
}
