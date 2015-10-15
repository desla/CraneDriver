namespace RestBridgeEmulator.RestContract
{
    using System.ComponentModel;
    using System.ServiceModel;
    using System.ServiceModel.Web;

    /// <summary>
    /// Контракт - API для rest-сервиса.
    /// </summary>
    [ServiceContract]
    internal interface IRestBridgeContract
    {
        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotroomsNetwork/")]
        [Description("Возвращает список соответствия ssid точек доступа wi-fi к номеру корпуса завода.")]
        PotroomNetwork[] GetPotroomsNetwork();

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getShiftTask/?potroomNumber={aPotroomNumber}")]
        [Description("Возвращает полное описание задания на смену.")]
        TaskDescription GetTaskDescription(string aPotroomNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotMode/?potroomNumber={aPotroomNumber}&potNumber={aPotNumber}")]
        [Description("Возвращает текущий режим работы электролизера.")]
        PotModeDescription GetPotMode(string aPotroomNumber, string aPotNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/setPotMode/?potroomNumber={aPotroomNumber}" +
                                             "&potNumber={aPotNumber}" +
                                             "&potMode={aPotMode}")]
        [Description("Выполняет запрос на изменение режима работы электролизера.")]
        PotMode SetPotMode(string aPotroomNumber, string aPotNumber, string aPotMode);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getAnodesStates/?potroomNumber={aPotroomNumber}" +
                                                    "&potNumber={aPotNumber}")]
        [Description("Возвращает текущие статусы всех анодов в корпусе на электролизере.")]
        AnodeStateDescription[] GetAnodesStates(string aPotroomNumber, string aPotNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/setAnodeState/?potroomNumber={aPotroomNumber}" +
                                                "&potNumber={aPotNumber}" +
                                                "&anodeNumber={aAnodeNumber}" +
                                                "&anodeState={aAnodeState}")]
        [Description("Возвращает текущие статусы всех анодов в корпусе на электролизере.")]
        AnodeStateDescription SetAnodeState(string aPotroomNumber, string aPotNumber, string aAnodeNumber, string aAnodeState);
    }
}
