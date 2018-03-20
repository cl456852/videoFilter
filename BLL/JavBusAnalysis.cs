using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class JavBusAnalysis : BaseAnalysis
    {
        Regex imgRegex = new Regex("https://pics.javbus.com/cover.*?.jpg");
        Regex sizeRegex = new Regex(@"\s[1-9]([^\s])*?[0-9]GB|\s[1-9]([^\s])*?[0-9]MB");
        Regex nameRegex = new Regex("<h3>.*</h3>");

        public override ArrayList alys(string content, string path, string vid, bool isCheckHis)
        {
            ArrayList list = new ArrayList();
            try
            {
                if (path.EndsWith("_magenet") || Path.GetFileNameWithoutExtension(path).StartsWith("http^__www.javbus.com"))
                    return new ArrayList();
        
                string id = Path.GetFileNameWithoutExtension(path);
                string img = imgRegex.Match(content).Value;
                His his = new His();
                his.IsCheckSize = false;
                string[] strings = id.Split('-');
                if (strings.Length == 1)
                {
                    his.Vid = strings[0];
                }
                else
                {
                    his.Vid = strings[0] + strings[1];
                }
                StreamReader sr = new StreamReader(path + "_magenet");
                string magContent = sr.ReadToEnd();
                MatchCollection mc = sizeRegex.Matches(magContent);
                double size = 0;
                foreach (Match match in mc)
                {
                    double eachSize = 0;
                    string sizeStr = match.Value;
                    if (sizeStr.Contains("GB"))
                        eachSize = Convert.ToDouble(sizeStr.Replace("GB", "")) * 1024;
                    else
                        eachSize = Convert.ToDouble(sizeStr.Replace("MB", ""));
                    size = eachSize > size ? eachSize : size;
                }
                Match matchName= nameRegex.Match(content);
                his.Name= matchName.Value.Replace("<h3>","").Replace("</h3>","");
                
                his.HisTimeSpan = 120;
                his.IsCHeckHisSize = isCheckHis;
                his.Html += "<img src=\"" + img + "\"/><br>\n<table>";
                his.Html += magContent + "</table><br>\n";
                his.Size = size;
                list.Add(his);
        
            }
            catch (Exception e)
            {
                
                Console.WriteLine("分析文件异常"+e.Message+path);
            }
            return list;

        }
    }
}
