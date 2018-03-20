using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class AkibaOnlineAnalysis : BaseAnalysis
    {
        Regex srcRegex=new Regex("src=\".*\"");
        Regex hrefRegex=new Regex("href=\".*\"");
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        public override ArrayList alys(string content, string path, string vid,bool isCheckHis)
        {
            ArrayList resList = new ArrayList();
            if (!path.EndsWith("htm"))
                return resList;
  
            MatchCollection mc = idRegex.Matches(RemoveExtension(Path.GetFileNameWithoutExtension(path)));
            if (mc.Count != 1)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "AkibaOnlineUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                return resList;
            }
            His his = new His();
            his.HtmPath = path;
            his.Vid = mc[0].Value.Replace("-", "");
            his.HisTimeSpan = 48;
            string directory = Path.GetDirectoryName(path);
            String[] torrentPaths = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly);
            
            foreach(string torrentPath in torrentPaths)
            {
                if(torrentPath.EndsWith(".torrent")&&torrentPath.Contains(Path.GetFileNameWithoutExtension(path)))
                {
                    float size=GetTorrentSize(torrentPath);
                    if(size>his.Size)
                    {
                        his.TorrentPath = torrentPath;
                        his.Size = size;
                    }
                }
            }
            his.Html="<div class=\"messageContent\">"+ content.Split(new string[] { "class=\"messageContent\">", "class=\"messageMeta ToggleTriggerAnchor\">" }, StringSplitOptions.RemoveEmptyEntries)[1]+"<br>\n";
            MatchCollection srcMC = srcRegex.Matches(his.Html);
            foreach (Match match in srcMC)
            {
                if (!match.Value.StartsWith("src=\"http"))
                {
                    his.Html= his.Html.Replace(match.Value, match.Value.Replace("src=\"", "src=\"https://www.akiba-online.com/"));
                }
            }
            MatchCollection hrefMC = hrefRegex.Matches(his.Html);
            foreach (Match match in hrefMC)
            {
                if (!match.Value.StartsWith("href=\"http"))
                {
                    his.Html = his.Html.Replace(match.Value, match.Value.Replace("href=\"", "href=\"https://www.akiba-online.com/"));
                }
            }
            his.IsCHeckHisSize=isCheckHis;
            resList.Add(his);
            return resList;
        }

        
    }
}
