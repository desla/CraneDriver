namespace RestBridgeEmulator.RestContract {

    /// <summary>
    /// Состояния засыпки анода.
    /// </summary>
    public enum CoveringState {
        /// <summary>
        /// Пустое состояние.
        /// </summary>
        EMPTY,

        /// <summary>
        /// Требуется засыпка анода.
        /// </summary>        
        NEED_ANODE_COVERING,

        /// <summary>
        /// Выполнена засыпка анода.
        /// </summary>
        ANODE_COVERED,

        /// <summary>
        /// Отмена выполнения засыпки анода.
        /// </summary>
        CANCELED_ANODE_COVERING
    }
}
