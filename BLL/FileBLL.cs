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

        public void process(string directoryStr, IAnalysis ana, bool ifCheckHis)
        {
            string invalidHtmlHis = "<html><body>";
            string invalidHTML="<html><body>";
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
                    if (filter.checkValid(his))
                    {
                        his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name,true);
                        hisList.Add(his);
                    }
                    else
                    {
                        his.Html += BaseAnalysis.getSearchHtml(his.Vid, his.Size, his.Name, false);
                        if (his.FailReason == "file")
                            invalidHTML += his.Html;
                        else
                            invalidHtmlHis += his.Html;
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

            Tool.WriteFile(Path.Combine(directoryStr, "result.htm"), resultHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalid.htm"), invalidHTML);
            Tool.WriteFile(Path.Combine(directoryStr, "invalidHis.htm"), invalidHtmlHis);

        }

        

       
    }
}

