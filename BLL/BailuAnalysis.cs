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
        Regex sizeRegex = new Regex("(?-i)容量.*?B");
        Regex imgRegex = new Regex("http://.*jpg");
        Regex torrentRegex = new Regex("forum.php\\?mod=attachment&amp;aid=.*?\"");
        Regex picRegex = new Regex("src=\"http.*?\"");
        Regex sizeRegex1 = new Regex("\\[FHD.*\\]");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(path.ToUpper());
                string sizeStr;
                //if (fileName.StartsWith("[FHD"))
                //{
                //    sizeStr = sizeRegex1.Match(fileName).Value.Replace("[FHD","").Replace("]","");
                //    fileName = fileName.Substring(4);
                    

                //}
                //else
                {
                    //容量：</div></td><td>812.71 MB</td></tr><tr><td><div
                    sizeStr = sizeRegex.Match(content).Value.Replace("容量：</div></td><td>", "").Replace("容量：","").Replace(" ","");
                }
                MatchCollection mc = idRegex.Matches(fileName);
            if (mc.Count != 1)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "BailuUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                return resList;
            }
            His his = new His();
            his.Vid = mc[0].Value.Replace("-", "");
            his.Name = Path.GetFileNameWithoutExtension(path.ToUpper());
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
                string torrentLink;
            MatchCollection matchCollection= torrentRegex.Matches(content);
                if (matchCollection.Count > 1)
                {
                    torrentLink = "http://www.100kke.info/" + matchCollection[1].Value.Replace("amp;","").Replace("\"", "");
                    his.Html= "<a href=\"" + torrentLink + "\"><img src=\"" + "http://www.100kke.info/" + matchCollection[0].Value.Replace("amp;","") + "/></a><br>"; 
                }
                else
                {
                    torrentLink = "http://www.100kke.info/" + matchCollection[0].Value.Replace("amp;", "").Replace("\"",""); 
                }

            MatchCollection picMc = picRegex.Matches(content);
                
            foreach (Match m in picMc)
            {
                his.Html += "<a href=\"" + torrentLink + "\"><img " + m.Value + "/></a><br>";
            }

            his.HisTimeSpan = 1000;
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
