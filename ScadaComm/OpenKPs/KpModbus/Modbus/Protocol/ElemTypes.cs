namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Типы элементов
    /// </summary>
    public enum ElemTypes
    {
        /// <summary>
        /// Логическое значение
        /// </summary>
        Bool,

        /// <summary>
        /// 2-байтное целое без знака
        /// </summary>
        UShort,

        /// <summary>
        /// 2-байтное целое со знаком
        /// </summary>
        Short,

        /// <summary>
        /// 4-байтное целое без знака
        /// </summary>
        UInt,

        /// <summary>
        /// 4-байтное целое со знаком
        /// </summary>
        Int,

        /// <summary>
        /// 8-байтное целое без знака
        /// </summary>
        ULong,

        /// <summary>
        /// 8-байтное целое со знаком
        /// </summary>
        Long,

        /// <summary>
        /// 4-байтное вещественное с плавающей запятой
        /// </summary>
        Float,

        /// <summary>
        /// 8-байтное вещественное с плавающей запятой
        /// </summary>
        Double
    }

}
