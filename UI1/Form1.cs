using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MODEL;
using BLL;
using DAL;
using DB;
using System.IO;
using System.Text.RegularExpressions;
using Framework;

namespace UI1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<MyFileInfo> list = new List<MyFileInfo>();

        FileBLL fb = new FileBLL();
        public void refresh()
        {
            list = fb.getFileList();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //refresh();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            MyFileInfo myFileInfo = new MyFileInfo();
            try
            {
                
                int row = e.RowIndex;

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            if (FileDAL.Update(myFileInfo) > 0)
                //MessageBox.Show("Seccess");
                ;
            else
                MessageBox.Show("Fail");
        }

        private void Insert_Click(object sender, EventArgs e)
        {
            DoConfig();
            fb.process(textBox1.Text.Replace("\\", "\\\\"),new Sis001Analysis(),checkBox1.Checked);

        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (OpenFolderDialog openFolderDlg = new OpenFolderDialog())
            {
                if (openFolderDlg.ShowDialog() == DialogResult.OK)
                {
                    this.textBox1.Text = openFolderDlg.Path;
                    Console.WriteLine(this.textBox1.Text.Replace("\\","\\\\"));
                }
            }
        }
        string dataClicked="" ;
        bool flag = false;
        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            List<MyFileInfo> sortList = new List<MyFileInfo>();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            DBHelper.connstr = this.textBox3.Text;
            refresh();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DoConfig();
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new JavBusAnalysis(), checkBox1.Checked);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new AkibaOnlineAnalysis(), checkBox1.Checked);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new ThzAnalysis(), checkBox1.Checked);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            DoConfig();
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new JavTorrentsAnalysis(), checkBox1.Checked);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new YouivAnalysis(), checkBox1.Checked);
            //addKiki();
        }

        MegFinder megFinder;
        void addKiki()
        {
            megFinder = new MegFinder();
            megFinder.addKiki(Path.Combine(textBox1.Text.Replace("\\", "\\\\"), "result.htm"),webBrowser1);
        }
        Kiki kiki;
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
            kiki.NavigateHandle(webBrowser1, e, textBox1.Text.Replace("\\", "\\\\"));
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            kiki = new Kiki();
            Regex regex = new Regex("<a href=\" http://kikibt.ws/search/.*/>");
            StreamReader sr = new StreamReader(Path.Combine(textBox1.Text.Replace("\\", "\\\\"), "result.htm"));
            string content = sr.ReadToEnd();
            string[] htmlStrs= content.Split(new string[] { Tool.splitter },StringSplitOptions.RemoveEmptyEntries);
            foreach(string html in htmlStrs)
            {
                if (html == "\r\n ")
                    continue;
                string url= regex.Match(html).Value.Trim().Replace("<a href=\"","").Replace("\"/>","").Trim();
                if(String.IsNullOrEmpty(url))
                {
                    kiki.Empty += html + Tool.splitter+"\n";
                    continue;
                }
                KikiDO kikiDO = new KikiDO();
                kikiDO.Url = url;
                kikiDO.Html = html;
                kiki.BlockingQueue.Enqueue(kikiDO);
                kiki.DictionarySearch.Add(url, kikiDO);

            }
            KikiDO kikiDO1 = kiki.BlockingQueue.Peek();
            webBrowser1.Navigate(kikiDO1.Url);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Config.isCheck168xC = checkBox3.Checked;
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new _168x(), checkBox1.Checked);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new BailuAnalysis(), checkBox1.Checked);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Config.isCheckSize = this.checkBox2.Checked;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            DoConfig();
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new _52iv(), checkBox1.Checked);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            
                fb.process(textBox1.Text.Replace("\\", "\\\\"), new TorrentAnalysis(), checkBox1.Checked);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Config.isXieZhen = true;
            Config.isKiki = false;
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new _168xXieZhen(), checkBox1.Checked);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Config.isCheck168xC = checkBox3.Checked;
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new _456kAnalysis(), checkBox1.Checked);

        }

        private void button13_Click(object sender, EventArgs e)
        {
            DoConfig();
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new JavDBAnalysis(), checkBox1.Checked);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DoConfig();
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new Hdd600(), checkBox1.Checked);
        }



        //private void SortRows(DataGridViewColumn sortColumn, bool orderToggle)
        //{
        //    if (sortColumn == null)
        //        return;

        //    //清除前面的排序
        //    if (sortColumn.SortMode == DataGridViewColumnSortMode.Programmatic &&
        //        dataGridView1.SortedColumn != null &&
        //        !dataGridView1.SortedColumn.Equals(sortColumn))
        //    {
        //        dataGridView1.SortedColumn.HeaderCell.SortGlyphDirection =
        //            SortOrder.None;
        //    }

        //    //设定排序的方向（升序、降序）
        //    ListSortDirection sortDirection;
        //    if (orderToggle)
        //    {
        //        sortDirection =
        //            dataGridView1.SortOrder == SortOrder.Descending ?
        //            ListSortDirection.Ascending : ListSortDirection.Descending;
        //    }
        //    else
        //    {
        //        sortDirection =
        //            dataGridView1.SortOrder == SortOrder.Descending ?
        //            ListSortDirection.Descending : ListSortDirection.Ascending;
        //    }
        //    SortOrder sortOrder =
        //        sortDirection == ListSortDirection.Ascending ?
        //        SortOrder.Ascending : SortOrder.Descending;

        //    //进行排序
        //    dataGridView1.Sort(sortColumn, sortDirection);

        //    if (sortColumn.SortMode == DataGridViewColumnSortMode.Programmatic)
        //    {
        //        //变更排序图标
        //        sortColumn.HeaderCell.SortGlyphDirection = sortOrder;
        //    }
        //}

        private void DoConfig()
        {
            Config.timeSpan =Convert.ToInt32( textBox2.Text);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Config.isCheck168xC = checkBox3.Checked;
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new Bo99Analysis(), checkBox1.Checked);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            fb.process(textBox1.Text.Replace("\\", "\\\\"), new Akiba(), checkBox1.Checked);
        }
    }
}
