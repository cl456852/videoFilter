using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace BLL
{
    public class Bo99Analysis : BaseAnalysis
    {
        Regex reg1 = new Regex("[A-Z]");
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex idRegex1 = new Regex("[A-Z]{1,}[0-9]{1,}");
        Regex sizeRegex = new Regex("容量.*<|影片大小.*?<");

        Regex picRegex = new Regex(" file=\".*?\"");
       
        Regex torrentRegex = new Regex("forum.php\\?mod=attachment&amp;aid=.*?\"");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            His his = new His();
            ArrayList resList = new ArrayList();
            try
            {
                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                if (mc.Count == 0)
                {
                    mc = idRegex1.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                    string id = mc[0].Value;
                    string letter = "";
                    string number = "";
                    bool isEndofLetter = false;
                    for (int i = 0; i < id.Length; i++)                        //修改   对于出现KIDM235A  KIDM235B
                        if (reg1.IsMatch(id[i].ToString()))
                        {
                            if (isEndofLetter)
                                break;
                            else
                                letter += id[i];
                        }
                        else
                        {
                            number += id[i];
                            isEndofLetter = true;
                        }

                    if (number.StartsWith("00"))
                    {
                        number = number.Substring(2);
                    }
                    his.Vid = letter + number;
                }
                else
                {
                    his.Vid = mc[0].Value.Replace("-", "");
                }
                if (mc.Count != 1)
                {
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "huaSeUnknown");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                    return resList;
                }

                his.Name = Path.GetFileNameWithoutExtension(path.ToUpper()).Replace(mc[0].Value, "");

                string sizeStr = sizeRegex.Match(content).Value.Replace("容量", "").Replace("</font>", "").Replace("<", "").Replace(":", "").Replace("：", "").Replace("影片大小】", "").Replace("：", "");
                if (sizeStr != "")
                {


                    if (sizeStr.ToUpper().Contains("G"))
                    {
                        sizeStr = sizeStr.ToUpper().Replace("GB", "").Split('（')[0];
                        
                        his.Size = Convert.ToDouble(sizeStr) * 1024;
                    }
                    else
                    {
                        sizeStr = sizeStr.ToUpper().Replace("MB", "").Split('（')[0];
                        his.Size = Convert.ToDouble(sizeStr);
                    }
                }
                MatchCollection matchCollection = torrentRegex.Matches(content);
                string torrentLink = "";

                for (int i = matchCollection.Count - 1; i >= 0; i--)
                {

                    torrentLink = HttpUtility.HtmlDecode( "https://bo99.tv/" + matchCollection[i].Value.Replace("<a href=\"", "").Replace("\"", ""));
                }



                MatchCollection picMc = picRegex.Matches(content);
                foreach (Match m in picMc)
                {
                    his.Html += "<a href=\"" + torrentLink + "\"><img src=\"" + m.Value.Replace("file=\"", "") + " /></a><br>";
                }

                his.HisTimeSpan = 999;
                his.IsCHeckHisSize = ifCheckHis;
                resList.Add(his);

            }
            catch
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "168xUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
