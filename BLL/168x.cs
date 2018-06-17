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
    public class _168x : BaseAnalysis
    {
        Regex reg1 = new Regex("[A-Z]");
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex idRegex1 = new Regex("[A-Z]{1,}[0-9]{1,}");
        Regex sizeRegex = new Regex("容量.*<");
        Regex picRegex = new Regex("src=\"http.*?\"");
        Regex torrentRegex = new Regex("forum.php\\?mod=attachment&amp;aid=.*?\"");

        public override ArrayList alys(string content, string path, string vid, bool isCheckHis)
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
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "thzUnknown");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                    return resList;
                }

                his.Name = Path.GetFileNameWithoutExtension(path.ToUpper()).Replace(mc[0].Value, "");

                string sizeStr = sizeRegex.Match(content).Value.Replace("容量", "").Replace("<", "").Replace(":","").Replace("：","");
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
                his.IsCHeckHisSize = isCheckHis;
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
