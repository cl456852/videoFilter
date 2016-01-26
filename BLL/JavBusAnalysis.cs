using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class JavBusAnalysis:BaseAnalysis
    {
        Regex imgRegex = new Regex("http://pics.javbus.com/cover.*?.jpg");
        Regex sizeRegex=new Regex(@"\s[1-9]([^\s])*?[0-9]GB|\s[1-9]([^\s])*?[0-9]MB");
        public override ArrayList alys(string content, string path, string vid)
        {
            if (path.EndsWith("_magenet") || Path.GetFileNameWithoutExtension(path).StartsWith("http^__www.javbus.com"))
                return new ArrayList();
            ArrayList list=new ArrayList();
            string id = Path.GetFileNameWithoutExtension(path);
            string img = imgRegex.Match(content).Value;
            His his = new His();
            his.Vid = id.Replace("-", "");
            StreamReader sr = new StreamReader(path + "_magenet");
            string magContent = sr.ReadToEnd();
            MatchCollection mc= sizeRegex.Matches(magContent);
            double size=0;
            foreach(Match match in mc)
            {
                double eachSize=0;
                string sizeStr = match.Value;
                if (sizeStr.Contains("GB"))
                    eachSize = Convert.ToDouble(sizeStr.Replace("GB", "")) * 1024;
                else
                    eachSize = Convert.ToDouble(sizeStr.Replace("MB", ""));
                size = eachSize > size ? eachSize : size;
            }
            his.HisTimeSpan = 3;
            his.Html += "<img src=\"" + img + "\"/><br>\n<table>";
            his.Html += magContent+"</table><br>\n";
            his.Size = size;
            his.Html += this.getSearchHtml(his.Vid, his.Size);
            list.Add(his);
            return list;

        }
    }
}
