using BencodeLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public abstract class BaseAnalysis:IAnalysis
    {
        public static HashSet<string> EXTENSIONS = new HashSet<string>() { "AVI", "WMV", "M4V", "MP4", "ISO", "MPG" };
        public string RemoveExtension(string name)
        {
            foreach(string ext in EXTENSIONS)
            {
                name = name.Replace(ext, "");
            }
            return name;
        }

        Regex reg1 = new Regex("[A-Z]");
        public string getSearchHtml(string vid, double size)
        {
            vid= vid.Replace("-", "").ToUpper();
            string html="";
            string letter = "";
            string number = "";
            bool isEndofLetter = false;
            foreach (char c in vid)
            {
                if (reg1.IsMatch(c.ToString()))
                {
                    if (isEndofLetter)
                        break;
                    else
                        letter += c;
                }
                else
                {
                    number += c;
                    isEndofLetter = true;
                }
            }
            html += "<a href=\"https://www.google.com.tw/search?um=1&newwindow=1&safe=off&hl=zh-CN&biw=1362&bih=839&dpr=1&ie=UTF-8&tbm=isch&source=og&sa=N&tab=wi&ei=QKr6U8KMKtOWaqbigogK&q=" + vid + "\"/>" + vid + "</a><br>";
            html += size + "<br>";
            html += "<a href=\"http://btdigg.org/search?info_hash=&q=" + letter + "+" + number + "\">" + vid + "</a>\n";
            html += "<a href=\"http://www.javbus.com/" + letter + "-" + number + "\">" + vid + "</a><br>\n";
            html += ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>\n >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            return html;
        }

        public abstract ArrayList alys(string content, string path, string vid);

        public float GetTorrentSize(string p)
        {
            long length=0;
            BDict torrentFile = null;
            BList b;
            try
            {
                torrentFile = BencodingUtils.DecodeFile(p) as BDict;
            }
            catch (Exception e)
            {
               Tool.MoveFile("decodeErr", p);
               return 0;
            }
            if (torrentFile == null)
            {
                Tool.MoveFile("decodeErr", p);
                return 0;
            }
            if ((torrentFile["info"] as BDict).ContainsKey("files"))
            {

                b = (BList)(torrentFile["info"] as BDict)["files"];



                for (int i = 0; i < b.Count; i++)
                {
                    BDict bd = (BDict)b[i];
                    length =length+ ((BInt)bd["length"]).Value;
                    
                }
            }
            else
            {
                length = ((BInt)(torrentFile["info"] as BDict)["length"]).Value;
            }
            return length / 1024 / 1024;
        }
    }
}
