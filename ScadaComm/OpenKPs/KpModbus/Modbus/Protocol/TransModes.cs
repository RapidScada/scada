namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Режимы передачи данных
    /// </summary>
    public enum TransModes
    {
        /// <summary>
        /// Передача данных через последовательный порт в бинарном формате
        /// </summary>
        RTU,

        /// <summary>
        /// Передача данных через последовательный порт в символьном формате
        /// </summary>
        ASCII,

        /// <summary>
        /// Передача данных через локальную сеть по протоколу TCP/IP
        /// </summary>
        TCP
    }
}
