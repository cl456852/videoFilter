using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEL;
using DAL;
using System.IO;
using DB;
using System.Text.RegularExpressions;
using Framework;

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
        Regex numberRegex = new Regex("[0-9]*");
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
                if (id.StartsWith("mmr"))
                {
                    letter = "mmr";
                    number = id.Replace("mmr", "");
                }
                else if(id.StartsWith("mbr"))
                {
                    letter = "mbr";
                    number= id.Replace("mbr", "");
                }
                else if(id.StartsWith("mar"))
                {
                    letter = "mar";
                    number = id.Replace("mar", "");
                }
                else
                {
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
                    number = numberRegex.Match(number).Value;
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
                    if (Config.isCheckSize)
                    {
                        if ((r.IsMatch(fileName) || r.IsMatch(directoryName)) && !fileName.Contains("incomplete"))
                        {
                            his.IfExistSmaller = true;
                            if ((len * 2.5 > his.Size ||
                             (extension.ToLower() == ".mds" || extension.ToLower() == ".iso") && his.Size < 3000))
                            {
                                flag = false;
                                break;
                            }
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
                    if(Config.isCheck168xC)
                    {
                        if( DBHelper.Check168xC(letter +"-"+ number)>0)
                        {
                            return false;
                        }
                        his.FailReason = "";
                        his.Is168xC = true;
                        return true;
                    }
                }
            }

            if (flag)
            {
                //if (!his.IfExistSmaller)  //如果有小文件 说明要下载,
                //{                         //改为SMALLER也要CHECKHIS
                    flag = checkHis(his);
                    if (!flag)
                        his.FailReason = "his";
                //}
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

        public bool checkXieZhen(His his)
        {
            bool result;
            if (DBHelper.CheckXieZhen(his.Vid) > 0)
                result= false;
            else
            {
                if (DBHelper.searchHis(his) > 0)
                    result= false;
                else
                    result= true;
            }
            if(result)
            {
                DBHelper.insertHis(his);
            }
            return result;
        }
    }
}
