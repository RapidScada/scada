/*
 * Copyright 2014 Mikhail Shiryaev
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
 * Module   : Report Builder
 * Summary  : The base class for building reports in WordprocessingML (Microsoft Word 2003) format
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2005
 * Modified : 2013
 */

using System;
using System.IO;
using System.Xml;

namespace Utils.Report
{
    /// <summary>
    /// The base class for building reports in WordprocessingML (Microsoft Word 2003) format
    /// <para>Базовый класс для построения отчётов в формате WordprocessingML (Microsoft Word 2003)</para>
    /// </summary>
    public abstract class WordRepBuilder : RepBuilder
    {
        /// <summary>
        /// Префикс XML-элемента для перехода на новую строку в WordML
        /// </summary>
        protected const string BrPref = "w";
        /// <summary>
        /// Имя XML-элемента для перехода на новую строку в WordML
        /// </summary>
        protected const string BrName = "br";
        /// <summary>
        /// Имя с префиксом XML-элемента в шаблоне, значение которого может содержать директивы изменения
        /// </summary>
        protected const string ElemName = "w:t";
        /// <summary>
        /// Высота XML-дерева от элемента строки таблицы до элемента с текстом ячейки
        /// </summary>
        protected const int RowTreeHeight = 4;


        /// <summary>
        /// Обрабатываемый XML-документ
        /// </summary>
        protected XmlDocument xmlDoc;


        /// <summary>
        /// Конструктор
        /// </summary>
        public WordRepBuilder()
            : base()
        {
            xmlDoc = null;
        }


        /// <summary>
        /// Получить формат отчёта
        /// </summary>
        public override string RepFormat
        {
            get
            {
                return "WordprocessingML";
            }
        }


