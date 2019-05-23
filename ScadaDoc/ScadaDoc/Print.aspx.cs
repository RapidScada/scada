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
 * Module   : Documentation
 * Summary  : Web form for a documentation print version
 * 
 * Author   : Mikhail Shiryaev
 * Created  : 2019
 * Modified : 2019
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace ScadaDoc
{
    /// <summary>
    /// Web form for a documentation print version.
    /// <para>Веб-форма версии доментации для печати.</para>
    /// </summary>
    public partial class WFrmPrint : System.Web.UI.Page
    {
        /// <summary>
        /// Represents an article in the contents.
        /// <para>Представляет статью в содержании.</para>
        /// </summary>
        private class Article
        {
            public string Link { get; set; }
            public string Title { get; set; }
            public int Level { get; set; }
        }


        /// <summary>
        /// Parses the JavaScript contents file.
        /// </summary>
        private List<Article> ParseArticles(string fileName)
        {
            List<Article> articles = new List<Article>();
            return articles;
        }

        /// <summary>
        /// Writes the article to the output stream.
        /// </summary>
        private void WriteArticle(Article article)
        {
            Response.Write(string.Format("<div>{0}</div>", article.Title));
        }

        /// <summary>
        /// Generates joined documentation and outputs it to the stream.
        /// </summary>
        protected void GenerateDoc()
        {
            string lang = Request.QueryString["lang"];

            if (string.IsNullOrEmpty(lang))
            {
                Response.Write("Language not defined.");
            }
            else
            {
                string topDir = Path.Combine(Server.MapPath("~"), "content", lang);

                if (Directory.Exists(topDir))
                {
                    List<Article> articles = ParseArticles(Path.Combine(topDir, "js", "contents.js"));

                    if (articles.Count > 0)
                    {
                        foreach (Article article in articles)
                        {
                            WriteArticle(article);
                        }
                    }
                    else
                    {
                        Response.Write("No articles found.");
                    }
                }
                else
                {
                    Response.Write("No content found.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
