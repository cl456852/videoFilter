using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class KikiDO
    {
        string html;
        string searchUrl;
        string url;

        public string Html { get => html; set => html = value; }
 
        public string Url { get => url; set => url = value; }
        public string SearchUrl { get => searchUrl; set => searchUrl = value; }
    }
}
