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
    public class Hdd600 : BaseAnalysis
    {
        
        Regex sizeRegex = new Regex(@"([1-9][\d]*|0)(\.[\d]+)?(GiB|GB)");
        Regex sizeRegexMB = new Regex(@"([1-9][\d]*|0)(\.[\d]+)?(MiB|MB)");

        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList list = new ArrayList();
            His his = new His();
            try {
                string fileName = Path.GetFileNameWithoutExtension(path);
                string[] strs = fileName.Split(new string[2] { "(", ")" }, StringSplitOptions.None);
                if (!fileName.StartsWith("("))
                {
                    his.Vid = strs[0];
                }
                else
                {

                    string id = strs[strs.Length - 2];
                    string number = "";
                    string idStr = "";
                    bool hasChar = false;
                    for (int i=id.Length-1;i>=0;i--)
                    {
                        if (char.IsDigit(id[i]))
                        {
                            if (hasChar)
                                break;
                            else
                                number += id[i];
                        }
                        else
                        {
                            idStr += id[i];
                            hasChar = true;
                        }
                    }
                    idStr = string.Concat(Enumerable.Reverse(idStr));
                    number= string.Concat(Enumerable.Reverse(number));
                    if (number.Length >= 5 && number[0] == '0' && number[1] == '0')
                    {
                        number = number.Substring(2);
                    }
                    his.Vid = idStr + number;
                }
                if(his.Vid=="")
                {
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "Hdd600Unknown");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                    Console.WriteLine("VID为空  " + path);
                }
                string[] names = path.Split(new string[] { ")" }, StringSplitOptions.None);
                his.Name = names[names.Length - 1].Replace(".htm","");
                string sizeStr = sizeRegex.Match(content).Value.ToUpper().Replace("GIB", "").Replace("GB", "");
                if (sizeStr != "")
                    his.Size = Convert.ToDouble(sizeStr) * 1024;
                else
                {
                    sizeStr = sizeRegexMB.Match(content).Value.ToUpper().Replace("MIB", "").Replace("MB", "");
                    his.Size = Convert.ToDouble(sizeStr);
                }
                his.Html = Regex.Match(content, "<tbody><tr><td class=\"t_f\" id=\"postmessage.*?</tbody>", RegexOptions.Singleline).Value+"<br>\n";
                list.Add(his);
                his.HisTimeSpan = 6;
            }
            catch (Exception e)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "Hdd600Unknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                Console.WriteLine("分析文件异常"+e.Message+path);
            }
            return list;
        }
    }
}
