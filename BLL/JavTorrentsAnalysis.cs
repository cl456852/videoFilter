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

        public override System.Collections.ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();
            if (path.Contains("httpjavtorrent"))
                return resList;
            His his = new His();
            //<img src="//jtl.re/x/18/docp021.jpg" class="s-full">
            string imgUrl ="http:"+ imgRegex.Match(content).Value.Replace("<img src=\"", "").Replace("\" class=\"s-full\">", "");

            his.Vid = Path.GetFileNameWithoutExtension(path.ToUpper()).Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries)[0];
            MatchCollection sizeMc=  sizeRegex.Matches(content);
            double size=0; 
            foreach(Match match in sizeMc)
            {
                //class="dl-link DL" target="_blank">DL - 881.8 MB</a>
                //class="dl-link HD" target="_blank">HD - 3.49 GB</a>
                
                string sizeString= match.Value.Replace("class=\"dl-link DL\" target=\"_blank\">DL - ", "").Replace("class=\"dl-link HD\" target=\"_blank\">HD - ", "");
                if (sizeString.Contains("MB"))
                    size = Convert.ToDouble(sizeString.Replace("MB</a>","").Trim());
                else
                    size = Convert.ToDouble(sizeString.Replace("GB</a>", "").Trim())*1024;

            }
            his.Size = size;
            his.HisTimeSpan = 999;
            his.Html = "<img src=\"" + imgUrl + "\"/><br>";
            his.IsCHeckHisSize = ifCheckHis;
            his.IsCheckSize = false;
            his.Name = path.Split(new char[] { ']', '.' })[1];
            resList.Add(his);
            return resList;
        }
    }
}
