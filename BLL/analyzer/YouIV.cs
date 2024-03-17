using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

namespace BLL
{
    public class YouIV: BaseAnalysis
    {
        Regex idRegex = new Regex("<title>.*?\\[(.*)\\].*?<\\/title>");
        Regex picRegex = new Regex("forum\\.php\\?mod=attachment&amp;aid=.*?&amp;noupdate=yes");

        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();
            
            try
            {
                string fileName = RemoveExtension(Path.GetFileNameWithoutExtension(path));
                if (fileName.StartsWith("httpsyouiv"))
                {
                    return resList;
                }
                string[] fileNameStr= fileName.Split(new string[] {"^^^"},StringSplitOptions.None);
                His his = new His
                {
                    Vid = fileNameStr[0],
                    Name = fileNameStr[1]
                };
                try
                {
                    string picHtml =  picRegex.Match(content).Value;
                    //https://youiv.tv/forum.php?mod=attachment&aid=MzE5NDcxfGQwYmUxYjZlfDE3MDk0MTg3MTF8MHwxOTY3MDc%3D&noupdate=yes
                    his.Html = "<img src=\"https://youivt.com/" + picHtml + "\"/><br>\n";
                } 
                catch(Exception e)
                {
                    Console.WriteLine(path + "  获取图片失败" +e.ToString());
                }
                resList.Add(his);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "52ivUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}