using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using MODEL;
using BLL;
namespace DB
{
    public class DBHelper
    {
        static string searchHisSql = "select * from his1 where LOWER(vid)=LOWER('{0}') and size*1.7>{1} and DATEDIFF(M,createtime,GETDATE())<{2}";
        static string searchHisSqlWithoutSize = "select * from his1 where LOWER(vid)=LOWER('{0}') and DATEDIFF(M,createtime,GETDATE())<{1}";
        static string insertHisSql = "insert into his1 values('{0}',{1},'{2}',{3},'{4}',getdate())";
        public static string connstr = @"server=localhost;uid=sa;pwd=iamjack'scolon;database=cd";
        //static string connstr = "server=MICROSOF-8335F8\\SQLEXPRESS;uid=sa;pwd=a;database=cd";
        public static SqlConnection conn = new SqlConnection(connstr);
        //static string connstr = "server=.;uid=sa;pwd=a;database=cd";
        public static int ExecuteSql(string sql)
        {
            int i = 0;
            //using (SqlConnection conn = new SqlConnection(connstr))
            // {
            Console.WriteLine(sql);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            i = cmd.ExecuteNonQuery();
            conn.Close();

            //conn.Dispose();
            //}

            return i;
        }

        public static SqlDataReader SearchSql(string sql)
        {
            conn = new SqlConnection(connstr);
            conn.Open();
            SqlDataReader sda;
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                 sda = cmd.ExecuteReader();
            }

            return sda;
        }

        public static int ExecuteInsert_SP(string spName, MyFileInfo fi)
        {
            // using (SqlConnection conn = new SqlConnection(connstr))
            //{
            SqlCommand objCommand = new SqlCommand(spName, conn);
            objCommand.CommandType = CommandType.StoredProcedure;
            objCommand.Parameters.Add("@fileName", SqlDbType.VarChar, 300).Value = fi.FileName;
            objCommand.Parameters.Add("@extension", SqlDbType.VarChar, 50).Value = fi.Extension;
            objCommand.Parameters.Add("@directoryName", SqlDbType.VarChar, 500).Value = fi.DirectoryName;
            objCommand.Parameters.Add("@directory", SqlDbType.VarChar, 500).Value = fi.Directory;
            objCommand.Parameters.Add("@length", SqlDbType.Float).Value = fi.Length;
            objCommand.Parameters.Add("@lastAccessTime", SqlDbType.VarChar, 50).Value = fi.LastAccessTime;
            objCommand.Parameters.Add("@lastWriteTime", SqlDbType.VarChar, 50).Value = fi.LastWriteTime;
            objCommand.Parameters.Add("@mark", SqlDbType.Text).Value = fi.Mark;
            conn.Open();
            int i = objCommand.ExecuteNonQuery();
            conn.Close();

            //conn.Dispose();
            return i;

            //}
        }

        public static int searchHis(His his)
        {
            int res=0;
            string sql = "";

            if (his.IsCHeckHisSize && his.Size > 0)
            {

                sql = string.Format(searchHisSql, his.Vid, his.Size, his.HisTimeSpan);
            }
            else
            {
                sql = string.Format(searchHisSqlWithoutSize, his.Vid, his.HisTimeSpan);
            }

            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand sc = new SqlCommand(sql, conn);
                try
                {
                    res = Convert.ToInt32(sc.ExecuteScalar());
                } 
                catch(Exception e)
                {
                    searchHis(his);
                }
            }
            return res;
        }

        public static void insertHis(His his)
        {
            string sql="";
            try
            {


                if (his.Actress.Length > 250)
                    his.Actress = his.Actress.Substring(0, 248);
                sql = string.Format(insertHisSql, his.Vid, his.Size, his.Actress.Replace("'", "''"), his.FileCount, his.Files.Replace("'", "''"));
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlCommand sc = new SqlCommand(sql, conn);
                    sc.CommandTimeout = 120000;
                    sc.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(sql);
                Console.WriteLine(e.ToString());
            }

        }

        public static void insertBLackList(His his)
        {
            string insert = "INSERT INTO [dbo].[blackList] ([vid],[size],[actress],[fileCount],[files],[createtime]) VALUES('{0}',{1},'{2}',{3},'{4}',getdate())";
            string sql= string.Format(insert, his.Vid, his.Size, his.Actress, his.FileCount, his.Files);
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand sc = new SqlCommand(sql, conn);
                sc.CommandTimeout = 120000;
                sc.ExecuteNonQuery();
            }
        }

        public static int getBlackListHis(His his)
        {
            string sql = "select count(*) from blackList where vid='" + his.Vid+"'";
            int res;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand sc = new SqlCommand(sql, conn);
                res = Convert.ToInt32(sc.ExecuteScalar());
            }
            return res;
        }

        public static SqlDataReader EuroAndAmericaList()
        {
            string sql = "select * from files where length>500 and len(fileName)>20 and fileName not like  '%-%'";
            
                conn.Open();
                SqlCommand sc = new SqlCommand(sql, conn);

                return sc.ExecuteReader();
             
                    
            

        }

        public static void UpdateType(int fileId)
        {
            string sql = "update files set type =1 where fileId=" + fileId;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand sc = new SqlCommand(sql, conn);
                sc.ExecuteNonQuery();
            }

        }


        public static void UpdateTypeBatch(string fileId)
        {
            string sql = "update files set type =1 where fileId in(  " + fileId+")";
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand sc = new SqlCommand(sql, conn);
                sc.ExecuteNonQuery();
            }

        }

        public static int Check168xC(string vid)
        {
            //string sql = "SELECT count(*) FROM [cd].[dbo].[files] where (directoryName like '%" + vid + "%-C' or directoryName like '%\\"+vid+"%-C\\%') and length>1000";

            string sql = "SELECT count(*) FROM [cd].[dbo].[files] where (directoryName like '%\\" + vid + "%-C' or directoryName like '%\\" + vid + "%-C\\%' or fileName like '%@" + vid+"%-C_%') and length>1000";
            SqlDataReader sdr = DBHelper.SearchSql(sql);
            sdr.Read();
            int count = (int)sdr[0];
            sdr.Close();
            sdr.Dispose();
            DBHelper.conn.Close();
            return count;
        }


        public static int CheckXieZhen(string id)
        {
            string sql = "select count(*) from files where fileName='" + id + "'";
            SqlDataReader sdr = DBHelper.SearchSql(sql);
            sdr.Read();
            return (int)sdr[0];
        }

    }
}
