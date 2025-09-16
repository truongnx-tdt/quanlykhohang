using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Models
{
    public class SQL
    {
        public static string Data = "SQLQuanLyHangTonKho";
        public static string ServerName = @"HI";

        private static SQL instance;
        public static SQL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQL();
                }
                return instance;
            }
        }

        private SQL() { }

        public string GetConnectionString()
        {
            //SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder()
            //{
            //    InitialCatalog = Data,
            //    DataSource = ServerName,
            //    IntegratedSecurity = true,
            //    MultipleActiveResultSets = true,
            //    ConnectTimeout = 0,
            //    MaxPoolSize = 500,
            //    TrustServerCertificate = true
            //};
            //return stringBuilder.ConnectionString;
            return "Server=localhost,1433;Database=SQLQuanLyHangTonKho;User ID=sa;Password=SQlserver123@;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        }
        public static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            string conn_string = Instance.GetConnectionString();

            using (SqlConnection sqlConnect = new SqlConnection(conn_string))
            {
                sqlConnect.Open();
                SqlCommand cmd = new SqlCommand(sql, sqlConnect);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public static bool FindExists(string sql)
        {
            Boolean isCheck = false;
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                isCheck = true;
            }
            return isCheck;
        }
        public static string GetValue(string sql)
        {
            string value = "";
            DataTable dt = new DataTable();
            dt = GetData(sql);
            if (dt.Rows.Count > 0)
            {
                value = dt.Rows[0][0].ToString();
            }
            return value;
        }
        public static bool RunQuery(string sql)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(Instance.GetConnectionString())) // Sử dụng Instance để kết nối
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception)
            {
                Alert.Error("Dữ liệu đã phát sinh, không được xóa");
                return false;
            }
        }
        public static bool InsertTable(string table, string[] key, string[] value)
        {
            string keys = string.Join(", ", key);
            string values = string.Join(", ", value.Select(v => $"'{v}'"));
            string strInsert = $"INSERT INTO {table} ({keys}) VALUES ({values})";
            try
            {
                RunQuery(strInsert);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return false;
            }
        }
        public static bool UpdateTable(string table, string[] key, string[] value, string condition)
        {
            List<string> setClauses = new List<string>();
            for (int i = 0; i < key.Length; i++)
            {
                setClauses.Add($"{key[i]} = '{value[i]}'");
            }
            string setStatement = string.Join(", ", setClauses);
            string strUpdate = $"UPDATE {table} SET {setStatement} WHERE {condition}";
            try
            {
                RunQuery(strUpdate);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return false;
            }
        }
    }
}
