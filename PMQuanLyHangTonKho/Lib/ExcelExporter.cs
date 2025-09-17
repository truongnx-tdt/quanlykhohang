// PMQuanLyHangTonKho/Lib/ExcelExporter.cs
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public static class ExcelExporter
    {
        /* =========================
         *  A) EXPORT TỪ DATAGRIDVIEW (giữ nguyên để tương thích cũ)
         * ========================= */

        public sealed class SheetGridSpec
        {
            public string SheetName;
            public DataGridView Grid;
            public SheetGridSpec(string sheetName, DataGridView grid)
            {
                SheetName = sheetName;
                Grid = grid;
            }
        }

        public static void ExportGrids(string suggestedFileName, IList<SheetGridSpec> sheets)
        {
            if (sheets == null || sheets.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = !string.IsNullOrWhiteSpace(suggestedFileName)
                               ? suggestedFileName
                               : string.Format("Export_{0:yyyyMMdd_HHmm}.xlsx", DateTime.Now);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        for (int i = 0; i < sheets.Count; i++)
                        {
                            var spec = sheets[i];
                            if (spec == null || spec.Grid == null) continue;

                            var dt = GridToDataTable(spec.Grid);
                            var ws = wb.Worksheets.Add(SafeSheetName(spec.SheetName));
                            var tableName = "T_" + Guid.NewGuid().ToString("N");
                            ws.Cell(1, 1).InsertTable(dt, tableName);

                            StyleWorksheet(ws, dt);
                        }

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static DataTable GridToDataTable(DataGridView grid)
        {
            var dt = new DataTable();

            var cols = new List<DataGridViewColumn>();
            foreach (DataGridViewColumn c in grid.Columns)
                if (c.Visible) cols.Add(c);
            cols = cols.OrderBy(c => c.DisplayIndex).ToList();

            for (int i = 0; i < cols.Count; i++)
            {
                var header = string.IsNullOrEmpty(cols[i].HeaderText) ? cols[i].Name : cols[i].HeaderText;
                dt.Columns.Add(header, typeof(object));
            }

            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                var values = new object[cols.Count];
                for (int i = 0; i < cols.Count; i++)
                {
                    values[i] = row.Cells[cols[i].Index].Value;
                }
                dt.Rows.Add(values);
            }

            return dt;
        }

        /* =========================
         *  B) EXPORT TỪ DATATABLE (giữ nguyên để tương thích cũ)
         * ========================= */

        public sealed class SheetTableSpec
        {
            public string SheetName;
            public DataTable Table;
            public SheetTableSpec(string sheetName, DataTable table)
            {
                SheetName = sheetName;
                Table = table;
            }
        }

        public static void ExportDataTables(string suggestedFileName, IList<SheetTableSpec> sheets)
        {
            if (sheets == null || sheets.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = !string.IsNullOrWhiteSpace(suggestedFileName)
                               ? suggestedFileName
                               : string.Format("Export_{0:yyyyMMdd_HHmm}.xlsx", DateTime.Now);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        for (int i = 0; i < sheets.Count; i++)
                        {
                            var spec = sheets[i];
                            if (spec == null || spec.Table == null || spec.Table.Rows.Count == 0) continue;

                            var ws = wb.Worksheets.Add(SafeSheetName(spec.SheetName));
                            var tableName = "T_" + Guid.NewGuid().ToString("N");
                            ws.Cell(1, 1).InsertTable(spec.Table, tableName);

                            StyleWorksheet(ws, spec.Table);
                        }

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /* =========================
         *  C) EXPORT TỪ DANH SÁCH ĐỐI TƯỢNG (KHÔNG CẦN DATATABLE)
         * ========================= */

        /// <summary>
        /// Cấu hình một cột khi export object.
        /// </summary>
        public sealed class ColumnSpec<T>
        {
            public string Header;                       // tiêu đề cột
            public Func<T, object> Selector;            // lấy dữ liệu từ item
            public string NumberFormat;                 // ví dụ "#,##0.00"
            public XLDataType? DataType;                // ví dụ XLDataType.Number
            public double? Width;                       // ví dụ 120

            public ColumnSpec()
            {
                Header = string.Empty;
                Selector = _ => null;
                NumberFormat = null;
                DataType = null;
                Width = null;
            }
        }

        /// <summary>
        /// Xuất 1 sheet từ danh sách đối tượng T với cấu hình cột (không dùng DataTable).
        /// </summary>
        public static void ExportObjects<T>(string suggestedFileName, string sheetName, IEnumerable<T> data, IList<ColumnSpec<T>> columns)
        {
            if (data == null)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (columns == null || columns.Count == 0)
            {
                MessageBox.Show("Chưa cấu hình cột (headers).", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = !string.IsNullOrWhiteSpace(suggestedFileName)
                               ? suggestedFileName
                               : string.Format("Export_{0:yyyyMMdd_HHmm}.xlsx", DateTime.Now);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add(SafeSheetName(sheetName));

                        // Header
                        WriteHeader(ws, columns.Select(c => c.Header).ToArray());

                        // Data
                        int r = 2;
                        foreach (var item in data)
                        {
                            for (int c = 0; c < columns.Count; c++)
                            {
                                object val = null;
                                try { val = columns[c].Selector(item); }
                                catch { val = null; }

                                var cell = ws.Cell(r, c + 1);
                                TryWriteCell(cell, val, columns[c].DataType, columns[c].NumberFormat);
                            }
                            r++;
                        }

                        // Style + kích thước
                        FinalizeSheet(ws, columns.Count, columns.Select(x => x.Width).ToArray());

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Khi muốn export nhiều sheet từ danh sách đối tượng nhưng mỗi sheet có thể là kiểu khác nhau.
        /// </summary>
        public sealed class ObjectSheetSpec
        {
            public string SheetName;
            public IEnumerable Data;                     // IEnumerable của item
            public IList<ObjectColumnSpec> Columns;      // cột dạng object
            public ObjectSheetSpec(string sheetName, IEnumerable data, IList<ObjectColumnSpec> columns)
            {
                SheetName = sheetName;
                Data = data;
                Columns = columns;
            }
        }

        public sealed class ObjectColumnSpec
        {
            public string Header;
            public Func<object, object> Selector;
            public string NumberFormat;
            public XLDataType? DataType;
            public double? Width;

            public ObjectColumnSpec()
            {
                Header = string.Empty;
                Selector = _ => null;
                NumberFormat = null;
                DataType = null;
                Width = null;
            }
        }

        /// <summary>
        /// Xuất nhiều sheet từ nhiều danh sách đối tượng (không dùng DataTable).
        /// </summary>
        public static void ExportObjectsMulti(string suggestedFileName, IList<ObjectSheetSpec> sheets)
        {
            if (sheets == null || sheets.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                sfd.FileName = !string.IsNullOrWhiteSpace(suggestedFileName)
                               ? suggestedFileName
                               : string.Format("Export_{0:yyyyMMdd_HHmm}.xlsx", DateTime.Now);

                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var wb = new XLWorkbook())
                    {
                        for (int si = 0; si < sheets.Count; si++)
                        {
                            var spec = sheets[si];
                            if (spec == null || spec.Columns == null || spec.Columns.Count == 0) continue;

                            var ws = wb.Worksheets.Add(SafeSheetName(spec.SheetName));

                            // Header
                            WriteHeader(ws, spec.Columns.Select(c => c.Header).ToArray());

                            // Data
                            int r = 2;
                            foreach (var item in spec.Data ?? Enumerable.Empty<object>())
                            {
                                for (int c = 0; c < spec.Columns.Count; c++)
                                {
                                    object val = null;
                                    try { val = spec.Columns[c].Selector(item); }
                                    catch { val = null; }

                                    var cell = ws.Cell(r, c + 1);
                                    TryWriteCell(cell, val, spec.Columns[c].DataType, spec.Columns[c].NumberFormat);
                                }
                                r++;
                            }

                            // Style + kích thước
                            FinalizeSheet(ws, spec.Columns.Count, spec.Columns.Select(x => x.Width).ToArray());
                        }

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Xuất Excel thành công.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Xuất Excel thất bại: " + ex.Message, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /* =========================
         *  D) HELPERS CHUNG
         * ========================= */

        private static void WriteHeader(IXLWorksheet ws, string[] headers)
        {
            for (int c = 0; c < headers.Length; c++)
            {
                var cell = ws.Cell(1, c + 1);
                cell.Value = headers[c] ?? string.Empty;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
            }
        }

        /// <summary>
        /// Ghi cell: ưu tiên DataType người dùng set; nếu null thì tự suy luận kiểu để không bị "text hóa".
        /// Hỗ trợ NumberFormat nếu truyền vào.
        /// </summary>
        private static void TryWriteCell(IXLCell cell, object value, XLDataType? dataType, string numberFormat)
        {
            // Null -> chuỗi rỗng
            if (value == null || value == DBNull.Value)
            {
                cell.Value = string.Empty;
                return;
            }

            // Nếu caller đã chỉ định DataType, tôn trọng
            if (dataType.HasValue)
            {
                // Với một số kiểu, ClosedXML cần set DataType + Value đúng kiểu
                if (!string.IsNullOrEmpty(numberFormat))
                    cell.Style.NumberFormat.Format = numberFormat;

                // Cố gắng set theo kiểu tương thích
                switch (dataType.Value)
                {
                    case XLDataType.Number:
                        // chấp nhận các kiểu số phổ biến
                        if (value is IConvertible)
                        {
                            try
                            {
                                // Parse an toàn: hỗ trợ cả string số
                                double d = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                                cell.Value = d;
                                return;
                            }
                            catch { /* fallback dưới */ }
                        }
                        break;

                    case XLDataType.DateTime:
                        if (value is DateTime dt)
                        {
                            cell.Value = dt;
                            return;
                        }
                        // cố gắng parse string -> DateTime
                        DateTime parsed;
                        if (DateTime.TryParse(value.ToString(), out parsed))
                        {
                            cell.Value = parsed;
                            return;
                        }
                        break;

                    case XLDataType.Boolean:
                        if (value is bool b)
                        {
                            cell.Value = b;
                            return;
                        }
                        bool pb;
                        if (bool.TryParse(value.ToString(), out pb))
                        {
                            cell.Value = pb;
                            return;
                        }
                        break;

                    default:
                        // Cho Text/TimeSpan… để ClosedXML tự xử lý
                        cell.Value = value.ToString();
                        return;
                }

                // nếu không convert được theo DataType đã chỉ định, fallback ghi chuỗi
                cell.Value = value.ToString();
                return;
            }

            // Không chỉ định DataType: tự suy luận
            // 1) số
            if (value is sbyte || value is byte || value is short || value is ushort ||
                value is int || value is uint || value is long || value is ulong ||
                value is float || value is double || value is decimal)
            {
                cell.Value = Convert.ToDouble(value, CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(numberFormat))
                    cell.Style.NumberFormat.Format = numberFormat;
                return;
            }

            // 2) bool
            if (value is bool vb)
            {
                cell.Value = vb;
                return;
            }

            // 3) datetime
            if (value is DateTime vdt)
            {
                cell.Value = vdt;
                if (!string.IsNullOrEmpty(numberFormat))
                    cell.Style.NumberFormat.Format = numberFormat; // ví dụ "dd/mm/yyyy"
                return;
            }

            // 4) string có thể là số hoặc ngày
            var s = value.ToString();
            double num;
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
            {
                cell.Value = num;
                if (!string.IsNullOrEmpty(numberFormat))
                    cell.Style.NumberFormat.Format = numberFormat;
                return;
            }
            DateTime sdt;
            if (DateTime.TryParse(s, out sdt))
            {
                cell.Value = sdt;
                if (!string.IsNullOrEmpty(numberFormat))
                    cell.Style.NumberFormat.Format = numberFormat;
                return;
            }

            // 5) mặc định: text
            cell.Value = s ?? string.Empty;
        }

        private static void FinalizeSheet(IXLWorksheet ws, int columnCount, double?[] widths)
        {
            var used = ws.RangeUsed();
            if (used == null) return;

            ws.SheetView.FreezeRows(1);
            used.SetAutoFilter();
            ws.Columns(1, columnCount).AdjustToContents();

            for (int c = 0; c < columnCount; c++)
            {
                if (widths != null && c < widths.Length && widths[c].HasValue)
                    ws.Column(c + 1).Width = widths[c].Value;
            }

            // Căn giữa header theo chiều dọc cho đẹp
            var header = ws.Range(1, 1, 1, columnCount);
            header.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        }

        private static void StyleWorksheet(IXLWorksheet ws, DataTable dt)
        {
            var used = ws.RangeUsed();
            if (used == null) return;

            var header = ws.Range(1, 1, 1, dt.Columns.Count);
            header.Style.Font.Bold = true;
            header.Style.Fill.BackgroundColor = XLColor.FromHtml("#F2F2F2");
            header.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            ws.SheetView.FreezeRows(1);
            used.SetAutoFilter();
            ws.Columns().AdjustToContents();
        }

        /// <summary>
        /// Làm sạch tên sheet, giới hạn 31 ký tự – không dùng range operator.
        /// </summary>
        private static string SafeSheetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return "Sheet1";

            var badChars = new List<char>();
            badChars.AddRange(System.IO.Path.GetInvalidFileNameChars());
            badChars.Add('['); badChars.Add(']'); badChars.Add('*'); badChars.Add('?'); badChars.Add('/'); badChars.Add('\\');

            var arr = name.ToCharArray();
            for (int i = 0; i < arr.Length; i++)
            {
                if (badChars.Contains(arr[i]))
                    arr[i] = '_';
            }

            var safe = new string(arr);
            if (safe.Length > 31) return safe.Substring(0, 31);
            return safe;
        }
    }
}



//1) Một sheet từ List<T>:
//var columns = new List<ExcelExporter.ColumnSpec<MyRow>>
//{
//    new ExcelExporter.ColumnSpec<MyRow>{ Header="Mã", Selector=x=>x.Code },
//    new ExcelExporter.ColumnSpec<MyRow>{ Header="Số lượng", Selector=x=>x.Qty, DataType=XLDataType.Number, NumberFormat="#,##0" },
//    new ExcelExporter.ColumnSpec<MyRow>{ Header="Đơn giá", Selector=x=>x.Price, DataType=XLDataType.Number, NumberFormat="#,##0.00" },
//    new ExcelExporter.ColumnSpec<MyRow>{ Header="Ngày", Selector=x=>x.CreatedAt, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy" },
//};
//ExcelExporter.ExportObjects("BangGia.xlsx", "Bảng giá", myList, columns);


//2) Nhiều sheet(mỗi sheet có kiểu khác nhau):
//var sheets = new List<ExcelExporter.ObjectSheetSpec>
//{
//    new ExcelExporter.ObjectSheetSpec(
//        "Sản phẩm",
//        products, // IEnumerable
//        new List<ExcelExporter.ObjectColumnSpec>{
//            new ExcelExporter.ObjectColumnSpec{ Header="Mã", Selector=o=>((Product)o).Code },
//            new ExcelExporter.ObjectColumnSpec{ Header="Giá", Selector=o=>((Product)o).Price, DataType=XLDataType.Number, NumberFormat="#,##0" }
//        }
//    ),
//    new ExcelExporter.ObjectSheetSpec(
//        "Khách hàng",
//        customers,
//        new List<ExcelExporter.ObjectColumnSpec>{
//            new ExcelExporter.ObjectColumnSpec{ Header="Tên", Selector=o=>((Customer)o).Name },
//            new ExcelExporter.ObjectColumnSpec{ Header="Ngày sinh", Selector=o=>((Customer)o).Dob, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy" }
//        }
//    )
//};
//ExcelExporter.ExportObjectsMulti("BaoCao.xlsx", sheets);
