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
using Framework;

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
            string invalidHTMLSmaller = "<html><body>";
            string blackListHTML= "<html><body>";
            ArrayList hisList = new ArrayList();
            ArrayList hisInvalidLIst = new ArrayList();

            ArrayList smallerList = new ArrayList();
            string resultHTML = "<html><body>";
            String[] path = Directory.GetFiles(directoryStr, "*", SearchOption.TopDirectoryOnly);
            int count = path.Length;
            int i = 0;
            foreach (String p in path)
            {

                Console.WriteLine(count - i++);
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
                    his.HisTimeSpan = Config.timeSpan;
                    string allId = his.Vid;
                    string[] vids = his.Vid.Split('#');
                    bool isValid = true;
        
                    foreach(string id in vids)
                    {
                        his.Vid = id;
                        if (Config.isXieZhen)
                        {
                            isValid= filter.checkXieZhen(his);

                        }
                        else
                        {
                            isValid = filter.checkValid(his);
                            if (!isValid)
                            {
                                break;
                            }
                        }
                    }
                    if (isValid)
                    {
                        foreach(string id in vids)
                        {
                            his.Vid = id;
                            his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, true, his);
                        }
                        if(his.IfExistSmaller)
                        {
                            smallerList.Add(his);
                        }
                        else
                            hisList.Add(his);
                    }
                    else
                    {
                        his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, false,his);
                        if (his.FailReason == "file")
                            invalidHTML += his.Html;
                        else if (his.FailReason == "his")
                            hisInvalidLIst.Add(his);
                
                    }

                    if (vids.Length > 1)
                    {

                        Console.WriteLine(allId);
                        Console.WriteLine(isValid);
                        Console.WriteLine(his.FailReason);
                    }
                }

            }
            SortedDictionary<String, His> dic = Sort(hisList);
            SortedDictionary<String, His> invalidDic = Sort(hisInvalidLIst);
            SortedDictionary<String, His> smallerDic = Sort(smallerList);
            foreach (His his in invalidDic.Values)
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
            resultHTML += "</body></html>";
            invalidHTML += "</body></html>";
            invalidHtmlHis += "</body></html>";
            blackListHTML+= "</body></html>";
            invalidHTMLSmaller+= "</body></html>";
            Tool.WriteFile(Path.Combine(directoryStr, "result.htm"), resultHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalid.htm"), invalidHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalidHis.htm"), invalidHtmlHis);
            Tool.WriteFile(Path.Combine(directoryStr, "blackList.htm"), blackListHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "Smaller.htm"), invalidHTMLSmaller);

        }


        SortedDictionary<String, His> Sort(ArrayList hisList)
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

