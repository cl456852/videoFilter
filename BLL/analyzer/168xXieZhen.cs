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
    public class _168xXieZhen : BaseAnalysis
    {
        Regex idRegex = new Regex("<span id=\"thread_subject\">.*?</span>");
        Regex torrentRegex = new Regex("forum.php\\?mod=attachment&amp;aid=.*?\"");
        Regex sizeRegex = new Regex("/.*?\\]");
        Regex picRegex = new Regex(" file=\".*?\"");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            Console.WriteLine(path);
            ArrayList resList = new ArrayList();
            try
            {
                if (content.Contains(">[国产写真]</a>"))
                {
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "国产写真");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");

                }
                else
                    return resList;
                His his = new His();
                string id = idRegex.Match(content).Value.Replace("<span id=\"thread_subject\">", "").Replace("</span>", "");
                try
                {


                    MatchCollection sizeMC = sizeRegex.Matches(id);
                    string sizeStr = sizeMC[sizeMC.Count - 1].Value.Replace("/", "").Replace("]", "");
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
                catch (Exception e)
                {
                    Console.WriteLine("size error " + path);
                    Console.WriteLine(e.ToString());
                }

                his.Vid = id;
                his.Name = id;
                 
                MatchCollection matchCollection = torrentRegex.Matches(content);
                string torrentLink="";

                try
                {


                    torrentLink = "https://www.sehuatang.net/" + matchCollection[matchCollection.Count - 1];
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                MatchCollection picMc = picRegex.Matches(content);
                foreach (Match m in picMc)
                {
                    his.Html += "<a href=\"" + torrentLink + "><img src=\"" + m.Value.Replace("file=\"", "") + " /></a><br>";
                }
                his.HisTimeSpan = 999;
                resList.Add(his);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "168xUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
