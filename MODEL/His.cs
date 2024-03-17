using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class His:IComparable
    {
        string id="";

        bool is168xC;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        string vid="";

        public string Vid
        {
            get { return vid; }
            set { vid = value; }
        }
        double size=0;

        public double Size
        {
            get { return size; }
            set { size = value; }
        }
        string actress="";

        public string Actress
        {
            get { return actress; }
            set { actress = value; }
        }
        int fileCount=0;

        public int FileCount
        {
            get { return fileCount; }
            set { fileCount = value; }
        }
        string files="";

        public string Files
        {
            get { return files; }
            set { files = value; }
        }

        string html="";

        public string Html
        {
            get { return html; }
            set { html = value; }
        }

        string originalHtml="";

        public string OriginalHtml
        {
            get { return originalHtml; }
            set { originalHtml = value; }
        }

        string info="";

        public string Info
        {
            get { return info; }
            set { info = value; }
        }

        int hisTimeSpan=6;

        public int HisTimeSpan
        {
            get { return hisTimeSpan; }
            set { hisTimeSpan = value; }
        }

        public int CompareTo(object obj)
        {
            return this.vid.CompareTo(((His)obj).vid);
        }

        string htmPath;

        public string HtmPath
        {
            get { return htmPath; }
            set { htmPath = value; }
        }

        string torrentPath;

        public string TorrentPath
        {
            get { return torrentPath; }
            set { torrentPath = value; }
        }
        private string failReason = "";

        public string FailReason
        {
            get { return failReason; }
            set { failReason = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool IsBlack { get => isBlack; set => isBlack = value; }
        public bool IfExistSmaller { get => ifExistSmaller; set => ifExistSmaller = value; }
        public bool Is168xC { get => is168xC; set => is168xC = value; }
        public string SortBy { get => sortBy; set => sortBy = value; }

        private Boolean isBlack;

        private bool ifExistSmaller;

        string sortBy;

        


    }
}
