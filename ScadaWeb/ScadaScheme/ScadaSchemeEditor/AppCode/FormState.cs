/*
 * Copyright 2019 Mikhail Shiryaev
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
 * Module   : Scheme Editor
 * Summary  : Form state
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2017
 * Modified : 2019
 */

using Scada.UI;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Scada.Scheme.Editor
{
    /// <summary>
    /// Form state.
    /// <para>Состояние формы.</para>
    /// </summary>
    internal class FormState
    {
        /// <summary>
        /// Расстояние притягивания к краям экрана.
        /// </summary>
        private const int PullDistance = 10;
        /// <summary>
        /// Высота панели инструментов браузера (на примере Chrome).
        /// </summary>
        private const int BrowserToolbarHeight = 70;
        /// <summary>
        /// Имя файла состояния по умолчанию.
        /// </summary>
        public const string DefFileName = "ScadaSchemeEditorState.xml";

        public int screenWidth; // ширина экрана
        public int formAdj;     // поправка позиции и размера формы


        /// <summary>
        /// Конструктор.
        /// </summary>
        public FormState()
        {
            SetToDefault();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public FormState(Form form, bool correct = false)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            Rectangle bounds = form.WindowState == FormWindowState.Normal ? 
                form.Bounds : form.RestoreBounds;

            if (correct)
            {
                // учёт особенностей ОС
                screenWidth = Screen.FromControl(form).Bounds.Width;
                formAdj = CalcFormAdj(form);

                IsEmpty = false;
                Left = bounds.Left + formAdj;
                Top = bounds.Top;
                Width = bounds.Width - formAdj * 2;
                Height = bounds.Height - formAdj;
            }
            else
            {
                screenWidth = 0;
                formAdj = 0;

                IsEmpty = false;
                Left = bounds.Left;
                Top = bounds.Top;
                Width = bounds.Width;
                Height = bounds.Height;
            }
        }


        /// <summary>
        /// Получить признак, что состояние формы не заполнено.
        /// </summary>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// Получить или установить позицию по горизонтали.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Получить или установить позицию по вертикали.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Получить или установить ширину.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Получить или установить высоту.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Получить или установить директорию, из которой открывались схемы.
        /// </summary>
        public string SchemeDir { get; set; }

        /// <summary>
        /// Получить или установить директорию, из которой открывались изображения.
        /// </summary>
        public string ImageDir { get; set; }


        /// <summary>
        /// Установить значения по умолчанию.
        /// </summary>
        private void SetToDefault()
        {
            screenWidth = 0;
            formAdj = 0;

            IsEmpty = true;
            Left = 0;
            Top = 0;
            Width = 0;
            Height = 0;
            SchemeDir = "";
            ImageDir = "";
        }

        /// <summary>
        /// Рассчитать поправку позиции и размера формы.
        /// </summary>
        private static int CalcFormAdj(Form form)
        {
            Version version = Environment.OSVersion.Version;
            bool win8Plus = version.Major == 6 && version.Minor >= 2 || version.Major > 6;
            return win8Plus ? form.PointToScreen(new Point(0, 0)).X - form.Left - 1 /*рамка*/ : 0;
        }


        /// <summary>
        /// Загрузить состояние формы из файла.
        /// </summary>
        public bool Load(string fileName, out string errMsg)
        {
            try
            {
                SetToDefault();

                if (File.Exists(fileName))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileName);
                    XmlElement rootElem = xmlDoc.DocumentElement;
                    Left = rootElem.GetChildAsInt("Left");
                    Top = rootElem.GetChildAsInt("Top");
                    Width = rootElem.GetChildAsInt("Width");
                    Height = rootElem.GetChildAsInt("Height");
                    SchemeDir = rootElem.GetChildAsString("SchemeDir");
                    ImageDir = rootElem.GetChildAsString("ImageDir");
                    IsEmpty = false;
                }

                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.LoadFormStateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Сохранить состояние формы в файле.
        /// </summary>
        public bool Save(string fileName, out string errMsg)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(xmlDecl);

                XmlElement rootElem = xmlDoc.CreateElement("ScadaSchemeEditorState");
                xmlDoc.AppendChild(rootElem);

                rootElem.AppendElem("Left", Left);
                rootElem.AppendElem("Top", Top);
                rootElem.AppendElem("Width", Width);
                rootElem.AppendElem("Height", Height);
                rootElem.AppendElem("SchemeDir", SchemeDir);
                rootElem.AppendElem("ImageDir", ImageDir);

                xmlDoc.Save(fileName);
                errMsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errMsg = AppPhrases.SaveFormStateError + ":" + Environment.NewLine + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Применить состояние к заданной форме.
        /// </summary>
        public void Apply(Form form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            if (IsEmpty || !ScadaUiUtils.AreaIsVisible(Left, Top, Width, Height))
            {
                int adj = CalcFormAdj(form);
                form.Left = -adj;
                form.Top = BrowserToolbarHeight;
                form.Height = Math.Max(form.Height, 
                    Screen.FromControl(form).WorkingArea.Height + adj - BrowserToolbarHeight);
            }
            else
            {
                form.SetBounds(Left, Top, Width, Height);
            }
        }

        /// <summary>
        /// Притянуть форму к левому или правому краю экрана.
        /// </summary>
        public bool PullToEdge(Form form)
        {
            if (form == null)
                throw new ArgumentNullException("form");

            int newLeft = Left;

            if (0 < Left && Left <= PullDistance)
            {
                newLeft = 0;
            }
            else if (screenWidth > 0)
            {
                int maxLeft = screenWidth - Width;
                if (maxLeft - PullDistance < Left && Left < maxLeft)
                    newLeft = maxLeft;
            }

            if (Left == newLeft)
            {
                return false;
            }
            else
            {
                Left = newLeft;
                form.Left = Left - formAdj;
                return true;
            }
        }

        /// <summary>
        /// Получить состояние формы для передачи.
        /// </summary>
        public FormStateDTO GetFormStateDTO(bool formDisplayed)
        {
            return new FormStateDTO()
            {
                StickToLeft = formDisplayed && Left == 0,
                StickToRight = formDisplayed && Left + Width == screenWidth,
                Width = Width
            };
        }
    }
}
