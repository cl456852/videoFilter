﻿
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class JavDBAnalysis : BaseAnalysis
    {
        Regex imgRegex = new Regex("<img src=\".*?\" class=\"video-cover\">");
        Regex magRegex = new Regex("<div id=\"magnets-content\">.*?</div>",RegexOptions.Singleline);
        Regex sizeRegex = new Regex("\\&nbsp;\\( .*");

        public override ArrayList alys(string content, string path, string path1, bool ifCheckHis) 
        {
            ArrayList list = new ArrayList();
            try
            {


                His his = new His();
                his.IsCHeckHisSize = ifCheckHis;
                ArrayList resList = new ArrayList();
                his.Vid = Path.GetFileName(path).Split(new string[] { "$$$" }, StringSplitOptions.None)[0];
                his.Name = Path.GetFileNameWithoutExtension(path).ToString().Replace("$$$", "");
                string imgUrl = imgRegex.Match(content).Value.Replace("<img src=\"", "").Replace("\" class=\"video-cover\">", "") + "\n";
                string magHtml = magRegex.Match(content).Value + "\n";
                his.Html += "<img src=\"" + imgUrl + "\"/><br>\n";
                his.Html += magHtml;
                double size;
                MatchCollection mc = sizeRegex.Matches(magHtml);
                foreach (Match m in mc)
                {
                    string sizeDigit = m.Value.Replace("&nbsp;( ", "");
                    if (sizeDigit.EndsWith("GB"))
                    {
                        sizeDigit = sizeDigit.Replace("GB", "");
                        size = Convert.ToDouble(sizeDigit) * 1024;

                    }
                    else
                        size = Convert.ToDouble(sizeDigit.Replace("MB", "").Replace("KB", ""));
                    his.Size = his.Size > size ? his.Size : size;
                }
                list.Add(his);
            }
            catch (Exception e)
            {
                Console.WriteLine("分析文件异常" + e.Message + path);
            }

            return list;
        }

    }
}