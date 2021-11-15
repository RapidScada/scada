/*
 * Copyright 2021 Mikhail Shiryaev
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
 * Module   : ModDbExport
 * Summary  : Represents a state of snapshot upload process
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2020
 * Modified : 2021
 */

using System;
using System.Text;

namespace Scada.Server.Modules.DbExport
{
    /// <summary>
    /// Represents a state of snapshot upload process.
    /// <para>Представляет состояние процесса загрузки срезов.</para>
    /// </summary>
    internal class ArcUploadState
    {
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ArcUploadState()
        {
            IsReady = true;
            SnapshotIndex = -1;
            SnapshotDate = DateTime.MinValue;
            MinSnapshotDT = DateTime.MinValue;
            MaxSnapshotDT = DateTime.MaxValue;
            QueuedSnapshotDT = DateTime.MinValue;
            SentSnapshotDT = DateTime.MinValue;
        }


        /// <summary>
        /// Gets or sets a value indicating whether a gateway is ready to upload next snapshot.
        /// </summary>
        public bool IsReady { get; set; }

        /// <summary>
        /// Gets or sets the current index in a snapshot table.
        /// </summary>
        public int SnapshotIndex { get; set; }

        /// <summary>
        /// Gets or sets the date to select snapshots.
        /// </summary>
        public DateTime SnapshotDate { get; set; }

        /// <summary>
        /// Gets or sets the minimum snapshot timestamp.
        /// </summary>
        public DateTime MinSnapshotDT { get; set; }

        /// <summary>
        /// Gets or sets the maximum snapshot timestamp.
        /// </summary>
        public DateTime MaxSnapshotDT { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the queued snapshot.
        /// </summary>
        public DateTime QueuedSnapshotDT { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the last snapshot that was successfully sent.
        /// </summary>
        public DateTime SentSnapshotDT { get; set; }


        /// <summary>
        /// Initializes the snapshot date.
        /// </summary>
        public void InitSnapshotDate(int maxAge)
        {
            SnapshotDate = SentSnapshotDT > DateTime.MinValue ?
                SentSnapshotDT.Date :
                DateTime.Today.AddDays(1 - Math.Max(maxAge, 1));
        }

        /// <summary>
        /// Confirms that a snapshot has been sent.
        /// </summary>
        public bool ConfirmSending()
        {
            if (IsReady)
            {
                return false;
            }
            else
            {
                IsReady = true;
                SentSnapshotDT = QueuedSnapshotDT;
                QueuedSnapshotDT = DateTime.MinValue;
                return true;
            }
        }

        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public void AppendInfo(StringBuilder sbInfo)
        {
            if (Localization.UseRussian)
            {
                sbInfo
                    .Append("Готовность     : ").Append(IsReady ? "да" : "нет").AppendLine()
                    .Append("Индекс         : ").Append(SnapshotIndex).AppendLine()
                    .Append("Дата           : ").Append(SnapshotDate.ToLocalizedDateString()).AppendLine()
                    .Append("Мин. время     : ").Append(MinSnapshotDT > DateTime.MinValue ? MinSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .Append("Макс. время    : ").Append(MaxSnapshotDT < DateTime.MaxValue ? MaxSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .Append("В очереди      : ").Append(QueuedSnapshotDT > DateTime.MinValue? QueuedSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .Append("Отправлен      : ").Append(SentSnapshotDT > DateTime.MinValue ? SentSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .AppendLine();
            }
            else
            {
                sbInfo
                    .Append("Ready      : ").Append(IsReady ? "Yes" : "No").AppendLine()
                    .Append("Index      : ").Append(SnapshotIndex).AppendLine()
                    .Append("Date       : ").Append(SnapshotDate.ToLocalizedDateString()).AppendLine()
                    .Append("Min. time  : ").Append(MinSnapshotDT > DateTime.MinValue ? MinSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .Append("Max. time  : ").Append(MaxSnapshotDT < DateTime.MaxValue ? MaxSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .Append("Queued     : ").Append(QueuedSnapshotDT > DateTime.MinValue ? QueuedSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .Append("Sent       : ").Append(SentSnapshotDT > DateTime.MinValue ? SentSnapshotDT.ToLocalizedString() : "---").AppendLine()
                    .AppendLine();
            }
        }

        /// <summary>
        /// Appends information to the string builder.
        /// </summary>
        public static void AppendInfo(ArcUploadState uploadState, StringBuilder sbInfo)
        {
            if (uploadState == null)
            {
                sbInfo
                    .AppendLine(Localization.UseRussian ? "Отключена" : "Disabled")
                    .AppendLine();
            }
            else
            {
                uploadState.AppendInfo(sbInfo);
            }
        }
    }
}
