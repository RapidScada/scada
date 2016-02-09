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
 * Module   : ScadaData
 * Summary  : File creation listener
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2015
 * Modified : 2015
 */

using System.IO;
using System.Threading;

namespace Scada
{
    /// <summary>
    /// File creation listener
    /// <para>Прослушиватель создания файла</para>
    /// <remarks>
    /// The class is used for receiving stop service command when running on Mono .NET framework
    /// <para>Класс используется для получения команды остановки службы при выполнении в Mono .NET framework</para>
    /// </remarks>
    /// </summary>
    public class FileListener
    {
        private string fileName; // имя ожидаемого файла
        private Thread thread;   // поток ожидания файла

        /// <summary>
        /// Обнаружен ожидаемый файл
        /// </summary>
        public volatile bool FileFound;


        /// <summary>
        /// Конструктор, ограничивающий создание объекта без параметров
        /// </summary>
        protected FileListener()
        {

        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public FileListener(string fileName)
        {
            FileFound = false;
            this.fileName = fileName;
            thread = new Thread(new ThreadStart(WaitForFile));
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }


        /// <summary>
        /// Ожидать появления файла
        /// </summary>
        private void WaitForFile()
        {
            while (!FileFound)
            {
                if (File.Exists(fileName))
                    FileFound = true;
                else
                    Thread.Sleep(ScadaUtils.ThreadDelay);
            }
        }

        /// <summary>
        /// Удалить файл
        /// </summary>
        public void DeleteFile()
        {
            try
            {
                File.Delete(fileName);
            }
            catch { }
        }

        /// <summary>
        /// Прервать ожидание файла
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
}
