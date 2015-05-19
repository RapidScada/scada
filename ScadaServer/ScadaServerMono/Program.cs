/*
 * Copyright 2015 Mikhail Shiryaev
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * Product  : Rapid SCADA
 * Module   : SCADA-Server Console Application for Mono .NET framework
 * Summary  : The main entry point for the application
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using Scada.Server.Svc;
using System;
using System.IO;
using System.Threading;

namespace Scada.Server.Mono
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Задержка потока для экономии ресурсов, мс
        /// </summary>
        private const int ThreadDelay = 200;

        /// <summary>
        /// Механизм для получения команды остановки сервиса из файла
        /// </summary>
        private class StopListener
        {
            private string stopFileName; // имя файла остановки сервиса
            private Thread thread;       // поток ожидания файла остановки сервиса

            /// <summary>
            /// Обнаружен файл остановки сервиса
            /// </summary>
            public volatile bool StopFileFound;

            /// <summary>
            /// Конструктор
            /// </summary>
            public StopListener(string stopFileName)
            {
                StopFileFound = false;
                this.stopFileName = stopFileName;
                thread = new Thread(new ThreadStart(WaitForStopFile));
                thread.Priority = ThreadPriority.BelowNormal;
                thread.Start();
            }

            /// <summary>
            /// Ожидать появления файла остановки сервиса
            /// </summary>
            private void WaitForStopFile()
            {
                while (!StopFileFound)
                {
                    if (File.Exists(stopFileName))
                        StopFileFound = true;
                    else
                        Thread.Sleep(ThreadDelay);
                }
            }
            /// <summary>
            /// Удалить файл остановки сервиса
            /// </summary>
            public void DeleteStopFile()
            {
                try
                {
                    File.Delete(stopFileName);
                }
                catch { }
            }
            /// <summary>
            /// Прервать ожидание файла остановки сервиса
            /// </summary>
            public void Abort()
            {
                if (thread != null)
                {
                    thread.Abort();
                    thread = null;
                }
            }
        }


        /// <summary>
        /// Основной цикл работы программы
        /// </summary>
        static void Main(string[] args)
        {
            // запуск службы
            Console.WriteLine("Starting SCADA-Server");
            Manager manager = new Manager();
            manager.StartService();

            Console.WriteLine("SCADA-Server is started");
            Console.WriteLine("Press 'x' or create 'serverstop' file to stop SCADA-Server");

            // остановка службы при нажатии 'x' или обнаружении файла остановки
            StopListener stopListener = new StopListener("Cmd" + Path.DirectorySeparatorChar + "serverstop");
            while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X || stopListener.StopFileFound))
                Thread.Sleep(ThreadDelay);
            manager.StopService();
            stopListener.DeleteStopFile();
            stopListener.Abort();
            Console.WriteLine("SCADA-Server is stopped");
        }
    }
}
