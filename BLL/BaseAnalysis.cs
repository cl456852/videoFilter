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

        static Regex reg1 = new Regex("[A-Z]");
        public static string getSearchHtml(string vid, double size, string name,bool getKiki,His his)
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
            //http://javscreens.com/d/div-235.html
            string[] idSplit = Tool.getId(vid.ToLower());

            //html += "<a href=\"http://javscreens.com/" + vid.ToLower()[0]+"/"+idSplit[0] +"-"+idSplit[1]+ ".html\"/>" + vid + "</a><br>";
            //http://video-jav.net/wp-content/uploads/NMK-030_Video-JAV.net_.mp4.scrlist.jpg
            html += "<a href=\"http://video-jav.net/wp-content/uploads/" + idSplit[0].ToUpper() + "-" + idSplit[1] + "_Video-JAV.net_.mp4.scrlist.jpg\"/>" +vid+"</a><br>";
            html += "<a href=\"https://www.google.com.tw/search?um=1&newwindow=1&safe=off&hl=zh-CN&biw=1362&bih=839&dpr=1&ie=UTF-8&tbm=isch&source=og&sa=N&tab=wi&ei=QKr6U8KMKtOWaqbigogK&q=" + vid + "\"/>" + vid + "</a><br>";
            if (!string.IsNullOrEmpty(name))
                html += "<a href=\"https://www.google.com.tw/search?as_st=y&tbm=isch&hl=zh-CN&as_q=" + name + "&as_epq=&as_oq=&as_eq=&cr=&as_sitesearch=&safe=images&tbs=iar:t#imgrc=l5VFSis1_tEGOM:\"/>" + name + "</a><br>"; 
               // html += "<a href=\"https://www.google.com.tw/search?um=1&newwindow=1&safe=off&hl=zh-CN&biw=1362&bih=839&dpr=1&ie=UTF-8&tbm=isch&source=og&sa=N&tab=wi&ei=QKr6U8KMKtOWaqbigogK&q=" + name + "\"/>" + name + "</a><br>";
            html += size + "<br>";
            html += "<a href=\"http://www.btanx.com/search/" + letter + "%20" + number + "-size-desc-1\"/>" +vid + "</a><br>\n";
            html += "<a href=\"http://www.javbus.com/" + letter + "-" + number + "\">" + vid + "</a><br>\n";
            if(getKiki)
                html += "<a href=\"" + KikiBt(letter + " " + number) + "\"/>" + vid + "</a><br>\n";
            if (his.IfExistSmaller)
                html += "ExistSmaller<br>\n";
            html += ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>><br>\n";
            return html;
        }

        public abstract ArrayList alys(string content, string path, string vid,bool ifCheckHis);

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

        public static string KikiBt(string keyword)
        {
            string str = "d:\\curl \"http://kikibt.co/\" -H \"Cookie: __cfduid=d84d7ed42600397ebc0617366fc2e02bc1477489552; a2204_times=13; CNZZDATA1260997767=915447713-1482412125-^%^7C1492937714; CNZZDATA1261675006=724777673-1492252758-^%^7C1492937311; UM_distinctid=15ed86daa300-0f615ded0eedfe-3a3e5c06-1fa400-15ed86daa34ba6; __atuvc=3^%^7C36^%^2C0^%^7C37^%^2C0^%^7C38^%^2C0^%^7C39^%^2C45^%^7C40; CNZZDATA1261857871=1481140133-1494768247-^%^7C1507030268; CNZZDATA1261841250=1785091964-1494766693-^%^7C1507028747; Hm_lvt_bd3d4db2c728324e870543c59e9e3b89=1504709377,1506869631; Hm_lpvt_bd3d4db2c728324e870543c59e9e3b89=1507031416; a5161_pages=1; a5161_times=10; Hm_lvt_f75b813e9c1ef4fb27eaa613c9f307b2=1504709378,1506869631; Hm_lpvt_f75b813e9c1ef4fb27eaa613c9f307b2=1507031416\" -H \"Origin: http://kikibt.net\" -H \"Accept-Encoding: gzip, deflate\" -H \"Accept-Language: zh-CN,zh;q=0.8,en;q=0.6,es;q=0.4\" -H \"Upgrade-Insecure-Requests: 1\" -H \"User-Agent: Mozilla/5.0 (Windows NT 6.3; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.101 Safari/537.36\" -H \"Content-Type: application/x-www-form-urlencoded\" -H \"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8\" -H \"Cache-Control: max-age=0\" -H \"Referer: http://kikibt.net/search/e7Zl9_O5DQA/1-0-0.html\" -H \"Connection: keep-alive\" --data \"keyword=" + keyword + "\" --compressed -vi --connect-timeout 10";
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示程序窗口
            p.Start();//启动程序

            //向cmd窗口发送输入信息
            p.StandardInput.WriteLine(str + "&exit");

            p.StandardInput.AutoFlush = true;
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令



            //获取cmd窗口的输出信息
            //string output = p.StandardOutput.ReadToEnd();
            //string[] spliter = new string[] { "Location:", "\r\n" };
            //string[] res = output.Split(spliter, StringSplitOptions.RemoveEmptyEntries);

            StreamReader reader = p.StandardOutput;
            string line="";
            while (!reader.EndOfStream)
            {
            
                line = reader.ReadLine();
                if(line.StartsWith("Location:"))
                {
                    line = line.Replace("Location:", "");
                    break;
                }
            }



            p.Close();


            Console.WriteLine(line);
            return line;
        }

    }
}
