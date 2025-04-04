﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class _168x : BaseAnalysis
    {
        Regex reg1 = new Regex("[A-Z]");
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}");
        Regex idRegex1 = new Regex("[A-Z]{1,}[0-9]{1,}");
        Regex sizeRegex = new Regex(@"(\d+\.?\d*)(GB)");
        Regex picRegex = new Regex(" file=\".*?\"");
        //https://www.rysuanaaser.com/tupian/down.php?module=forum&amp;attachment=202108/25/110333wu4ekku3ik3ligjj.attach&amp;filename=venx-068.2021.08.14.4k.x264.acc-JapornX.mp4.torrent&amp;filesize=41522&amp;dateline=1629857012" target="_blank">venx-068.2021.08.14.4k.x264.acc-JapornX.mp4.torrent</a>
        //https://www.rysuanaaser.com/tupian/down.php?module=forum&amp;attachment=202108/28/123747iikwkkv669vkzw3v.attach&amp;filename=mcsr-448.torrent&amp;filesize=36639&amp;dateline=1630125467
        //Regex torrentRegex = new Regex("<a href=\"https://www.rysuanaaser.com.*?\"");
        Regex torrentRegex = new Regex("magnet:\\?xt=.*?<");
        
        public override ArrayList alys(string content, string path, string vid, bool isCheckHis)
        {
            His his = new His();
            ArrayList resList = new ArrayList();
            try
            {
                MatchCollection mc = idRegex.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                if (mc.Count == 0)
                {
                    mc = idRegex1.Matches(Path.GetFileNameWithoutExtension(path.ToUpper()));
                    string id = mc[0].Value;
                    string letter = "";
                    string number = "";
                    bool isEndofLetter = false;
                    for (int i = 0; i < id.Length; i++)                        //修改   对于出现KIDM235A  KIDM235B
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

                    if (number.StartsWith("00"))
                    {
                        number = number.Substring(2);
                    }
                    his.Vid = letter + number;
                }
                else
                {
                    his.Vid = mc[0].Value.Replace("-", "");
                }
                if (mc.Count != 1)
                {
                    String unknownPath = Path.Combine(Path.GetDirectoryName(path), "huaSeUnknown");
                    if (!Directory.Exists(unknownPath))
                        Directory.CreateDirectory(unknownPath);
                    File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
                    return resList;
                }

                his.Name = Path.GetFileNameWithoutExtension(path.ToUpper()).Replace(mc[0].Value, "");

                string sizeStr = sizeRegex.Match(content).Value.Replace("<","");
                if (sizeStr != "")
                {


                    if (sizeStr.ToUpper().Contains("G"))
                    {
                        sizeStr = sizeStr.ToUpper().Replace("GB", "").Replace("G","");
                        his.Size = Convert.ToDouble(sizeStr) * 1024; 
                    }
                    else
                    {
                        sizeStr = sizeStr.ToUpper().Replace("MB", "").Replace("M","");
                        his.Size = Convert.ToDouble(sizeStr);
                    }
                }
                MatchCollection matchCollection = torrentRegex.Matches(content);
                string torrentLink="";
                
                for (int i=matchCollection.Count-1;i>=0;i--)
                {

                    torrentLink= matchCollection[i].Value.Replace("<", "");
                }
                
                

                MatchCollection picMc = picRegex.Matches(content);
                foreach (Match m in picMc)
                {
                    his.Html += "<a href=\"" + torrentLink + "\"><img src=\"" + m.Value.Replace("file=\"","") + " /></a><br>";
                }

                his.HisTimeSpan = 999;
                resList.Add(his);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                String unknownPath = Path.Combine(Path.GetDirectoryName(path), "168xUnknown");
                if (!Directory.Exists(unknownPath))
                    Directory.CreateDirectory(unknownPath);
                File.Move(path, Path.Combine(unknownPath, Path.GetFileNameWithoutExtension(path)) + ".htm");
            }
            return resList;
        }
    }
}
