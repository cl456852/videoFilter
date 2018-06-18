using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class BailuAnalysis : BaseAnalysis
    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex sizeRegex = new Regex("容量.*<");
        Regex imgRegex = new Regex("http://.*jpg");
        Regex torrentRegex = new Regex("forum.php\\?mod=attachment&amp;aid=.*?\"");
        Regex picRegex = new Regex("src=\"http.*?\"");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();
            try
            {
                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
            if (mc.Count != 1)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "thzUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                return resList;
            }
            His his = new His();
            his.Vid = mc[0].Value.Replace("-", "");
            his.Name = Path.GetFileNameWithoutExtension(path.ToUpper()).Replace(mc[0].Value, "");

            string sizeStr = sizeRegex.Match(content).Value.Replace("容量", "").Replace("<", "").Replace(":", "").Replace("：", "");
            if (sizeStr.ToUpper().Contains("G"))
            {
                sizeStr = sizeStr.ToUpper().Replace("GB", "");
                his.Size = Convert.ToDouble(sizeStr) * 1024;
            }
            else
            {
                sizeStr = sizeStr.ToUpper().Replace("MB", "");
                his.Size = Convert.ToDouble(sizeStr);
            }

            string torrentLink = "http://168x.me/" + torrentRegex.Match(content);

            MatchCollection picMc = picRegex.Matches(content);
            foreach (Match m in picMc)
            {
                his.Html += "<a href=\"" + torrentLink + "\"><img " + m.Value + "/></a><br>";
            }

            his.HisTimeSpan = 10;
            his.IsCHeckHisSize = ifCheckHis;
            resList.Add(his);
        }
            catch
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "BailuUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
