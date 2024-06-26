using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using MODEL;
using BLL;
using Framework;

namespace DB
{
    public class DBHelper
    {
        
        static string searchHisSql = "select count(*) from his1 where vid='{0}' and size>{1} and createtime>'{2}'";
        static string searchHisSqlWithoutSize = "select count(*) from his1 where vid='{0}' and createtime>'{1}'";
        static string insertHisSql = "insert into his1 values('{0}',{1},'{2}',{3},'{4}',getdate())";
        public static string connstr = @"server=localhost;uid=sa;pwd=iamjack'scolon;database=cd;Pooling=true;Min Pool Size=0;Max Pool Size=100;Connection Timeout=60;MultipleActiveResultSets=true;";
        //static string connstr = "server=MICROSOF-8335F8\\SQLEXPRESS;uid=sa;pwd=a;database=cd";
        public static SqlConnection conn = new SqlConnection(connstr);
        private static readonly object _lock = new object();
        public static void OpenConnection()
        {
            if (Monitor.TryEnter(_lock))
            {

                if (conn.State == ConnectionState.Closed)
                {
                    Console.WriteLine("trylock success, open connection");
                    conn.Open();
                }
            }else
            {
                Console.WriteLine("trylock failed");
                Thread.Sleep(1000);
            }
        }
        public static int ExecuteSql(string sql)
        {
            int i = 0;

            Console.WriteLine(sql);
            SqlCommand cmd = new SqlCommand(sql, conn);
            i = cmd.ExecuteNonQuery();


            return i;
        }

        public static SqlDataReader SearchSql(string sql)
        {
            SqlDataReader sda;
            while(conn.State==ConnectionState.Closed)
                OpenConnection();
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                 sda = cmd.ExecuteReader();
            }

            return sda;
        }

        public static int ExecuteInsert_SP(string spName, MyFileInfo fi)
        {

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
            int i = objCommand.ExecuteNonQuery();


            return i;
        }

        public static int searchHis(His his)
        {
            int res=0;
            string sql = "";
            if (Config.ifCheckHisSize && his.Size > 0)
            {

                sql = string.Format(searchHisSql, his.Vid.ToUpper(), his.Size/1.7, DateTime.Now.AddMonths(-his.HisTimeSpan));
            }
            else
            {
                sql = string.Format(searchHisSqlWithoutSize, his.Vid.ToUpper(), DateTime.Now.AddMonths(-his.HisTimeSpan));
            }


            SqlCommand sc = new SqlCommand(sql, conn);
            try
            {
                if(conn.State==ConnectionState.Closed)
                    OpenConnection();
                res = Convert.ToInt32(sc.ExecuteScalar());
            } 
            catch(Exception e)
            {
                Console.WriteLine(sql);
                Console.WriteLine(e.ToString());
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

                if(conn.State==ConnectionState.Closed)
                    OpenConnection();
                SqlCommand sc = new SqlCommand(sql, conn);
                sc.CommandTimeout = 120000;
                sc.ExecuteNonQuery();
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

            SqlCommand sc = new SqlCommand(sql, conn);
            sc.CommandTimeout = 120000;
            sc.ExecuteNonQuery();

        }

        public static int getBlackListHis(His his)
        {
            string sql = "select count(*) from blackList where vid='" + his.Vid+"'";
            int res;

            SqlCommand sc = new SqlCommand(sql, conn);
            res = Convert.ToInt32(sc.ExecuteScalar());

            return res;
        }

        public static SqlDataReader EuroAndAmericaList()
        {
            string sql = "select * from files where length>500 and len(fileName)>20 and fileName not like  '%-%'";
            

                SqlCommand sc = new SqlCommand(sql, conn);

                return sc.ExecuteReader();
             
                    
            

        }

        public static void UpdateType(int fileId)
        {
            string sql = "update files set type =1 where fileId=" + fileId;

            SqlCommand sc = new SqlCommand(sql, conn);
            sc.ExecuteNonQuery();


        }


        public static void UpdateTypeBatch(string fileId)
        {
            string sql = "update files set type =1 where fileId in(  " + fileId+")";

            SqlCommand sc = new SqlCommand(sql, conn);
            sc.ExecuteNonQuery();
        

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
