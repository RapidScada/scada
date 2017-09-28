namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Коды функций
    /// </summary>
    public static class FuncCodes
    {
        /// <summary>
        /// Считать дискретные входы
        /// </summary>
        public const byte ReadDiscreteInputs = 0x02;

        /// <summary>
        /// Считать флаги
        /// </summary>
        public const byte ReadCoils = 0x01;

        /// <summary>
        /// Считать входные регистры
        /// </summary>
        public const byte ReadInputRegisters = 0x04;

        /// <summary>
        /// Считать регистры хранения
        /// </summary>
        public const byte ReadHoldingRegisters = 0x03;

        /// <summary>
        /// Записать флаг
        /// </summary>
        public const byte WriteSingleCoil = 0x05;

        /// <summary>
        /// Записать регистр хранения
        /// </summary>
        public const byte WriteSingleRegister = 0x06;

        /// <summary>
        /// Записать множество флагов
        /// </summary>
        public const byte WriteMultipleCoils = 0x0F;

        /// <summary>
        /// Записать множество регистров хранения
        /// </summary>
        public const byte WriteMultipleRegisters = 0x10;
    }
}
