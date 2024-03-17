using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class ThzAnalysis:BaseAnalysis
    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex sizeRegex = new Regex("容量.*<");
        Regex imgRegex = new Regex("http://.*jpg");

        Regex torrentLinkRegex = new Regex("mod=attachment&aid=.*?=");

        public override ArrayList alys(string content, string path, string vid, bool isCheckHis)
        {
            ArrayList resList = new ArrayList();
            try
            {

                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                if (path.EndsWith(".htm.htm"))
                {
                    return resList;
                }
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
                string name= path.Split(new char[] { ']', '.' })[1];
                his.Name = name;
                try
                {
                    string sizeStr = sizeRegex.Match(content).Value.Replace("容量：", "").Replace("<", "");
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
                }
                catch(Exception e)
                {
                    Console.WriteLine("Size Error " + e.Message);
                }
                string torrentLink="";
                try
                {


                    StreamReader sr = new StreamReader(path + ".htm");
                    string content1 = sr.ReadToEnd();
                    sr.Close();
                    torrentLink = "http://taohuabt.info/forum.php?" + torrentLinkRegex.Match(content1).Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(".htm.htm異常" + path);
                }
                content= content.Split(new string[] { "alt=\"发新帖\"" },StringSplitOptions.RemoveEmptyEntries)[1];
                MatchCollection imgMc = imgRegex.Matches(content);
                int n = 0;
                foreach (Match match in imgMc)
                {
                    if (!match.Value.Contains("middle"))
                    {
                        if(n==0)
                            his.Html += "<a href=\"" + torrentLink + "\"><img src=\"" + match.Value + "\"/></a><br>";
                        else
                            his.Html += "<a href=\"" + torrentLink + "\"><img src=\"" + match.Value + "\" width=\"40%\"/></a><br>";
                        n++;
                    }
                }
                his.Html= his.Html.Replace("onclick=\"", "");
                his.HisTimeSpan = 10;
                resList.Add(his);
            }
            catch (Exception e)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "thzUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
