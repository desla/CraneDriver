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
                   UriTemplate = "/getPotroomsNetwork/?craneNumber={aCraneNumber}")]
        [Description("Возвращает список соответствия ssid точек доступа wi-fi к номеру корпуса завода.")]
        PotroomNetwork[] GetPotroomsNetwork(string aCraneNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getShiftTask/?craneNumber={aCraneNumber}" +
                                               "&potroomNumber={aPotroomNumber}")]
        [Description("Возвращает полное описание задания на смену.")]
        ShiftTaskDescription GetTaskDescription(string aCraneNumber, string aPotroomNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/addPotTask/?craneNumber={aCraneNumber}" +
                                             "&potroomNumber={aPotroomNumber}" +
                                             "&potNumber={aPotNumber}" +
                                             "&taskType={aTaskType}")]
        [Description("Добавляет задачу в сменное задание.")]
        PotTask AddPotTask(string aCraneNumber, string aPotroomNumber, 
            string aPotNumber, string aTaskType);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotMode/?craneNumber={aCraneNumber}" +
                                             "&potroomNumber={aPotroomNumber}&potNumber={aPotNumber}")]
        [Description("Возвращает текущий режим работы электролизера.")]
        PotModeDescription GetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/setPotMode/?craneNumber={aCraneNumber}" +
                                             "&potroomNumber={aPotroomNumber}" +
                                             "&potNumber={aPotNumber}" +
                                             "&potMode={aPotMode}")]
        [Description("Выполняет запрос на изменение режима работы электролизера.")]
        PotModeDescription SetPotMode(string aCraneNumber, string aPotroomNumber, string aPotNumber, string aPotMode);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getAnodesStates/?craneNumber={aCraneNumber}" +
                                                  "&potroomNumber={aPotroomNumber}" +
                                                  "&potNumber={aPotNumber}")]
        [Description("Возвращает текущие статусы всех анодов в корпусе на электролизере.")]
        AnodeStateDescription[] GetAnodesStates(string aCraneNumber, string aPotroomNumber, string aPotNumber);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/setAnodeState/?craneNumber={aCraneNumber}" +
                                                "&potroomNumber={aPotroomNumber}" +
                                                "&potNumber={aPotNumber}" +
                                                "&anodeNumber={aAnodeNumber}" +
                                                "&anodeState={aAnodeState}")]
        [Description("Возвращает текущие статусы всех анодов в корпусе на электролизере.")]
        AnodeStateDescription SetAnodeState(string aCraneNumber, string aPotroomNumber, string aPotNumber, 
            string aAnodeNumber, string aAnodeState);

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getPotroomPots/?craneNumber={aCraneNumber}" +
                                                 "&potroomNumber={aPotroomNumber}")]
        [Description("Возвращает список всех анодов в корпусе.")]
        PotroomPots GetPotroomPots(string aCraneNumber, string aPotroomNumber);        

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getConfiguration/?craneNumber={aCraneNumber}")]
        [Description("Возвращает текущее описание ПО.")]
        BridgeConfiguration GetConfiguration(string aCraneNumber);        

        [OperationContract]
        [WebInvoke(Method = "GET",
                   UriTemplate = "/getCurrentTime/?craneNumber={aCraneNumber}")]
        [Description("Возвращает текущее время сервера.")]
        string GetCurrentTime(string aCraneNumber);
    }
}
