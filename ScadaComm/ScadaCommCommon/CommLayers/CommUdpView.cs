/*
 * Библиотека слоёв связи
 * Пользовательский интерфейс слоя связи UDP
 * 
 * Разработчик:
 * 2015, Ширяев Михаил
 */

using System.Collections.Generic;

namespace Scada.Comm.Layers
{
    /// <summary>
    /// Пользовательский интерфейс слоя связи UDP
    /// </summary>
    public class CommUdpView : CommLayerView
    {
        /// <summary>
        /// Получить наименование слоя связи
        /// </summary>
        public override string Name
        {
            get
            {
                return "UDP";
            }
        }

        /// <summary>
        /// Получить описание слоя связи
        /// </summary>
        public override string Descr
        {
            get
            {
                return Localization.UseRussian ?
                    "Слой связи UDP.\n\n" +
                    "Параметры слоя связи:\n" +
                    "LocalUdpPort - локальный UDP-порт для входящих соединений,\n" +
                    "RemoteUdpPort - удалённый UDP-порт для всех устройств в режиме Master,\n" +
                    "Behavior - режим работы слоя связи (Master, Slave),\n" +
                    "DevSelMode - режим выбора КП (ByIPAddress, ByDeviceLibrary)." :

                    "UDP communication layer.\n\n" +
                    "Communication layer parameters:\n" +
                    "LocalUdpPort - local UDP port for incoming connections." +
                    "RemoteUdpPort - remote UDP port for all the devices in Master mode,\n" +
                    "Behavior - work mode of connection layer (Master, Slave),\n" +
                    "DevSelMode - device selection mode (ByIPAddress, ByDeviceLibrary).";
            }
        }

        /// <summary>
        /// Получить информацию о свойствах слоя связи
        /// </summary>
        public override string GetPropsInfo(Dictionary<string, string> layerParams)
        {
            CommUdpLogic.Settings defSett = new CommUdpLogic.Settings();
            return BuildPropsInfo(layerParams,
                new string[] { "LocalUdpPort", "RemoteUdpPort", "Behavior", "DevSelMode" },
                new object[] { defSett.LocalUdpPort, defSett.RemoteUdpPort, defSett.Behavior, defSett.DevSelMode });
        }
    }
}
