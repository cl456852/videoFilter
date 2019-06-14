using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class Sis001Analysis : BaseAnalysis
    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}|[A-Z]{1,}[0-9]{1,}|[A-Z]{1,}‐[0-9]{1,}");
        Regex sizeRegex = new Regex("size\\^\\^\\^.*");
        Regex imgRegex = new Regex("<img src=\".*?\"");
        Regex torrentLinkRegex = new Regex("attachment.php.*?\"");
        Regex reg1 = new Regex("[a-z]");

        public override ArrayList alys(string content, string path, string vid,bool isCheckHis)
        {
            ArrayList resList = new ArrayList();
            try
            {
                
                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()).Replace("S1",""));
                bool hasS1=false;
                //foreach(Match m in mc)
                //{
                //    if (m.Value == "S1")
                //        hasS1 = true;
                //}
                //if (mc.Count >2||mc.Count==2&&!hasS1)
                //{
                //    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "sisUnknown");
                //    if (!Directory.Exists(unknownPath))
                //        Directory.CreateDirectory(unknownPath);
                //    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                //    return resList;
                //}

                string idLetter="";
                string idNumber="";
                foreach(Match m in mc)
                {
                    string id = m.Value.Replace("-", "").ToLower().Replace("‐","");
                    string letter = "";
                    string number = "";
                    bool isEndofLetter = false;
                    for (int i = 0; i < id.Length; i++)                        
                        if (reg1.IsMatch(id[i].ToString()))
                        {
                            if (isEndofLetter)
                                break;
                            else
                                letter += id[i];
                        }
                        else
                        {
                            number += id[i];
                            isEndofLetter = true;
                        }
                    if(number.Length>1&&letter.Length>1)
                    {
                        if (letter.Length > idLetter.Length)
                        {
                            idLetter = letter;
                            idNumber = number;
                        }
                        else if(letter.Length==idLetter.Length&&number.Length>idNumber.Length)
                        {
                            idLetter = letter;
                            idNumber = number;
                        }
                    }
                }
                string id1 = idLetter + idNumber;
                if(String.IsNullOrEmpty(id1))
                {
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "sisUnknown");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                    return resList;
                }
                His his = new His();
                his.Vid = id1;
                his.Size = Convert.ToDouble(sizeRegex.Match(path).Value.Replace("size^^^", "").Replace(".htm", ""));
                his.Html = content.Split(new string[] { "count_add_one", "下载次数:" }, StringSplitOptions.RemoveEmptyEntries)[1];
                his.Name = Path.GetFileNameWithoutExtension(path.ToUpper()).Split(new string[] { "SIZE^^^" }, StringSplitOptions.RemoveEmptyEntries)[0];
                //if(his.Name.StartsWith("[FHD"))
                //{
                //    his.FailReason = "44x";
                //}
                string torrentLink = "http://sis001.com/bbs/" + torrentLinkRegex.Match(his.Html).Value.Replace("\"","")+ "&clickDownload=1";

                MatchCollection imgMc = imgRegex.Matches(his.Html);
                his.Html = "";
                foreach (Match match in imgMc)
                {
                    if (!match.Value.Contains("torrent.gif"))
                    {
                        his.Html += "<a href=\"" + torrentLink + "\">" + match.Value + "/></a><br>";
                    }
                }
                his.HisTimeSpan = 999;
                his.IsCHeckHisSize = isCheckHis;
                resList.Add(his);
                
            }
            catch (Exception e)
            {
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "sisUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
