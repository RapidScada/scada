namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Типы таблиц данных
    /// </summary>
    public enum TableTypes
    {
        /// <summary>
        /// Дискретные входы (1 бит, только чтение, 1X-обращения)
        /// </summary>
        DiscreteInputs,

        /// <summary>
        /// Флаги (1 бит, чтение и запись, 0X-обращения)
        /// </summary>
        Coils,

        /// <summary>
        /// Входные регистры (16 бит, только чтение, 3X-обращения)
        /// </summary>
        InputRegisters,

        /// <summary>
        /// Регистры хранения (16 бит, чтение и запись, 4X-обращения)
        /// </summary>
        HoldingRegisters
    }
}
