namespace RestBridgeEmulator.RestContract
{
    /// <summary>
    /// Состояния анода.
    /// </summary>
    public enum AnodeState
    {
        /// <summary>
        /// Пустое состояние.
        /// </summary>
        EMPTY,

        /// <summary>
        /// Требуется замена анода.
        /// </summary>        
        NEED_ANODE_REPLACE,

        /// <summary>
        /// Выполнена замена анода.
        /// </summary>
        ANODE_REPLACED,

        /// <summary>
        /// Отмена выполнения замены анода.
        /// </summary>
        CANCELED_ANODE_REPLACE
    }
}
