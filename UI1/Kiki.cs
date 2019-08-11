using BLL;
using Framework;
using MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace UI1
{
    public class Kiki
    {
        string empty = "";
        Regex itemRegex = new Regex("http://kikibt.ws/item/.*.html");
        Regex megRegex = new Regex("<a href='magnet:\\?xt=.*?</a>");
        Regex itemSize = new Regex("文件大小：<b>2.06 GB</b>");
        BlockingQueue<KikiDO> blockingQueue = new BlockingQueue<KikiDO>();
        Dictionary<string, KikiDO> dictionarySearch = new Dictionary<string, KikiDO>();
        
        Regex filesRegex = new Regex("<div class='dd filelist'>.*?</p></div>");

        public BlockingQueue<KikiDO> BlockingQueue { get => blockingQueue; set => blockingQueue = value; }
        public Dictionary<string, KikiDO> DictionarySearch { get => dictionarySearch; set => dictionarySearch = value; }
        public string Empty { get => empty; set => empty = value; }

        public void NavigateHandle(System.Windows.Forms.WebBrowser webBrowser1, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e, string path1)
        {
            webBrowser1.ScriptErrorsSuppressed = false;
            string result="";
            System.IO.StreamReader getReader = new System.IO.StreamReader(webBrowser1.DocumentStream);
            Console.WriteLine(e.Url);
            string gethtml = getReader.ReadToEnd();
            if (gethtml.Contains("500 Internal Privoxy Error"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                webBrowser1.Navigate(e.Url);
                return;
            }

            else
            {
                BlockingQueue.Dequeue();
                if (e.Url.ToString().Contains("/search/"))
                {
                    MatchCollection mc = itemRegex.Matches(gethtml);
                    foreach(Match m in mc)
                    {  
                        KikiDO kikiDO = new KikiDO();
                        kikiDO.SearchUrl = e.Url.ToString() ;
                        kikiDO.Url = m.Value;
                        try
                        {
                            DictionarySearch.Add(m.Value, kikiDO);
                        }
                        catch
                        {
                            Console.WriteLine("duplicate Item:  "+ e.Url + "   "+m.Value);
                        }
                        BlockingQueue.Enqueue(kikiDO);

                    }



                }
                else if(e.Url.ToString().Contains("/item/"))
                {
                    string html = itemAnalysis(gethtml);
                    KikiDO kikiDO = DictionarySearch[e.Url.ToString()];
                    KikiDO kikiDO1 = dictionarySearch[kikiDO.SearchUrl];
                    kikiDO1.Html += html;

                } 

             }
            if (BlockingQueue.Count == 0)
            {
                foreach (var item in DictionarySearch)
                {
                    if (item.Key.Contains("/search/"))
                    {
                        result += item.Value.Html + Tool.splitter+"\n";
                    }
                }
                result += empty;
                Tool.WriteFile(Path.Combine(path1, "resultKiki.htm"), result);
            }
            else
            {
                KikiDO kikiDO1 = BlockingQueue.Peek();
           
                webBrowser1.Navigate(kikiDO1.Url);
            }
        }

        public string itemAnalysis(string content)
        {
            string html= megRegex.Match(content).Value+"<br>\n";
            html += itemSize.Match(content).Value+"<br>\n";
            html += filesRegex.Match(content).Value.Replace("<span class='size'>","   ")+"<br>\n";
            return html;
        }
    }
}
