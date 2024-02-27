using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using Framework;
using MODEL;

namespace BLL
{
    public class AnalyzerWorker
    {
        public void Work(object state)
        {
            AsynObj asynObj = (AsynObj)state;
            while (asynObj.Queue.Count() > 0)
            {
                try
                {
                    Filter filter = new Filter();
                    string path = asynObj.Queue.Dequeue();
                    Console.WriteLine(path);
                    StreamReader sr = new StreamReader(path);
                    string content = sr.ReadToEnd();

                    sr.Close();
                    string[] strs = Path.GetFileNameWithoutExtension(path).Split('_');
                    string vid = "";
                    if (strs.Length > 4)
                        vid = strs[4];

                    ArrayList list = asynObj.Ana.alys(content, path, vid, asynObj.IfCheckHis);
                    foreach (His his in list)
                    {
                        his.Vid = his.Vid.Replace("-", "").Replace("_", "");
                        his.HisTimeSpan = Config.timeSpan;
                        string allId = his.Vid;
                        string[] vids = his.Vid.Split('#');
                        bool isValid = true;

                        foreach (string id in vids)
                        {
                            his.Vid = id;
                            if (Config.isXieZhen)
                            {
                                isValid = filter.checkXieZhen(his);
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
                            foreach (string id in vids)
                            {
                                his.Vid = id;
                                his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, true, his);
                            }

                            if (his.IfExistSmaller)
                            {
                                asynObj.SmallerList.Add(his);
                            }
                            else
                                asynObj.ResultList.Add(his);
                        }
                        else
                        {
                            his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, false, his);
                            if (his.FailReason == "file")
                                asynObj.InvalidList.Add(his);
                            else if (his.FailReason == "his")
                                asynObj.HisInvalidList.Add(his);
                        }

                        if (vids.Length > 1)
                        {
                            Console.WriteLine(allId + "\n" + isValid + "\n" + his.FailReason);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    lock (asynObj.lockObj)
                    {

                        if (asynObj.CountdownEvent.CurrentCount > 0)
                        {
                            asynObj.CountdownEvent.Signal();
                        }
                    }
                }
            }
        }
    }
}