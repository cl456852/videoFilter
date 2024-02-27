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
        Regex idRegex = new Regex("\\[.*?\\]");
        Regex picRegex = new Regex("zoomfile=\".*?\"");
        Regex nameRegex = new Regex("\\[.*?\\[");

        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList resList = new ArrayList();

            try
            {
                string fileName = RemoveExtension(Path.GetFileNameWithoutExtension(path));
                MatchCollection mc = idRegex.Matches(fileName);
                His his = new His();
                his.Vid = mc[0].Value.Replace("-", "").Replace("[","").Replace("]","");
                his.Name = nameRegex.Match(fileName).Value.Replace("[","").Replace("]","");
                his.HisTimeSpan = 999;
                try
                {
                    string sizeStr = mc[1].Value.Split('#')[1].Replace("]","");
               
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
                catch(Exception ex)
                {
                    Console.WriteLine(his.Vid + " SIZE ERROR  "+ ex.ToString());
                }
                try
                {
                    string picHtml = picRegex.Match(content).Value.Replace("zoomfile=\"", "");
                    his.Html = "<img src=\"https://www.52iv.click/" + picHtml + "/><br>\n";
                } 
                catch(Exception e)
                {
                    Console.WriteLine(path + "  获取图片失败" +e.ToString());
                }
                his.IsCHeckHisSize = ifCheckHis;
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
