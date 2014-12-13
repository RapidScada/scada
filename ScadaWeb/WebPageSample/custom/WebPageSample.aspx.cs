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
 * Module   : WebPageSample
 * Summary  : Example of custom web page embedded in SCADA-Web
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2012
 * Modified : 2014
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scada;
using Scada.Client;
using Scada.Web;

namespace WebPageSample
{
    /// <summary>
    /// Example of custom web page embedded in SCADA-Web
    /// <para>Пример пользовательской веб-страницы, встраиваемой в SCADA-Web</para>
    /// </summary>
    public partial class WFrmWebPageSample : System.Web.UI.Page
    {
        /// <summary>
        /// View
        /// <para>Представление</para>
        /// </summary>
        /// <remarks>
        /// The class is required for showing event list, drawing diagrams and sending commands
        /// <para>Класс необходим для отображения списка событий, построения графиков и отправки команд ТУ</para>
        /// </remarks>
        private class WebPageView : BaseView
        {
            /// <summary>
            /// Initializes a new instance of the class
            /// <para>Конструктор</para>
            /// </summary>
            public WebPageView()
                : base()
            {
                StoredOnServer = false;
            }

            /// <summary>
            /// Bind input channel properties to the elements of the view
            /// <para>Привязать свойства входных каналов к элементам представления</para>
            /// </summary>
            public override void BindCnlProps(CnlProps[] cnlPropsArr)
            {
                // вызов метода базового класса
                base.BindCnlProps(cnlPropsArr);
            }
            /// <summary>
            /// Fill input and output channel lists used in the view
            /// <para>Заполнить списки входных каналов и каналов управления, которые используются в представлении</para>
            /// </summary>
            public void AddCnlNums(PageInfo pageInfo)
            {
                if (pageInfo != null)
                {
                    foreach (int cnlNum in pageInfo.InCnlSet)
                        AddCnlNum(cnlNum);
                    foreach (int cnlNum in pageInfo.CtrlCnlSet)
                        AddCtrlCnlNum(cnlNum);
                }
            }
        }

        /// <summary>
        /// Channel to control binding
        /// <para>Привязка канала к элементу управления</para>
        /// </summary>
        private class CnlBinding
        {
            /// <summary>
            /// Initializes a new instance of the class
            /// <para>Конструктор</para>
            /// </summary>
            public CnlBinding(Control control, int cnlNum)
            {
                Control = control;
                CnlNum = cnlNum;
            }

            /// <summary>
            /// Gets or sets the control
            /// <para>Получить или установить элемент управления</para>
            /// </summary>
            public Control Control { get; set; }
            /// <summary>
            /// Gets or sets the channel number
            /// <para>Получить или установить номер канала</para>
            /// </summary>
            public int CnlNum { get; set; }
        }

        /// <summary>
        /// Web page information
        /// <para>Информация о веб-странице</para>
        /// </summary>
        private class PageInfo
        {
            /// <summary>
            /// Initializes a new instance of the class
            /// <para>Конструктор</para>
            /// </summary>
            private PageInfo()
            {
                InCnlSet = new HashSet<int>();
                CtrlCnlSet = new HashSet<int>();
                InCnlBindings = new List<CnlBinding>();
                CtrlCnlBindings = new List<CnlBinding>();
            }

            /// <summary>
            /// Gets the set of input channels used in the view
            /// <para>Получить множество номеров входных каналов, которые используются в представлении</para>
            /// </summary>
            public HashSet<int> InCnlSet { get; private set; }
            /// <summary>
            /// Gets the set of output channels used in the view
            /// <para>Получить множество номеров каналов управления, которые используются в представлении</para>
            /// </summary>
            public HashSet<int> CtrlCnlSet { get; private set; }
            /// <summary>
            /// Gets input channels to controls bindings
            /// <para>Получить привязку входных каналов к элементам управления</para>
            /// </summary>
            public List<CnlBinding> InCnlBindings { get; private set; }
            /// <summary>
            /// Gets output channels to controls bindings
            /// <para>Получить привязку каналов управления к элементам управления</para>
            /// </summary>
            public List<CnlBinding> CtrlCnlBindings { get; private set; }

