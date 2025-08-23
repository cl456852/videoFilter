using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace BLL
{

    public class _169bt  : BaseAnalysis
    {   
        Regex sizeRegex = new Regex(@"\[影片大小\]：(.*?)GB");
        Regex vidRegex = new Regex(@"[A-Z]{2,6}-?\d{3,4}");
        Regex imgRegex = new Regex("class=\"zoom\" file=\"(.*?)\"");
        Regex torrentRegex = new Regex("<a href=\"forum\\.php\\?mod=attachment&amp;aid=(.*?)\"");
        
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();
            try
            {


                His his = new His();

                string fileName = Path.GetFileNameWithoutExtension(path);
                his.Vid = vidRegex.Match(fileName).Value;
                his.Name = fileName.Replace("[4K] ", "");
                his.Size = Convert.ToDouble(sizeRegex.Match(content).Groups[1].Value) * 1024;
                MatchCollection mc = imgRegex.Matches(content);
                string torrent = torrentRegex.Match(content).Groups[1].Value;
                for (int i = 0; i < mc.Count; i++)
                {

                    his.Html += "<a href=\"https://169bt.com/forum.php?mod=attachment&aid=" + torrent +
                                "\"><img src=\"" + mc[i].Groups[1].Value + "\"></a><br>";
                }

                resList.Add(his);
            }
            catch (Exception e)
            {
                Console.WriteLine(path);
                Console.WriteLine(e);
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "169btException");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }

            return resList;
        }
    }
}