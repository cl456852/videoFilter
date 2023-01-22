using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL
{
    public class Akiba : BaseAnalysis
    {
        Regex idRegex = new Regex("[A-Z]{1,}-[0-9]{1,} ");
        public override ArrayList alys(string content, string path, string vid, bool ifCheckHis)
        {
            ArrayList alys = new ArrayList();
            string[] strs = content.Split( new string[] { "<article class=\"message    message-threadStarterPost message--post" }, StringSplitOptions.None);
            for(int i=1;i<strs.Length;i++)
            {
                His his = new His();
                string str = strs[i];
                Match m= idRegex.Match(str);
                his.Vid = m.Value;
                his.Html = str;
                alys.Add(his);

            }
            return alys;

        }
    }
}
