namespace Scada.Comm.Devices.Modbus.Protocol
{
    /// <summary>
    /// Элемент
    /// </summary>
    public class Elem
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Elem()
        {
            Name = "";
            ElemType = ElemTypes.Bool;
            ByteOrder = null;
            ByteOrderStr = "";
        }


        /// <summary>
        /// Получить или установить наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Получить или установить тип
        /// </summary>
        public ElemTypes ElemType { get; set; }

        /// <summary>
        /// Получить длину элемента (количество адресов)
        /// </summary>
        public int Length
        {
            get
            {
                return GetElemLength(ElemType);
            }
        }

        /// <summary>
        /// Получить или установить массив, определяющий порядок байт
        /// </summary>
        public int[] ByteOrder { get; set; }

        /// <summary>
        /// Получить или установить строковую запись порядка байт
        /// </summary>
        public string ByteOrderStr { get; set; }


        /// <summary>
        /// Инициализировать массив, определяющий порядок байт, на основе строково записи вида '01234567'
        /// </summary>
        public void InitByteOrder(string orderStr)
        {
            if (string.IsNullOrEmpty(orderStr))
            {
                ByteOrder = null;
                ByteOrderStr = "";
            }
            else
            {
                ByteOrderStr = orderStr;
                int len = orderStr.Length;
                ByteOrder = new int[len];

                for (int i = 0; i < len; i++)
                {
                    int n;
                    ByteOrder[i] = int.TryParse(orderStr[i].ToString(), out n) ? n : 0;
                }
            }
        }

        /// <summary>
        /// Получить длину элемента (количество адресов) в зависимости от типа элемента
        /// </summary>
        public static int GetElemLength(ElemTypes elemType)
        {
            switch (elemType)
            {
                case ElemTypes.ULong:
                case ElemTypes.Long:
                case ElemTypes.Double:
                    return 4;
                case ElemTypes.UInt:
                case ElemTypes.Int:
                case ElemTypes.Float:
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
