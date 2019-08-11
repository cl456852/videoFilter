
using BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace UI1
{
    public class MegFinder
    {
        string[] htmlList;
        int index;
        int itemIndex;
        Regex regex=new Regex("<a href=\" http://kikibt.ws/search/.*/>");
        Regex itemRegex = new Regex("http://kikibt.ws/item/.*.html");
        Regex megRegex = new Regex("<a href='magnet:\\?xt=.*?'>");
        MatchCollection mc;
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string[] itemList;
        string path;
        public void NavigateHandle(System.Windows.Forms.WebBrowser webBrowser1, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e, string path1)
        {
            System.IO.StreamReader getReader = new System.IO.StreamReader(webBrowser1.DocumentStream);
            Console.WriteLine(e.Url);
            string gethtml = getReader.ReadToEnd();
            if (gethtml.Contains("500 Internal Privoxy Error"))
            {
                Console.WriteLine("500 Internal Privoxy Error TRY AGAIN");
                webBrowser1.Navigate(e.Url);
                return;
            }
            else if(e.Url.ToString().Contains("/search/"))
            {
                mc= itemRegex.Matches(gethtml);
                itemList= gethtml.Split(new string[] { "<dl>", "</dl>" }, StringSplitOptions.RemoveEmptyEntries);
                getNextItem(webBrowser1);

            }
            else if(e.Url.ToString().Contains("/item/"))
            {
               string megUrl=  megRegex.Match(gethtml).Value+ "magnet</a>";
               htmlList[index-1]=htmlList[index-1]+  dictionary[e.Url.ToString()] + "<br>" + megUrl;
                if (htmlList.Count() > index)
                {
                    getNextItem(webBrowser1);
                }
                else
                {
                    string result="";
                    foreach(string s in htmlList)
                    {
                        result += s + "<<<<<<<<<<<<<<<<<<<<";
                    }
                    Tool.WriteFile(Path.Combine(path1, "resultKiki.htm"), result);
                }
                
            }
        }

        void getNextItem(WebBrowser webBrowser1)
        {

            if (mc.Count > itemIndex)
            {
                webBrowser1.Navigate(mc[itemIndex].Value);
                dictionary.Add(mc[itemIndex].Value, itemList[itemIndex + 1]);
                itemIndex++;
            }
            else if(htmlList.Count() > index)
            {
                string url = regex.Match(htmlList[index]).Value.Replace("<a href=\" ", "").Replace("\"/>", "");
                webBrowser1.Navigate(url);
                index++;
            }
            else
            {
                string result = "";
                foreach (string s in htmlList)
                {
                    result += s + "<<<<<<<<<<<<<<<<<<<<";
                }
                Tool.WriteFile(Path.Combine(path, "resultKiki.htm"), result);
            }
        }

        public void addKiki(string path1, WebBrowser webBrowser1)
        {
            path = path1;
            StreamReader sr = new StreamReader(path1);
            string magContent = sr.ReadToEnd();
            string url="";
            htmlList = magContent.Split(new string[] { ">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in htmlList)
            {
                url = regex.Match(htmlList[index]).Value.Replace("<a href=\" ", "").Replace("\"/>", "");
                index++;
                if (!String.IsNullOrEmpty(url))
                {
                    break;
                }
                
            }
            webBrowser1.Navigate(url);
        }
    }
}