            /// <summary>
            /// Retrieve the channel number from the string
            /// <para>Получить номер канала из строки</para>
            /// </summary>
            private int GetCnlNum(string source, string prefix)
            {
                int i1 = source.IndexOf(prefix);
                if (i1 > 0)
                {
                    i1 += prefix.Length;
                    int i2 = source.IndexOf(" ", i1);
                    string s = i1 < i2 ? source.Substring(i1, i2 - i1) : source.Substring(i1);
                    int cnlNum;
                    if (int.TryParse(s, out cnlNum))
                        return cnlNum;
                }
                return -1;
            }
            /// <summary>
            /// Recursively parse the information about the controls
            /// <para>Рекурсивно извлечь информацию об элементах управления</para>
            /// </summary>
            private void ParseControls(ControlCollection controls)
            {
                if (controls != null)
                {
                    foreach (Control control in controls)
                    {
                        if (control is WebControl)
                        {
                            // parse channel numbers from CSS class using the format "data cnl_N" or "data ctrl_N"
                            // извлечение номеров каналов из класса CSS имеющего формат "data cnl_N" или "data ctrl_N"
                            string cssClass = ((WebControl)control).CssClass.ToLower();

                            if (cssClass.StartsWith("data "))
                            {
                                int cnlNum = GetCnlNum(cssClass, " cnl_");
                                if (cnlNum > 0)
                                {
                                    InCnlSet.Add(cnlNum);
                                    InCnlBindings.Add(new CnlBinding(control, cnlNum));
                                }
                                else
                                {
                                    cnlNum = GetCnlNum(cssClass, " ctrl_");
                                    if (cnlNum > 0)
                                    {
                                        CtrlCnlSet.Add(cnlNum);
                                        CtrlCnlBindings.Add(new CnlBinding(control, cnlNum));
                                    }
                                }
                            }
                        }
                        ParseControls(control.Controls);
                    }
                }
            }
            /// <summary>
            /// Retrieve the page information
            /// <para>Извлечь информацию о веб-странице</para>
            /// </summary>
            public static PageInfo ParsePage(Page page)
            {
                PageInfo pageInfo = new PageInfo();
                if (page != null)
                    pageInfo.ParseControls(page.Controls);
                return pageInfo;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            // disable the page caching
            // отключение кэширования страницы
            ScadaUtils.DisablePageCache(Response);

            // get the current user data
            // получение данных пользователя
            UserData userData = UserData.GetUserData();

            // check login
            // проверка входа в систему
            userData.CheckLoggedOn(Context, false);
            if (!userData.LoggedOn)
                throw new Exception(WebPhrases.NotLoggedOn);

            // get the selected view indexes
            // определение индексов выбранного представления
            int viewSetIndex;
            if (!int.TryParse(Request["viewSet"], out viewSetIndex))
                viewSetIndex = -1;
            int viewIndex;
            if (!int.TryParse(Request["view"], out viewIndex))
                viewIndex = -1;

            // get the view and user rights
            // получение представления и прав пользователя на него
            BaseView view;
            MainData.Right right;
            userData.GetView(typeof(WebPageView), viewSetIndex, viewIndex, out view, out right);

            // check the view loading and check the view rights
            // проверка загрузки представления и прав на просмотр информации
            if (view == null)
                throw new Exception(WebPhrases.UnableLoadView);
            else if (!right.ViewRight)
                throw new Exception(CommonPhrases.NoRights);

            // retrieve the page information, fill the lists of channels used in the view
            // извлечение информации о веб-странице и заполнение списков каналов, используемых в представлении
            PageInfo pageInfo = PageInfo.ParsePage(this);
            ((WebPageView)view).AddCnlNums(pageInfo);

            // get the date for diagrams
            // определение даты, используемой для построения графиков
            DateTime diagDate;
            try
            {
                diagDate = new DateTime(int.Parse(Request["year"]), int.Parse(Request["month"]),
                    int.Parse(Request["day"]));
            }
            catch { diagDate = DateTime.Today; }

            // refresh the current data
            // обновление таблицы текущего среза
            AppData.MainData.RefreshData();

            // display the input channels data
            // вывод значений входных каналов
            foreach (CnlBinding cnlBinding in pageInfo.InCnlBindings)
            {
                Control ctrl = cnlBinding.Control;
                int inCnlNum = cnlBinding.CnlNum;

                if (ctrl is Label)
                {
                    Label lbl = (Label)ctrl;

                    if (inCnlNum > 0)
                    {
                        string color;
                        lbl.Text = AppData.MainData.GetCnlVal(inCnlNum, false, out color);
                        lbl.ForeColor = Color.FromName(color);
                    }
                    else
                    {
                        lbl.Text = "";
                    }
                }
                else if (cnlBinding.Control is HyperLink)
                {
                    HyperLink hl = (HyperLink)ctrl;

                    if (inCnlNum > 0)
                    {
                        string color;
                        hl.NavigateUrl = "javascript:ShowDiag(" + viewSetIndex + ", " + viewIndex + ", " +
                            diagDate.Year + ", " + diagDate.Month + ", " + diagDate.Day + ", " + inCnlNum + ", '../')";
                        hl.Text = AppData.MainData.GetCnlVal(inCnlNum, false, out color);
                        hl.ForeColor = Color.FromName(color);
                    }
                    else
                    {
                        hl.Text = "";
                    }
                }
            }

            // tune up the links binded to input channels
            // настройка ссылок, связанных с каналами управления
            foreach (CnlBinding cnlBinding in pageInfo.CtrlCnlBindings)
            {
                Control ctrl = cnlBinding.Control;
                int ctrlCnlNum = cnlBinding.CnlNum;

                if (cnlBinding.Control is HyperLink)
                {
                    HyperLink hl = (HyperLink)ctrl;

                    if (ctrlCnlNum > 0)
                    {
                        if (right.CtrlRight)
                            hl.NavigateUrl = "javascript:SendCmd(" + viewSetIndex + ", " + viewIndex + ", " +
                                ctrlCnlNum + ", '../')";
                        else
                            hl.Visible = false;
                    }
                    else
                    {
                        hl.Text = "";
                    }
                }
            }
        }
    }
}