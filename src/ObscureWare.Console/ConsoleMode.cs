namespace ObscureWare.Console
{
    /// <summary>
    /// Specifies working mode of the console
    /// </summary>
    public enum ConsoleMode
    {
        /// <summary>
        /// Buffered, scrolling lines during overflow, cleaning lines on the beginning of the buffer
        /// </summary>
        Buffered,

        /// <summary>
        /// No buffering, no vertical overflow. Has line wrapping. Yet screen-exceeding lines are just dropped
        /// </summary>
        SingleScreen,

        /// <summary>
        /// No buffering, no overflow. No screen wrapping. Exceeding text is just dropped (horizontally and vertically).
        /// </summary>
        SingleScreenNoWrapping
    }
}