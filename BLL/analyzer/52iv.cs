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
    public class _52iv : BaseAnalysis
    {
        Regex idRegex = new Regex("<title>.*?\\[(.*)\\].*?<\\/title>");
        Regex picRegex = new Regex("zoomfile=\".*?\"");

        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();

            try
            {
                string fileName = RemoveExtension(Path.GetFileNameWithoutExtension(path));
                His his = new His();
                his.Vid = idRegex.Match(content).Groups[1].Value;
                his.Name = fileName;
                try
                {
                    string picHtml = picRegex.Match(content).Value.Replace("zoomfile=\"", "");
                    his.Html = "<img src=\"https://www.ivxz.pw/" + picHtml + "/><br>\n";
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
