using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public static class Helper
    {
        public static T GetCell<T>(DataGridViewRow row, string columnName, T @default = default(T))
        {
            var cell = row.Cells[columnName];
            if (cell == null) return @default;

            var v = cell.Value;
            if (v == null || v == DBNull.Value) return @default;

            try
            {
                var t = typeof(T);
                var u = Nullable.GetUnderlyingType(t) ?? t;

                if (u == typeof(string)) return (T)(object)Convert.ToString(v);
                if (u.IsEnum)
                {
                    if (v is string s) return (T)Enum.Parse(u, s, true);
                    return (T)Enum.ToObject(u, Convert.ChangeType(v, Enum.GetUnderlyingType(u)));
                }
                if (u == typeof(DateTime))
                {
                    if (v is DateTime dt) return (T)(object)dt;
                    DateTime parsed;
                    if (DateTime.TryParse(Convert.ToString(v), out parsed)) return (T)(object)parsed;
                    return @default;
                }
                if (u == typeof(decimal)) return (T)(object)Convert.ToDecimal(v);
                if (u == typeof(double)) return (T)(object)Convert.ToDouble(v);
                if (u == typeof(float)) return (T)(object)Convert.ToSingle(v);
                if (u == typeof(int)) return (T)(object)Convert.ToInt32(v);
                if (u == typeof(long)) return (T)(object)Convert.ToInt64(v);
                if (u == typeof(bool)) return (T)(object)Convert.ToBoolean(v);

                return (T)Convert.ChangeType(v, u);
            }
            catch
            {
                return @default;
            }
        }

    }
}
