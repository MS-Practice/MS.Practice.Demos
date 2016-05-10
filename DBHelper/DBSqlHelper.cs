using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace DBHelper
{
    public static class DBSqlHelper
    {
        const string connectionString = "Data Source=MS-PC;Initial Catalog=person;Persist Security Info=True;User ID=sa;Password=123456";
        public static void usually_InsertData()
        {
            Stopwatch sw = new Stopwatch();
            SqlConnection sqlconn = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand();
            command.CommandText = string.Format("insert into person (Id,UserName,Pwd) values (@p0,@p1,@p2)");
            command.Parameters.Add("@p0", System.Data.SqlDbType.Int);
            command.Parameters.Add("@p1", System.Data.SqlDbType.NVarChar);
            command.Parameters.Add("@p2", System.Data.SqlDbType.VarChar);
            command.CommandType = System.Data.CommandType.Text;
            command.Connection = sqlconn;
            sqlconn.Open();
            try
            {
                //循环插入10W条数据
                for (var multiply = 0; multiply < 10; multiply++)
                {
                    for (int count = multiply * 10000; count < (multiply + 1) * 10000; count++)
                    {
                        command.Parameters["@p0"].Value = count;
                        command.Parameters["@p1"].Value = string.Format("ms{0}", count * multiply);
                        command.Parameters["@p2"].Value = string.Format("password{0}", count * multiply);
                        sw.Start();
                        command.ExecuteNonQuery();
                        sw.Stop();
                    }
                    Console.WriteLine(string.Format("Elapsed Time is {0} Milliseconds"), sw.ElapsedMilliseconds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlconn.Close();
            }
        }
        /// <summary>
        /// 使用Bulk批量插入
        /// </summary>
        /// <param name="dt"></param>
        public static void InserDataByBulkToDB(DataTable dt)
        {
            SqlConnection sqlConn = new SqlConnection(connectionString);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn);
            bulkCopy.DestinationTableName = "BulkTestTable";
            bulkCopy.BatchSize = dt.Rows.Count;

            try
            {
                sqlConn.Open();
                if (dt != null && dt.Rows.Count != 0)
                    bulkCopy.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
        }
        public static DataTable GetTableSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]{  
            new DataColumn("Id",typeof(int)),  
            new DataColumn("UserName",typeof(string)),  
            new DataColumn("UserPassword",typeof(string))});
            return dt;
        }
        /// <summary>
        /// 表值参数批量插入
        /// </summary>
        /// <param name="dt"></param>
        public static void TableValueToDB(DataTable dt) {
            SqlConnection sqlConn = new SqlConnection(connectionString);
            const string TSqlStatement =
             "insert into BulkTestTable (Id,UserName,UserPassword)" +
             " SELECT nc.Id, nc.UserName,nc.Pwd" +
             " FROM @NewBulkTestTvp AS nc";
            SqlCommand cmd = new SqlCommand(TSqlStatement, sqlConn);
            SqlParameter catParam = cmd.Parameters.AddWithValue("@NewBulkTestTvp", dt);
            catParam.SqlDbType = SqlDbType.Structured;
            //表值参数的名字叫BulkUdt，在上面的建立测试环境的SQL中有。  
            catParam.TypeName = "dbo.BulkUdt";
            try
            {
                sqlConn.Open();
                if (dt != null && dt.Rows.Count != 0)
                {
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sqlConn.Close();
            }  
        }

        static void Main(string[] args) {
            Stopwatch sw = new Stopwatch();
            for (int multiply = 0; multiply < 10; multiply++) {
                DataTable dt = GetTableSchema();
                for (int count = multiply * 10000; count < (multiply + 1) * 10000; count++)
                {
                    DataRow r = dt.NewRow();
                    r[0] = count;
                    r[1] = string.Format("User-{0}", count * multiply);
                    r[2] = string.Format("Pwd-{0}", count * multiply);
                    dt.Rows.Add(r);
                }
                sw.Start();
                InserDataByBulkToDB(dt);
                sw.Stop();
                Console.WriteLine(string.Format("Elapsed Time is {0} Milliseconds", sw.ElapsedMilliseconds));
            }
        }
    }
}
