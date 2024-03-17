using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Concurrent;
using MODEL;
using System.Text.RegularExpressions;
using BLL;
using DB;
using System.Threading;
using System.Diagnostics;
using Framework;

namespace BLL
{
    public class FileBLL
    {


        Filter filter;
        private BlockingQueue<string> queue = new BlockingQueue<string>();
        public FileBLL()
        {
            DBHelper.OpenConnection();
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
            string invalidHTMLSmaller = "<html><body>";
            string resultHTML = "<html><body>";
            ConcurrentBag<His> resultList = new ConcurrentBag<His>();
            ConcurrentBag<His> hisInvalidList = new ConcurrentBag<His>();
            ConcurrentBag<His> invalidList = new ConcurrentBag<His>();
            ConcurrentBag<His> smallerList = new ConcurrentBag<His>();
            Object lockObj = new Object();
            
            String[] path = Directory.GetFiles(directoryStr, "*", SearchOption.TopDirectoryOnly);
            int count = path.Length;
            int i = 0;
            foreach (String p in path)
            {

                queue.Enqueue(p);
            }

            using (CountdownEvent countdown = new CountdownEvent(queue.Count))
            {
                for (int j = 0; j < 10; j++)
                {
                    AsynObj asynObj = new AsynObj
                    {
                        LockObj = lockObj,
                        Ana = ana,
                        Queue = queue,
                        IfCheckHis = ifCheckHis,
                        CountdownEvent = countdown,
                        ResultList = resultList,
                        SmallerList = smallerList,
                        InvalidList = invalidList,
                        HisInvalidList = hisInvalidList
                    };
                    AnalyzerWorker analyzerWorker = new AnalyzerWorker();
                    ThreadPool.QueueUserWorkItem(analyzerWorker.Work, asynObj);
                }
                countdown.Wait();
            }
            
            SortedDictionary<String, His> dic = Sort(resultList);
            SortedDictionary<String, His> hisInvalidDic = Sort(hisInvalidList);
            SortedDictionary<String, His> smallerDic = Sort(smallerList);
            SortedDictionary<String, His> invalidDic = Sort(invalidList);
            foreach (His his in hisInvalidDic.Values)
            {
                invalidHtmlHis += his.Html;
            }
            foreach (His his in smallerDic.Values)
            {
                invalidHTMLSmaller += his.Html;
            }
            foreach (His his in dic.Values)
            {
               resultHTML += his.Html;
               if (his.TorrentPath != "")
               {
                   Tool.MoveFile("result", his.TorrentPath);
               }
               if (his.HtmPath != "")
                   Tool.MoveFile("result", his.HtmPath);
            }
            foreach (His his in invalidDic.Values)
            {
                invalidHTML += his.Html;
            }
            resultHTML += "</body></html>";
            invalidHTML += "</body></html>";
            invalidHtmlHis += "</body></html>";
            invalidHTMLSmaller+= "</body></html>";
            Tool.WriteFile(Path.Combine(directoryStr, "result.htm"), resultHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalid.htm"), invalidHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalidHis.htm"), invalidHtmlHis);
            Tool.WriteFile(Path.Combine(directoryStr, "Smaller.htm"), invalidHTMLSmaller);

        }


        SortedDictionary<String, His> Sort(ConcurrentBag<His> hisList)
        {
            SortedDictionary<String, His> dic = new SortedDictionary<string, His>();
            foreach (His his in hisList)
            {
                if (!dic.Keys.Contains(his.Vid) || dic[his.Vid].Size < his.Size)
                {
                    dic.Remove(his.Vid);
                    if (String.IsNullOrEmpty(his.SortBy))
                        dic.Add(his.Vid, his);
                    else
                        dic.Add(his.SortBy, his);
                }
            }
            return dic;
        }

    }
}

