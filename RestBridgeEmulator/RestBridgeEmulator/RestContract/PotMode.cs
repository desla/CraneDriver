namespace RestBridgeEmulator.RestContract
{
    /// <summary>
    /// Режим работы электролизера.
    /// </summary>
    public enum PotMode
    {
        /// <summary>
        /// Автоматический.
        /// </summary>
        AUTO,

        /// <summary>
        /// Замена анода.
        /// </summary>	    
        ANODES_REPLACE,
        
        /// <summary>
        /// Замена анодной рамы.
        /// </summary>
        FRAME_CHANGE,

        /// <summary>
        /// Засыпка/
        /// </summary>
        POT_FILL,

        /// <summary>
        /// Заправка бункеров.
        /// </summary>
        HOPPER_FILL
    }
}
