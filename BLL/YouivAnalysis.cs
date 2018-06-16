using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BLL
{
    public class YouivAnalysis : BaseAnalysis
    {
        Regex picRegex = new Regex("zoomfile=\".*?\"");
        Regex idRegex1 = new Regex("[A-Z]{1,}-[0-9]{1,}|[A-Z]{1,}[0-9]{1,}|[A-Z]{1,}‐[0-9]{1,}");
        Regex idRegex = new Regex("<a href=\"forum.php\\?mod=attachment&.*? target=\"_blank\">.*?torrent</a>");
        //https://youiv.tv/data/attachment/forum/201803/13/230458ayftyotraggsy7ts.jpg
        public override System.Collections.ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            His his = new His();
            ArrayList resList = new ArrayList();

            //if (!Path.GetFileNameWithoutExtension(path).StartsWith("[") || Path.GetFileNameWithoutExtension(path).StartsWith("[]"))
            if (!Path.GetFileNameWithoutExtension(path).StartsWith("[") )
            {
                Tool.MoveFile("unknown", path);
                return resList;
            }

            //string value = idRegex.Match(content).Value;
            //if (!String.IsNullOrEmpty(value))
            //{
            //    string torernt = idRegex.Match(content).Value.Split(new string[] { "target=\"_blank\"", "</a>" }, StringSplitOptions.RemoveEmptyEntries)[1];
            //    string id = idRegex1.Match(torernt.ToUpper().Replace("-", "")).Value;
            //    his.Vid = id;

            //}
            string[] strs = Path.GetFileNameWithoutExtension(path).Replace("[]", "").Split(new string[] { "[", "]" }, StringSplitOptions.None);
            if (strs.Length>=2 )
            {
                his.Vid = strs[1];
            }
            if(String.IsNullOrEmpty(his.Vid))
            {
                Tool.MoveFile("unknown", path);
                return resList;
            }
           
            MatchCollection mc = picRegex.Matches(content);
        
            foreach(Match m in mc)
            {
                
                // zoomfile="./data/attachment/forum/201803/04/225623iycsyb3petgs9z7j.jpg"
                string picUrl ="https://youiv.tv"+ m.Value.Replace("zoomfile=\".", "").Replace("\"","");
                his.Html = "<img src=\"" + picUrl + "\"/><br>";
            }
            if (content.Contains("filter=typeid&amp;typeid=432\">[U-15写真]</a>"))
            {
                Tool.MoveFile("U-15", path);
                his.IsBlack = true;
                resList.Add(his);
                return resList;
            }
            his.HisTimeSpan = 999;
            his.IsCHeckHisSize = ifCheckHis;
            his.IsCheckSize = false;
            his.Name = path.Split(new char[] { ']', '.' })[1];
            resList.Add(his);
            return resList;
        }
    }
}
