using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class JavTorrentsAnalysis : BaseAnalysis
    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex imgRegex=new Regex("<img src=.* class=\"s-full\">");
        Regex sizeRegex = new Regex("class=\"dl-link.*?</a>");
        Regex sizeRegex1 = new Regex("DL -.*?GB|DL -.*?MB");
        Regex sizeRegex2 = new Regex("HD -.*?GB|HD -.*?MB");

        public override System.Collections.ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();
            try
            {
                if (path.Contains("httpjavtorrent"))
                    return resList;
                His his = new His();
                //<img src="//jtl.re/x/18/docp021.jpg" class="s-full">
                string imgUrl = "http:" + imgRegex.Match(content).Value.Replace("<img src=\"", "").Replace("\" class=\"s-full\">", "");

                his.Vid = Path.GetFileNameWithoutExtension(path.ToUpper()).Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries)[0];
                MatchCollection sizeMc = sizeRegex.Matches(content);
                double size = 0;
                bool isGB = true ;
                string size2 = sizeRegex2.Match(content).Value;
                if (!String.IsNullOrEmpty(size2))
                {
                    if (size2.EndsWith("MB"))
                        isGB = false;
                    size2 = size2.Replace("HD", "").Replace("GB", "").Replace("-", "").Replace("MB","").Trim();
                    size = Convert.ToDouble(size2);
                    if (isGB)
                        size = size * 1024;
                }
                if (size == 0)
                {
                    string size1 = sizeRegex1.Match(content).Value;
                    if (!String.IsNullOrEmpty(size1))
                    {
                        if (size1.EndsWith("MB"))
                            isGB = false;
                        size1 = size1.Replace("DL", "").Replace("GB", "").Replace("-", "").Replace("MB", "").Trim();
                        size = Convert.ToDouble(size1);
                        if (isGB)
                            size = size * 1024;
                    }

                }

                his.Size = size;
                his.HisTimeSpan = 12;
                his.Html = "<img src=\"" + imgUrl + "\"/><br>";
                his.Name = path.Split(new char[] { ']', '.' })[1];
                resList.Add(his);
            } catch(Exception e)
            {
                Tool.MoveFile("error", path);
                Console.WriteLine("error  " + path);
            }
            return resList;
        }
    }
}
