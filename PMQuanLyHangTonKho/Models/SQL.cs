using PMQuanLyHangTonKho.Lib;
using PMQuanLyHangTonKho.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            return "Server=localhost,1433;Database=SQLQuanLyHangTonKho;User ID=sa;Password=Sqlserver123@;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
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
            catch (Exception ex)
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

        #region run query for all commands
        /// <summary>
        /// Thực thi câu lệnh SQL (INSERT/UPDATE/DELETE) với tham số dạng mảng 2 chiều:
        /// new object[,] { {\"@Id\", id}, {\"@Name\", name}, ... }
        /// Trả về số dòng bị ảnh hưởng.
        /// </summary>
        public static int RunQuery(string sql, object[,] parameters = null, int? commandTimeoutSeconds = null)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL rỗng.", nameof(sql));

            using (var conn = new SqlConnection(Instance.GetConnectionString()))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                using (var cmd = new SqlCommand(sql, conn, tran))
                {
                    if (commandTimeoutSeconds.HasValue)
                        cmd.CommandTimeout = commandTimeoutSeconds.Value;

                    if (parameters != null)
                        AddParameters(cmd, parameters);

                    try
                    {
                        int affected = cmd.ExecuteNonQuery();
                        tran.Commit();
                        return affected;
                    }
                    catch
                    {
                        try { tran.Rollback(); } catch { }
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// GetValue có tham số – trả về string ("" nếu null).
        /// Dùng object[,] như: new object[,] { {"@Id", id}, {"@Type","P"} }
        /// </summary>
        public static string GetValue(string sql, object[,] parameters, int? commandTimeoutSeconds = null)
        {
            var obj = ExecuteScalar(sql, parameters, commandTimeoutSeconds);
            return obj == null || obj == DBNull.Value
                ? string.Empty
                : Convert.ToString(obj, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// GetValue generic – trả về đúng kiểu T (default(T) nếu null/không chuyển được).
        /// Ví dụ:
        ///   var total = SQL.GetValue<decimal>("SELECT SUM(Amount) FROM ... WHERE Id=@Id",
        ///                                     new object[,] {{"@Id", id}}, defaultValue: 0m);
        /// </summary>
        public static T GetValue<T>(string sql, object[,] parameters = null,
                                    int? commandTimeoutSeconds = null, T defaultValue = default)
        {
            var obj = ExecuteScalar(sql, parameters, commandTimeoutSeconds);
            if (obj == null || obj == DBNull.Value) return defaultValue;

            try
            {
                var t = typeof(T);
                var u = Nullable.GetUnderlyingType(t) ?? t; // hỗ trợ Nullable<T>
                return (T)Convert.ChangeType(obj, u, CultureInfo.InvariantCulture);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Thực thi scalar (nội bộ).
        /// </summary>
        private static object ExecuteScalar(string sql, object[,] parameters, int? commandTimeoutSeconds)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentException("SQL rỗng.", nameof(sql));

            using (var conn = new SqlConnection(Instance.GetConnectionString()))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (commandTimeoutSeconds.HasValue)
                    cmd.CommandTimeout = commandTimeoutSeconds.Value;

                if (parameters != null)
                    AddParameters(cmd, parameters);

                conn.Open();
                return cmd.ExecuteScalar();
            }
        }


        private static void AddParameters(SqlCommand cmd, object[,] pairs)
        {
            int n = pairs.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                var nameObj = pairs[i, 0];
                if (nameObj == null) continue;

                string name = nameObj.ToString();
                if (!name.StartsWith("@")) name = "@" + name;

                object value = pairs[i, 1];
                if (value == null) value = DBNull.Value;

                if (value is DateTime dt && (dt == DateTime.MinValue || dt == DateTime.MaxValue))
                    value = DBNull.Value;

                cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
            }

        }
        #endregion
        #region handle for select query
        // Cache metadata cho mỗi kiểu T
        private static readonly Dictionary<Type, List<PropertyMap>> _mapCache =
            new Dictionary<Type, List<PropertyMap>>();

        private class PropertyMap
        {
            public PropertyInfo Prop;
            public string ColumnName; // tên cột để map
        }

        /// <summary>
        /// Query trả về List&lt;T&gt; với ADO.NET.
        /// - sql: câu lệnh có @param
        /// - parameters: Dictionary tênParam -> value (không cần @ ở key)
        /// - projector: nếu muốn tự map từng dòng -> T, truyền vào đây (ưu tiên projector nếu có)
        /// </summary>
        public static List<T> QueryList<T>(
            string sql,
            IDictionary<string, object> parameters = null,
            Func<IDataRecord, T> projector = null)
        {
            var result = new List<T>();

            using (var conn = new SqlConnection(Instance.GetConnectionString()))
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                {
                    foreach (var kv in parameters)
                    {
                        var name = kv.Key.StartsWith("@") ? kv.Key : "@" + kv.Key;
                        cmd.Parameters.AddWithValue(name, kv.Value ?? DBNull.Value);
                    }
                }

                conn.Open();
                using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (projector != null)
                    {
                        while (reader.Read())
                            result.Add(projector(reader));
                        return result;
                    }

                    // Map tự động theo tên cột
                    var mapper = GetOrBuildMapper<T>(reader);
                    while (reader.Read())
                        result.Add(mapper(reader));
                }
            }

            return result;
        }

        /// <summary>
        /// Tạo mapper cho T dựa trên schema từ IDataReader (dùng cache theo T).
        /// </summary>
        private static Func<IDataRecord, T> GetOrBuildMapper<T>(IDataRecord schemaSample)
        {
            var t = typeof(T);

            if (IsSimpleType(t))
            {
                return rec => (T)ConvertTo(rec.IsDBNull(0) ? null : rec.GetValue(0), t);
            }

            List<PropertyMap> maps;
            if (!_mapCache.TryGetValue(t, out maps))
            {
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.CanWrite).ToList();

                maps = new List<PropertyMap>(props.Count);
                foreach (var p in props)
                {
                    var colAttr = (ColumnAttribute)Attribute.GetCustomAttribute(p, typeof(ColumnAttribute));
                    var columnName = colAttr != null && !string.IsNullOrEmpty(colAttr.Name)
                                     ? colAttr.Name
                                     : p.Name;

                    maps.Add(new PropertyMap { Prop = p, ColumnName = columnName });
                }
                _mapCache[t] = maps;
            }

            // Tạo bảng tra cứu columnName -> ordinal (case-insensitive)
            var ordinals = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < schemaSample.FieldCount; i++)
            {
                var name = schemaSample.GetName(i);
                if (!ordinals.ContainsKey(name)) ordinals.Add(name, i);
            }

            return rec =>
            {
                var obj = (T)Activator.CreateInstance(t);
                foreach (var m in maps)
                {
                    int ordinal;
                    if (!ordinals.TryGetValue(m.ColumnName, out ordinal))
                        continue; // không có cột tương ứng -> bỏ qua

                    if (rec.IsDBNull(ordinal))
                    {
                        m.Prop.SetValue(obj, null, null);
                        continue;
                    }

                    var raw = rec.GetValue(ordinal);
                    var converted = ConvertTo(raw, m.Prop.PropertyType);
                    m.Prop.SetValue(obj, converted, null);
                }
                return obj;
            };
        }

        private static bool IsSimpleType(Type type)
        {
            var u = Nullable.GetUnderlyingType(type) ?? type;
            return u.IsPrimitive
                   || u.IsEnum
                   || u == typeof(string)
                   || u == typeof(decimal)
                   || u == typeof(DateTime)
                   || u == typeof(Guid)
                   || u == typeof(TimeSpan);
        }

        private static object ConvertTo(object value, Type targetType)
        {
            if (value == null || value == DBNull.Value) return null;

            var t = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Enum: chấp nhận số hoặc chuỗi
            if (t.IsEnum)
            {
                if (value is string s) return Enum.Parse(t, s, true);
                return Enum.ToObject(t, System.Convert.ChangeType(value, Enum.GetUnderlyingType(t), CultureInfo.InvariantCulture));
            }

            if (t == typeof(Guid))
            {
                if (value is Guid g) return g;
                return new Guid(value.ToString());
            }

            if (t == typeof(TimeSpan))
            {
                if (value is TimeSpan ts) return ts;
                return TimeSpan.Parse(value.ToString(), CultureInfo.InvariantCulture);
            }

            // DateTime
            if (t == typeof(DateTime))
            {
                if (value is DateTime dt) return dt;
                return DateTime.Parse(value.ToString(), CultureInfo.InvariantCulture);
            }

            // Decimal/Number: dùng InvariantCulture
            if (t == typeof(decimal))
                return Convert.ToDecimal(value, CultureInfo.InvariantCulture);
            if (t == typeof(double))
                return Convert.ToDouble(value, CultureInfo.InvariantCulture);
            if (t == typeof(float))
                return Convert.ToSingle(value, CultureInfo.InvariantCulture);

            // Chuẩn chung
            return System.Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
        }
        #endregion
    }
}
