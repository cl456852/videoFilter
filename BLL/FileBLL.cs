using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Linq;
using System.IO;
using System.Collections;
using MODEL;
using System.Text.RegularExpressions;
using BLL;
using DB;
using System.Threading;
using System.Diagnostics;

namespace BLL
{
    public class FileBLL
    {


        Filter filter;

        public FileBLL()
        {
            filter = new Filter();
        }

        public List<MyFileInfo> getFileList()
        {
            return FileDAL.selectMyFileInfo("");
        }

        void curlCheck()
        {
            while (true)
            {
                Process[] processes = Process.GetProcesses();

                foreach (Process p in processes)
                {

                    try
                    {
                        if (p.ProcessName.Contains("curl"))
                        {
                            Console.WriteLine(String.Format("{0} {1} {2} {3}", p.Id, p.ProcessName, p.BasePriority, p.StartTime));
                            if ((DateTime.Now - p.StartTime).Seconds > 30)
                            {
                                p.Kill();
                                Console.WriteLine("curl killed");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                Thread.Sleep(30000);
            }
        }

        public void process(string directoryStr, IAnalysis ana, bool ifCheckHis)
        {
            Thread th = new Thread(curlCheck);
            th.Start();

            string invalidHtmlHis = "<html><body>";
            string invalidHTML="<html><body>";
            string invalidHTML44x = "<html><body>";
            string blackListHTML= "<html><body>";
            ArrayList hisList = new ArrayList();
            string resultHTML = "<html><body>";
            String[] path = Directory.GetFiles(directoryStr, "*", SearchOption.TopDirectoryOnly);
            foreach (String p in path)
            {
                Console.WriteLine(p);
                StreamReader sr = new StreamReader(p);
                string content = sr.ReadToEnd();

                sr.Close();
                string[] strs = Path.GetFileNameWithoutExtension(p).Split('_');
                string vid="";
                if (strs.Length > 4)
                    vid = strs[4];
                ArrayList list = ana.alys(content, p, vid, ifCheckHis);
                foreach (His his in list)
                {
                    his.Vid = his.Vid.Replace("-", "").Replace("_", "");
                    if (his.IsBlack)
                    {
                        DBHelper.insertBLackList(his);
                        blackListHTML += his.Html;
                        continue;
                    }
                    if(filter.CheckBlackList(his))
                    {
                        blackListHTML += his.Html;
                        continue;
                    }
                    if (filter.checkValid(his))
                    {
                        his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, true,his);
                        hisList.Add(his);
                    }
                    else
                    {
                        his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, false,his);
                        if (his.FailReason == "file")
                            invalidHTML += his.Html;
                        else if (his.FailReason == "his")
                            invalidHtmlHis += his.Html;
                        else if (his.FailReason == "44x")
                        {
                            invalidHTML44x += his.Html;
                        }
                    }
                }

            }
                      SortedDictionary<String, His> dic = new SortedDictionary<string, His>();
            foreach(His his in hisList)
            {
                if(!dic.Keys.Contains(his.Vid)||dic[his.Vid].Size<his.Size)
                {
                    dic.Remove(his.Vid);
                    dic.Add(his.Vid,his);
                }
            }
           foreach(His his in dic.Values)
           {
               resultHTML += his.Html;
               if (his.TorrentPath != "")
               {
                   Tool.MoveFile("result", his.TorrentPath);
               }
               if (his.HtmPath != "")
                   Tool.MoveFile("result", his.HtmPath);
           }
            resultHTML += "</body></html>";
            invalidHTML += "</body></html>";
            invalidHtmlHis += "</body></html>";
            blackListHTML+= "</body></html>";
            invalidHTML44x+= "</body></html>";
            Tool.WriteFile(Path.Combine(directoryStr, "result.htm"), resultHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalid.htm"), invalidHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalidHis.htm"), invalidHtmlHis);
            Tool.WriteFile(Path.Combine(directoryStr, "blackList.htm"), blackListHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "44x.htm"), invalidHTML44x);

        }

        

       
    }
}

