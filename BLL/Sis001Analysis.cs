using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class Sis001Analysis :BaseAnalysis
    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex sizeRegex=new Regex("size\\^\\^\\^.*");
        Regex imgRegex=new Regex("<img src=\".*?\"");
        Regex torrentLinkRegex=new Regex("attachment.php.*?\"");

        public override ArrayList alys(string content, string path, string vid)
        {
            ArrayList resList = new ArrayList();
            try
            {
                
                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                if (mc.Count != 1)
                {
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "sisUnknown");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                    return resList;
                }
                His his = new His();
                his.Vid = mc[0].Value.Replace("-", "");
                his.Size = Convert.ToDouble(sizeRegex.Match(path).Value.Replace("size^^^", "").Replace(".htm", ""));
                his.Html = content.Split(new string[] { "count_add_one", "下载次数:" }, StringSplitOptions.RemoveEmptyEntries)[1];
                string torrentLink = "http://sis001.com/bbs/" + torrentLinkRegex.Match(his.Html).Value;

                MatchCollection imgMc = imgRegex.Matches(his.Html);
                his.Html = "";
                foreach (Match match in imgMc)
                {
                    if (!match.Value.Contains("torrent.gif"))
                    {
                        his.Html += "<a href=\"" + torrentLink + "\">" + match.Value + "/></a><br>";
                    }
                }
                his.HisTimeSpan = 3;
                his.Html += this.getSearchHtml(his.Vid, his.Size);
                resList.Add(his);
            }
            catch (Exception e)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "sisUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
