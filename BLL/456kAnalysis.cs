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
    public class _456kAnalysis : BaseAnalysis
    {
        Regex sizeRegex = new Regex("影片容量：.*?<br>|【影片大小】：.*?<br>");
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}|[A-Z]{1,}[0-9]{1,}|[A-Z]{1,}‐[0-9]{1,}");
        Regex picRegex = new Regex("http:.*?.jpg");
        Regex torrentRegex = new Regex("forum.php\\?mod=.*?\" onmouseover");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            His his = new His();
            ArrayList resList = new ArrayList();
            try
            {
                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                his.Vid = mc[0].Value.Replace("-", "");
                his.Name = Path.GetFileNameWithoutExtension(path.ToUpper());
                try
                {
                    String sizeStr = sizeRegex.Match(content).Value.Replace("影片容量：", "").Replace("【影片大小】：", "").Replace("<br>", "");
                    //影片容量：9.34GB<br>
                    if (sizeStr == "")
                    {
                        sizeStr = path.Split(new string[] { "[HD#", "]" }, StringSplitOptions.None)[1];
                    }
                    if (sizeStr.ToUpper().Contains("GB"))
                    {
                        his.Size = Convert.ToDouble(sizeStr.Replace("GB", "")) * 1024;
                    }
                    else
                    {
                        his.Size = Convert.ToDouble(sizeStr.Replace("MB", ""));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("size error：" + path);
                    Console.WriteLine(e.ToString());
                }
                int index = torrentRegex.Matches(content).Count - 1;
                string torrentLink = "http://www.1080fhd.com/" + torrentRegex.Matches(content)[index].Value.Replace("\" onmouseover", "").Replace("amp;","");
                MatchCollection picMC = picRegex.Matches(content);

                foreach (Match match in picMC)
                {
                    if (!his.Html.Contains(match.Value))
                    {
                        his.Html += "<a href=\"" + torrentLink + "\"/><img src=\"" + match.Value + "\" /></a><br>";
                    }
                }

                his.HisTimeSpan = 999;
                his.IsCHeckHisSize = ifCheckHis;
                resList.Add(his);
            }catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "456kAnalysisUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;

        }
    }
}
