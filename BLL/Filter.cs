using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEL;
using DAL;
using System.IO;
using DB;
using System.Text.RegularExpressions;


namespace BLL
{
    public class Filter
    {
        List<MyFileInfo> list;
        Regex reg1 = new Regex("[a-z]");
        Regex reg2 = new Regex("[a-z].*");
        Regex fiterListRegex = new Regex(@"[a-zA-Z]+[-_]?(\s){0,3}[0-9]+");
        public Filter()
        {

            list = getFileList();
            for (int i = list.Count - 1; i >= 0; i--)
            {
                string fileName = Path.GetFileNameWithoutExtension(list[i].FileName);
                if (!fiterListRegex.IsMatch(fileName) && !fiterListRegex.IsMatch(list[i].Directory))
                    list.Remove(list[i]);
            }
        }

        public bool CheckBlackList(His his)
        {
            return DBHelper.getBlackListHis(his)>0;
        }

        public bool checkValid(His his)
        {
            if (!String.IsNullOrEmpty(his.FailReason))
                return false;
            string id = his.Vid;
            if (id == null || id == "")
            {
                return true;
            }
            id = id.ToLower();
            bool flag = true;
            if (Tool.IsNum(id))
            {
                foreach (MyFileInfo info in list)
                {
                    if (info.FileName.Contains(id) || info.Directory.Contains(id))
                    {
                        flag = false;
                        break;
                    }
                }

            }
            else
            {
                id = reg2.Match(id).Value;
                string letter = "";
                string number = "";
                bool isEndofLetter = false;
                for (int i = 0; i < id.Length; i++)                        //修改   对于出现KIDM235A  KIDM235B
                    if (reg1.IsMatch(id[i].ToString()))
                    {
                        if (isEndofLetter)
                            break;
                        else
                            letter += id[i];
                    }
                    else
                    {
                        number += id[i];
                        isEndofLetter = true;
                    }

                string[] searchStr = { letter, number };


                Regex r = new Regex("[^a-z]" + searchStr[0] + @"(\s){0,3}[-_]?(\s){0,3}(0){0,3}" + searchStr[1] + "[^0-9]|^" + searchStr[0] + @"(\s){0,3}[-_]?(\s){0,3}(0){0,3}" + searchStr[1] + "$|[^a-z]" + searchStr[0] + @"(\s){0,3}[-_]?(\s){0,3}(0){0,3}" + searchStr[1] + "$|^" + searchStr[0] + @"(\s){0,3}[-_]?(\s){0,3}(0){0,3}" + searchStr[1] + "[^0-9]");
                for (int i = 0; i < list.Count; i++)
                {

                    flag = true;

                    string fileName = Path.GetFileNameWithoutExtension(list[i].FileName.ToLower()).Replace("_","-");
                    string directoryName = list[i].Directory.ToLower();
                    string extension = list[i].Extension;
                    double len = list[i].Length;
                    if (his.IsCheckSize)
                    {
                        if ((len * 1.7 > his.Size ||
                             (extension.ToLower() == ".mds" || extension.ToLower() == ".iso") && his.Size < 3000) &&
                            (r.IsMatch(fileName) || r.IsMatch(directoryName)) && !fileName.Contains("incomplete"))
                        {
                            flag = false;
                            break;

                        }
                    }
                    else
                    {
                        if ((r.IsMatch(fileName) || r.IsMatch(directoryName)) && !fileName.Contains("incomplete"))
                        {
                            flag = false;
                            break;
                        }

                    }


                }
                if (!flag)
                {
                    his.FailReason = "file";
                }
            }

            if (flag)
            {
                flag = checkHis(his);
                if (!flag)
                    his.FailReason = "his";
            } 
          



            if (flag)
            {
                DBHelper.insertHis(his);
            }

            return flag;
        }

        bool checkHis(His his)
        {
            if (DBHelper.searchHis(his) > 0)
                return false;
            else
                return true;
        }

        public List<MyFileInfo> getFileList()
        {
            return FileDAL.selectMyFileInfo("");
        }
    }
}
