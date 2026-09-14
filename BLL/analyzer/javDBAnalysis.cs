
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class JavDBAnalysis : BaseAnalysis
    {
        Regex imgRegex = new Regex("<img[^>]+?class=['\"][^'\"]*?video-cover[^'\"]*?['\"][^>]*?>|<img[^>]+?src=['\"][^'\"]+?['\"][^>]*?class=['\"][^'\"]*?video-cover[^'\"]*?['\"][^>]*?>", RegexOptions.Singleline);
        // <div id="magnets-content" class="magnet-links" data-magnet-sort-target="list">.*?</article>
        Regex magRegex = new Regex("<div[^>]*?(id=['\"]magnets-content['\"]|class=['\"][^'\"]*?magnet-links[^'\"]*?['\"])[^>]*?>.*?</article>", RegexOptions.Singleline);
        Regex fallbackMagRegex = new Regex("<div[^>]*?id=['\"]magnets-content['\"][^>]*?>.*?</div>\\s*</div>", RegexOptions.Singleline);
        Regex sizeRegex = new Regex("<span[^>]*?class=['\"][^'\"]*?meta[^'\"]*?['\"][^>]*?>.*?</span>", RegexOptions.Singleline);

        public override ArrayList alys(string content, string path, string path1, bool ifCheckHis) 
        {
            ArrayList list = new ArrayList();
            try
            {
                His his = new His();
                ArrayList resList = new ArrayList();
                his.Vid = Path.GetFileName(path).Split(new string[] { "$$$" }, StringSplitOptions.None)[0];
                his.Name = Path.GetFileNameWithoutExtension(path).ToString().Replace("$$$", "");

                string imgUrl = "";
                Match imgMatch = imgRegex.Match(content);
                if (imgMatch.Success)
                {
                    Match srcMatch = Regex.Match(imgMatch.Value, "src=['\"]([^'\"]+)['\"]");
                    if (srcMatch.Success)
                    {
                        imgUrl = srcMatch.Groups[1].Value.Trim();
                    }
                }
                if (!string.IsNullOrEmpty(imgUrl))
                {
                    his.Html += "<img src=\"" + imgUrl + "\"/><br>\n";
                }

                Match magMatch = magRegex.Match(content);
                if (!magMatch.Success)
                {
                    magMatch = fallbackMagRegex.Match(content);
                }

                string magHtml = magMatch.Success ? (magMatch.Value + "\n") : "";
                if (!string.IsNullOrEmpty(magHtml))
                {
                    magHtml = Regex.Replace(magHtml, @"<div[^>]*?class=['""][^'""]*?sda-content[^'""]*?['""][^>]*?>.*?</div>\s*", "", RegexOptions.Singleline);
                    magHtml = Regex.Replace(magHtml, @"<a[^>]*?>\s*<img[^>]*?/ads/[^>]*?>\s*</a>\s*", "", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                }
                his.Html += magHtml;

                double size;
                MatchCollection mc = sizeRegex.Matches(magHtml);
                foreach (Match m in mc)
                {
                    string sizeDigit = m.Value.Replace("<span class=\"meta\">", "").Replace("</span>", "").Trim();
                    try
                    {
                        Match sizeMatch = Regex.Match(sizeDigit, @"(?<num>\d+(\.\d+)?)\s*(?<unit>GB|MB|KB)", RegexOptions.IgnoreCase);
                        if (sizeMatch.Success)
                        {
                            double val = Convert.ToDouble(sizeMatch.Groups["num"].Value, CultureInfo.InvariantCulture);
                            string unit = sizeMatch.Groups["unit"].Value.ToUpper();
                            if (unit == "GB")
                                size = val * 1024;
                            else if (unit == "MB")
                                size = val;
                            else if (unit == "KB")
                                size = val / 1024;
                            else
                                size = 0;
                            his.Size = his.Size > size ? his.Size : size;
                        }
                        else
                        {
                            sizeDigit = sizeDigit.Split(',')[0].Trim();
                            if (sizeDigit.EndsWith("GB", StringComparison.OrdinalIgnoreCase))
                            {
                                sizeDigit = sizeDigit.Substring(0, sizeDigit.Length - 2).Trim();
                                size = Convert.ToDouble(sizeDigit, CultureInfo.InvariantCulture) * 1024;
                            }
                            else if (sizeDigit.EndsWith("MB", StringComparison.OrdinalIgnoreCase))
                            {
                                sizeDigit = sizeDigit.Substring(0, sizeDigit.Length - 2).Trim();
                                size = Convert.ToDouble(sizeDigit, CultureInfo.InvariantCulture);
                            }
                            else if (sizeDigit.EndsWith("KB", StringComparison.OrdinalIgnoreCase))
                            {
                                sizeDigit = sizeDigit.Substring(0, sizeDigit.Length - 2).Trim();
                                size = Convert.ToDouble(sizeDigit, CultureInfo.InvariantCulture) / 1024;
                            }
                            else
                                size = 0;
                            his.Size = his.Size > size ? his.Size : size;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("分析文件异常" + e.Message + path + "    " + sizeDigit);
                    }
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