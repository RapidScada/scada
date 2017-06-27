using System;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace ScadaDoc
{
    /// <summary>
    /// Генерирует карту сайта для поисковых систем
    /// </summary>
    public class Sitemap : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/xml";
                string siteRoot = VirtualPathUtility.AppendTrailingSlash(
                    context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath);

                using (XmlTextWriter writer = new XmlTextWriter(context.Response.OutputStream, Encoding.UTF8))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("urlset");
                    writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");

                    // добавление корня сайта
                    writer.WriteStartElement("url");
                    writer.WriteElementString("loc", siteRoot);
                    writer.WriteEndElement();

                    // добавление всех html-файлов из директории контента
                    string contentDir = context.Server.MapPath("~/content/");
                    DirectoryInfo dirInfo = new DirectoryInfo(contentDir);
                    FileInfo[] fileInfoArr = dirInfo.GetFiles("*.html", SearchOption.AllDirectories);

                    string rootDir = context.Server.MapPath("~/");
                    int rootDirLen = rootDir.Length;

                    foreach (FileInfo fileInfo in fileInfoArr)
                    {
                        string loc = siteRoot + fileInfo.FullName.Substring(rootDirLen).Replace('\\', '/');
                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", loc);
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(ex.Message);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}