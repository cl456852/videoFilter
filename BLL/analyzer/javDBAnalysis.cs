
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class JavDBAnalysis : BaseAnalysis
    {
        Regex imgRegex = new Regex("<img src=\".*?\" class=\"video-cover\">");
        //<div id="magnets-content" class="magnet-links">.*?</article>
        //<div id="magnets-content" class="magnet-links">.*?</article>
        Regex magRegex = new Regex("<div id=\"magnets-content\" class=\"magnet-links\">.*?</article>",RegexOptions.Singleline);
        Regex sizeRegex = new Regex("<span class=\"meta\">.*?</span>", RegexOptions.Singleline);

        public override ArrayList alys(string content, string path, string path1, bool ifCheckHis) 
        {
            ArrayList list = new ArrayList();
            try
            {


                His his = new His();
                ArrayList resList = new ArrayList();
                his.Vid = Path.GetFileName(path).Split(new string[] { "$$$" }, StringSplitOptions.None)[0];
                his.Name = Path.GetFileNameWithoutExtension(path).ToString().Replace("$$$", "");
                string imgUrl = imgRegex.Match(content).Value.Replace("<img src=\"", "").Replace("\" class=\"video-cover\">", "") + "\n";
                string magHtml = magRegex.Match(content).Value + "\n";
                his.Html += "<img src=\"" + imgUrl + "\"/><br>\n";
                his.Html += magHtml;
                double size;
                MatchCollection mc = sizeRegex.Matches(magHtml);
                foreach (Match m in mc)
                {
                    string sizeDigit = m.Value.Replace("<span class=\"meta\">","").Replace("</span>", "").Trim();
                    try
                    {

                        sizeDigit = sizeDigit.Split(',')[0];
                        if (sizeDigit.EndsWith("GB"))
                        {
                            sizeDigit = sizeDigit.Replace("GB", "");
                            size = Convert.ToDouble(sizeDigit) * 1024;

                        }
                        else if (sizeDigit.EndsWith("MB") || sizeDigit.EndsWith("KB"))
                            size = Convert.ToDouble(sizeDigit.Replace("MB", "").Replace("KB", ""));
                        else
                            size = 0;
                        his.Size = his.Size > size ? his.Size : size;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("分析文件异常" + e.Message + path+"    "+ sizeDigit);
                    }
                }
                list.Add(his);
            }
            catch (Exception e)
            {

                Console.WriteLine("分析文件异常" + e.Message + path);
            }

            return list;
        }

    }
}