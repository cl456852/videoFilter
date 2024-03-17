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
        Regex vidRegex = new Regex(@"[A-Za-z]{1,6}\d{5}");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList list = new ArrayList();
            His his = new His();
            try {
                string fileName = Path.GetFileNameWithoutExtension(path);
                
                int index = fileName.IndexOf('['); // Find the index of the first '['

                if (index != -1) // Check if '[' was found
                {
                    his.Vid= fileName.Substring(0, index).Trim(); // Extract substring before '['
                }
                else
                {
                    
                    his.Vid = vidRegex.Match(fileName).Value;
                    string letters = "";
                    string numbers = "";

                    foreach (char c in his.Vid)
                    {
                        if (Char.IsLetter(c))
                        {
                            letters += c;
                        }
                        else if (Char.IsDigit(c))
                        {
                            numbers += c;
                        }
                    }
                    if (numbers.StartsWith("0000"))
                    {
                        numbers = numbers.Replace("0000", "00");
                    }
                    else if(numbers.StartsWith("00"))
                    {
                        numbers = numbers.Substring(2);
                    }
                    else if(numbers.StartsWith("0"))
                    {
                        numbers = numbers.Substring(1);
                    }
                    his.Vid = letters + numbers;
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
                    if (sizeStr != "")
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
