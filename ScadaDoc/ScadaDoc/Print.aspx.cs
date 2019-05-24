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
using System.Text;
using System.Web;

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
        /// The home link to skip.
        /// </summary>
        private const string HomeLink = "../../../";
        /// <summary>
        /// The symbol of the latest version.
        /// </summary>
        private const string LatestVersion = "latest";

        private string ver;          // the version specified in the query string
        private string lang;         // the language specified in the query string
        private string articleDir;   // the top directory of the articles
        protected string articleUrl; // the URL of the top directory


        /// <summary>
        /// Parses the JavaScript contents file.
        /// </summary>
        private List<Article> ParseArticles(string fileName)
        {
            List<Article> articles = new List<Article>();

            if (File.Exists(fileName))
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();

                            const string FuncBegin = "  addArticle(";
                            int begIdx = line.IndexOf(FuncBegin);

                            if (begIdx >= 0)
                            {
                                begIdx += FuncBegin.Length;
                                int endIdx = line.IndexOf(");", begIdx);

                                if (endIdx >= 0)
                                {
                                    string[] args = line.Substring(begIdx, endIdx - begIdx).Split(',');
                                    articles.Add(new Article
                                    {
                                        Link = args[1].Trim(' ', '"'),
                                        Title = args[2].Trim(' ', '"'),
                                        Level = args.Length <= 3 ? 0 : int.Parse(args[3])
                                    });
                                }
                            }
                        }
                    }
                }
            }

            return articles;
        }

        /// <summary>
        /// Writes the article to the output stream.
        /// </summary>
        private void WriteArticle(Article article)
        {
            Response.Write(string.Format("<div class='print-article' data-link='{0}'>", article.Link));

            try
            {
                if (article.Link.EndsWith("/"))
                {
                    Response.Write(string.Format("<h1>{0}</h1>", article.Title));
                }
                else if (article.Link.EndsWith(".html"))
                {
                    string absPath = Path.Combine(articleDir, article.Link.Replace('/', Path.DirectorySeparatorChar));
                    WriteArticleBody(absPath);
                }
            }
            catch (Exception ex)
            {
                Response.Write(string.Format("<div class='exception'>{0}</div>", ex.ToString()));
            }
            finally
            {
                Response.Write("</div>");
            }
        }

        /// <summary>
        /// Writes the HTML body of the article.
        /// </summary>
        private void WriteArticleBody(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    bool bodyFound = false;

                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string lineTrimmed = line.Trim();

                        if (lineTrimmed == "</body>")
                        {
                            break;
                        }
                        else if (bodyFound)
                        {
                            Response.Write(line);
                            Response.Write(Environment.NewLine);
                        }
                        else if (lineTrimmed == "<body>")
                        {
                            bodyFound = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates joined documentation and outputs it to the stream.
        /// </summary>
        protected void GenerateDoc()
        {
            Response.Write(string.Format("<h1>Rapid SCADA {0}</h1>", ver == LatestVersion ? "" : ver));

            if (string.IsNullOrEmpty(lang))
            {
                Response.Write("Language not defined.");
            }
            else
            {
                articleDir = Path.Combine(Server.MapPath("~"), "content", ver, lang);

                if (Directory.Exists(articleDir))
                {
                    List<Article> articles = ParseArticles(Path.Combine(articleDir, "js", "contents.js"));

                    if (articles.Count > 0)
                    {
                        foreach (Article article in articles)
                        {
                            if (article.Link != HomeLink && !article.Link.StartsWith("version-history"))
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
            ver = Request.QueryString["ver"] ?? LatestVersion;
            lang = Request.QueryString["lang"];
            articleUrl = VirtualPathUtility.ToAbsolute(string.Format("~/content/{0}/{1}/", ver, lang));
        }
    }
}
