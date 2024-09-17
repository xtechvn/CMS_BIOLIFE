using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities
{
    public static class ArticleHelper
    {
        public static string HighLightLinkTag(string html)
        {
            try
            {

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);
                var atag = document.DocumentNode.SelectNodes("//a");
                if(atag!=null && atag.Count > 0)
                {
                    foreach (var item in atag)
                    {
                        try
                        {
                            if (item.Attributes["style"] != null)
                            {
                                var style = item.Attributes["style"].Value;
                                string pattern = @"color(.*?)(;)";
                                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                                string newStyle = regex.Replace(style, String.Empty);
                                item.Attributes["style"].Value = newStyle + "color: #37A07F;";
                            }
                            else
                            {
                                item.SetAttributeValue("style", "color: #37A07F;text-decoration: underline;");
                            }
                        }
                        catch (NullReferenceException)
                        {
                            item.SetAttributeValue("style", "color: #37A07F;text-decoration: underline;");
                        }
                    }
                }
                return document.DocumentNode.OuterHtml;
            } catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("ArticleHelper - HighLightLinkTag: " + ex.ToString()+ "\nHTML="+html.Substring(0,220));
                return html;
            }
        }
        
    }
}
