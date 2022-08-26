using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace MyWork
{
    public class SqlHelper
    {
        public static string sqlcon;
        public static DataTable ExcuteDataTable(string sql, params SqlParameter[] param)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(sqlcon))
            {
                conn.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(sql, conn))
                {
                    sda.SelectCommand.Parameters.AddRange(param);
                    sda.Fill(dt);
                }
            }
            return dt;
        }
        public static int ExcuteNoquery(string sql, params SqlParameter[] param)
        {
            int n = -1;
            using (SqlConnection conn = new SqlConnection(sqlcon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(param);
                    n = cmd.ExecuteNonQuery();
                }
            }
            return n;
        }
        public static object ExcuteScalar(string sql, params SqlParameter[] param)
        {
            object obj = null;
            using (SqlConnection conn = new SqlConnection(sqlcon))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddRange(param);
                    obj = cmd.ExecuteScalar();
                }
            }
            return obj;
        }
    }
}