        /// <summary>
        /// Найти атрибут (директиву) в строке и получить его значение
        /// </summary>
        /// <param name="str">Строка для поиска</param>
        /// <param name="attrName">Имя искомого атрибута</param>
        /// <param name="attrVal">Значение атрибута</param>
        /// <returns>Найден ли атрибут</returns>
        protected bool FindAttr(string str, string attrName, out string attrVal)
        {
            // "attrName=attrVal", вместо '=' может быть любой символ
            attrVal = "";
            if (str.StartsWith(attrName))
            {
                int start = attrName.Length + 1;
                if (start < str.Length)
                {
                    int end = str.IndexOf(" ", start);
                    if (end < 0)
                        end = str.IndexOf("x", start, StringComparison.OrdinalIgnoreCase);
                    if (end < 0)
                        end = str.Length;
                    attrVal = str.Substring(start, end - start);
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Получить XML-узлы строки таблицы и самой таблицы XML-дерева отчёта по узлу, содержащему директиву
        /// </summary>
        /// <param name="xmlNode">XML-узел, содержащий директиву</param>
        /// <param name="rowNode">XML-узел строки таблицы отчёта</param>
        /// <param name="tblNode">XML-узел таблицы отчёта</param>
        /// <returns>Найдены ли XML-узлы</returns>
        protected bool GetTreeNodes(XmlNode xmlNode, out XmlNode rowNode, out XmlNode tblNode)
        {
            try
            {
                rowNode = xmlNode;
                for (int i = 0; i < RowTreeHeight; i++)
                    rowNode = rowNode.ParentNode;
                tblNode = rowNode.ParentNode;
                return true;
            }
            catch
            {
                rowNode = null;
                tblNode = null;
                return false;
            }
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк, разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        /// <param name="textBreak">Обозначение переноса строки в устанавливаемом тексте</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, string text, string textBreak)
        {
            XmlNode parNode = xmlNode.ParentNode;
            if (parNode == null)
                throw new Exception("Parent XML element is missing.");

            // отсоединить разбиваемый узел
            parNode.RemoveChild(xmlNode);
            XmlNode cloneNode = xmlNode.Clone();

            if (text == null) text = "";
            string uri = parNode.NamespaceURI;
            int breakLen = textBreak.Length;

            do
            {
                // определение строки текста
                int breakPos = text.IndexOf(textBreak);
                bool haveBreak = breakPos >= 0;
                string line = haveBreak ? text.Substring(0, breakPos) : text;

                // добавление строки текста
                XmlNode newNode = cloneNode.Clone();
                newNode.InnerText = line;
                parNode.AppendChild(newNode);

                // добавление тега переноса строки при необходимости
                if (haveBreak)
                    parNode.AppendChild(xmlDoc.CreateElement(BrPref, BrName, uri));

                // обрезание обработанной части текста
                text = haveBreak && breakPos + breakLen < text.Length ?
                    text.Substring(breakPos + breakLen) : "";
            } while (text != "");
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк, разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        /// <param name="textBreak">Обозначение переноса строки в устанавливаемом тексте</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, object text, string textBreak)
        {
            string textStr = text == null || text.ToString() == "" ? " " : text.ToString();
            SetNodeTextWithBreak(xmlNode, textStr, textBreak);
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк "\n", разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, string text)
        {
            SetNodeTextWithBreak(xmlNode, text, "\n");
        }

        /// <summary>
        /// Установить XML-узлу текст, содержащий переносы строк "\n", разбив при необходимости элемент на несколько
        /// </summary>
        /// <param name="xmlNode">XML-узел</param>
        /// <param name="text">Устанавливаемый текст</param>
        protected void SetNodeTextWithBreak(XmlNode xmlNode, object text)
        {
            SetNodeTextWithBreak(xmlNode, text, "\n");
        }


        /// <summary>
        /// Начальная обработка дерева XML-документа
        /// </summary>
        protected virtual void StartXmlDocProc()
        {
        }

        /// <summary>
        /// Рекурсивный обход и обработка дерева XML-документа согласно директивам для получения отчёта
        /// </summary>
        /// <param name="xmlNode">Обрабатываемый XML-узел</param>
        protected virtual void XmlDocProc(XmlNode xmlNode)
        {
            if (xmlNode.Name == ElemName)
            {
                // поиск директив преобразований элементов
                string nodeVal = xmlNode.InnerText;
                string attrVal;
                if (FindAttr(nodeVal, "repRow", out attrVal))
                {
                    if (nodeVal.Length < 8 /*"repRow=".Length + 1*/ + attrVal.Length)
                        xmlNode.InnerText = "";
                    else
                        xmlNode.InnerText = nodeVal.Substring(8 + attrVal.Length);
                    ProcRow(xmlNode, attrVal);
                }
                else if (FindAttr(nodeVal, "repVal", out attrVal))
                    ProcVal(xmlNode, attrVal);
            }
            else
            {
                // рекурсивный перебор потомков текущего элемента
                XmlNodeList children = xmlNode.ChildNodes;
                foreach (XmlNode node in children)
                    XmlDocProc(node);
            }
        }

        /// <summary>
        /// Окончательная обработка дерева XML-документа
        /// </summary>
        protected virtual void FinalXmlDocProc()
        {
        }

        /// <summary>
        /// Обработка директивы, изменяющей значение элемента
        /// </summary>
        /// <param name="xmlNode">XML-узел, содержащий директиву</param>
        /// <param name="valName">Имя элемента, заданное директивой</param>
        protected virtual void ProcVal(XmlNode xmlNode, string valName)
        {
        }

        /// <summary>
        /// Обработка директивы, создающей строки таблицы
        /// </summary>
        /// <param name="xmlNode">XML-узел, содержащий директиву</param>
        /// <param name="rowName">Имя строки, заданное директивой</param>
        protected virtual void ProcRow(XmlNode xmlNode, string rowName)
        {
        }


        /// <summary>
        /// Генерировать отчёт в поток в формате WordML
        /// </summary>
        /// <param name="outStream">Выходной поток</param>
        /// <param name="templateDir">Директория шаблона со '\' на конце</param>
        public override void Make(Stream outStream, string templateDir)
        {
            // имя файла шаблона отчёта
            string templFileName = templateDir + TemplateFileName;

            // загрузка и разбор файла шаблона в формате XML
            xmlDoc = new XmlDocument();
            xmlDoc.Load(templFileName);

            // создание отчёта - модификация xmlDoc
            StartXmlDocProc();
            XmlDocProc(xmlDoc.DocumentElement);
            FinalXmlDocProc();

            // запись в выходной поток
            xmlDoc.Save(outStream);
        }
    }
}
