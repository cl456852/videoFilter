using BencodeLibrary;

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class TorrentAnalysis : BaseAnalysis

    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,}|[A-Z]{1,}[0-9]{1,}|[A-Z]{1,}‐[0-9]{1,}");
        Regex MMRRegex = new Regex("M[A-Z].*?\\W[A-Z].*?\\W");

        public override ArrayList alys(string content, string p, string vid, bool ifCheckHis)
        {
            ArrayList arrayList = new ArrayList();
            BList b;
            BDict torrentFile = BencodingUtils.DecodeFile(p) as BDict;
            if ((torrentFile["info"] as BDict).ContainsKey("files"))
            {
                b = (BList)(torrentFile["info"] as BDict)["files"];
                for (int i = 0; i < b.Count; i++)
                {
                    His his = new His(); 
                    BDict bd = (BDict)b[i];
                    BList list = (BList)bd["path"];
                    long length = ((BInt)bd["length"]).Value;
                    string s = ((BString)list[list.Count - 1]).Value;
                    foreach(BString bString in list)
                    {
                        his.SortBy+=bString.Value+"\\";
                    }
                    his.Name = his.SortBy;
                    his.Size = length / 1024 / 1024;
                    if(his.Size<25)
                    {
                        continue; 
                    }
                    if (s.StartsWith("MMR-") || s.StartsWith("MBR-")||s.StartsWith("MAR-"))
                    {
                        try
                        {
                            his.Vid = MMRRegex.Match(s).Value;
                            his.Vid = his.Vid.Substring(0, his.Vid.Length - 1);
                        }
                        catch(Exception e)
                        {
                            his.Vid = idRegex.Match(s).Value;
                        }

                    }
                    else
                    {
                        his.Vid = idRegex.Match(s).Value;
                    }
                    if (String.IsNullOrEmpty(his.Vid))
                    {
                        Console.WriteLine(s);
                        continue;
                    }
                    his.Html = s+"<br>";
                    his.IsCHeckHisSize = ifCheckHis;
                    his.HisTimeSpan = 999;
                    arrayList.Add(his);
                }
            }
            return arrayList;
        }
    }
}