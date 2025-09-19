using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditExportStockMaster : Form
    {
        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM Warehouse";
        string[] columnsWareText = new[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new[] { 60, 120, 250 };


        string strQueryDetail =
                "SELECT a.ProductsId, b.Name AS ProductsName, " +
                "CONVERT(VARCHAR, a.Amount) AS Amount, CONVERT(VARCHAR, a.Price) AS Price, " +
                "CONVERT(VARCHAR, a.TotalMoney) AS TotalMoney, a.Note, a.SalesInvoiceDetailId " +
                "FROM ExportStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";


        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá xuất", "Tổng tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 120, 120, 120, 250 };
        DataTable dtDetail = new DataTable();

        string strQueryProduct =
            "SELECT CAST(0 AS BIT) AS xtag,Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Số lượng tồn tối thiểu", "Số lượng tồn tối đa", "Mô tả", "Hạn sử dụng", "Trạng thái sử dụng" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 120, 120, 120, 250, 120, 180 };

        string strQueryCustomer = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Customer";
        string[] columnsCustomerText = new string[] { "Chọn", "Mã kh", "Tên kh" };
        int[] widthCustomer = new int[] { 60, 120, 250 };

        // Chi tiết HĐ bán (IsRetail=0) còn lại chưa xuất (RemainQty>0)
        string strQuerySIRemainingDetail = @"
                                            SELECT CAST(0 AS BIT) AS xtag,
                                                   m.Id AS SIMasterId, d.Id AS SIDetailId,
                                                   m.DocDate, m.CustomerId, c.Name AS CustomerName,
                                                   d.ProductId, p.Name AS ProductName,
                                                   d.Qty AS QtyOrdered,
                                                    (d.Qty - ISNULL(x.ShippedQty,0)) AS RemainQty,
                                                   ISNULL(d.UnitPrice,0) AS UnitPrice
                                            FROM SalesInvoiceDetail d
                                            JOIN SalesInvoiceMaster m  ON d.SIMasterId = m.Id
                                            JOIN Products p           ON d.ProductId  = p.Id
                                            LEFT JOIN Customer c      ON m.CustomerId = c.Id
                                            LEFT JOIN (
                                               SELECT SalesInvoiceDetailId, SUM(Amount) AS ShippedQty
                                               FROM ExportStockDetail
                                               GROUP BY SalesInvoiceDetailId
                                            ) x ON x.SalesInvoiceDetailId = d.Id
                                            WHERE m.IsRetail = 0
                                              AND (d.Qty - ISNULL(x.ShippedQty,0)) > 0";
        string[] columnsSIRemainText = {
            "Chọn","Mã HĐ","Mã dòng","Ngày HĐ","Mã KH", "Tên KH","Mã SP","Tên SP",
            "SL HĐ","Còn lại","Đơn giá"
        };
        int[] widthSIRemain = { 60, 120, 120, 110, 120, 110, 240, 90, 90, 90, 110 };



        // Lưu HĐ bán đã chọn để gán vào PXK
        string _selectedSIId = null;

        public frmEditExportStockMaster()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);

            this.btnSave.Click += btnSave_Click;
            this.btnChooseCustomer.Click += btnChooseCustomer_Click;
            this.Load += frm_Load;

            this.dtgvDetail.CellEndEdit += dtgvDetail_CellEndEdit;
            this.dtgvDetail.CellValidating += dtgvDetail_CellValidating;
            this.dtgvDetail.KeyDown += dtgvDetail_KeyDown;

            //  lấy từ HĐ bán 
            this.btnLayTuHoaDonBan.Click += btnLayTuHoaDonBan_Click;
            // ctor
            this.btnChooseWarehouse.Click += btnChooseWarehouse_Click;

        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            dtpVoucherDate.DataBindings.Add("Text", binSource, "VoucherDate");
            txtCustomerId.DataBindings.Add("Text", binSource, "CustomerId");
            txtNote.DataBindings.Add("Text", binSource, "Note");
            txtTotalAmount.DataBindings.Add("Text", binSource, "TotalAmount");
            txtTotalMoney.DataBindings.Add("Text", binSource, "TotalMoney");
            txtWarehouseId.DataBindings.Add("Text", binSource, "WarehouseId");
        }

        private void btnChooseWarehouse_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, txtWarehouseId.Text, out outId, out outName);
            txtWarehouseId.Text = outId; lblWarehouseName.Text = outName;
        }

        private void frm_Load(object sender, EventArgs e)
        {
            lblWarehouseName.Text = !string.IsNullOrEmpty(txtWarehouseId.Text)
                ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id=@Id", new object[,] { { "@Id", txtWarehouseId.Text } })
                : "";

            lblCustomerName.Text = !string.IsNullOrEmpty(txtCustomerId.Text)
                ? Models.SQL.GetValue("SELECT Name FROM Customer WHERE Id=@Id", new object[,] { { "@Id", txtCustomerId.Text } })
                : "";
            if (frmExportStockMaster.isSave && !frmExportStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PXK");
            }
            else if (!frmExportStockMaster.isSave || frmExportStockMaster.isCopy)
            {
                txtTotalMoney.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", ToDec(txtTotalMoney.Text));
                txtTotalAmount.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", ToDec(txtTotalAmount.Text));
            }
            txtId.ReadOnly = true;

            string key = " WHERE ExportStockMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + key, columnsNameDetail, widthDetail, out dtDetail);

            if (!dtDetail.Columns.Contains("SalesInvoiceDetailId"))
                dtDetail.Columns.Add("SalesInvoiceDetailId", typeof(string));


            if (dtgvDetail.Columns.Contains("SalesInvoiceDetailId"))
                dtgvDetail.Columns["SalesInvoiceDetailId"].Visible = false;

            var dtMap = Models.SQL.GetData(@"
                        SELECT ProductsId,
                               CONVERT(VARCHAR, Amount) AS Amount,
                               CONVERT(VARCHAR, Price)  AS Price,
                               SalesInvoiceDetailId
                        FROM ExportStockDetail
                        WHERE ExportStockMasterId = '" + txtId.Text + "'");

            foreach (DataRow m in dtMap.Rows)
            {
                var found = dtDetail.Select(
                    $"ProductsId = '{m["ProductsId"]}' AND Amount = '{m["Amount"]}' AND Price = '{m["Price"]}'");
                if (found.Length > 0)
                    found[0]["SalesInvoiceDetailId"] =
                        m["SalesInvoiceDetailId"] == DBNull.Value ? null : m["SalesInvoiceDetailId"].ToString();
            }

            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            dtgvDetail.Columns["TotalMoney"].ReadOnly = true;

            if (frmExportStockMaster.isCopy) txtId.Text = DLLSystem.GenCode("PXK");
        }

        private string BuildExcludeWhereFromGrid()
        {
            var ids = new List<string>();
            foreach (DataRow r in dtDetail.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                var sid = Convert.ToString(r["SalesInvoiceDetailId"]);
                if (!string.IsNullOrEmpty(sid))
                    ids.Add(sid.Replace("'", "''"));
            }
            if (ids.Count == 0) return string.Empty;
            return " AND d.Id NOT IN ('" + string.Join("','", ids) + "')";
        }


        private void btnLayTuHoaDonBan_Click(object sender, EventArgs e)
        {
            // 1) Ghép query cuối cùng với bộ lọc:
            //    - Nếu đã chọn 1 HĐ trước đó, chỉ hiển thị dòng còn lại của HĐ đó
            //    - Loại bỏ các dòng đã có trên grid bằng NOT IN SIDetailId
            string whereExtra = "";
            if (!string.IsNullOrEmpty(_selectedSIId))
                whereExtra += " AND m.Id = '" + _selectedSIId.Replace("'", "''") + "'";

            whereExtra += BuildExcludeWhereFromGrid();

            string finalQuery = strQuerySIRemainingDetail + whereExtra;

            // 2) Người dùng chọn nhiều dòng còn lại
            DataTable dtPicked;
            Lookup.SearchLookupMulti(
                finalQuery,
                columnsSIRemainText, // ví dụ: new[] { "Chọn", "Mã HĐ", "Mã dòng", "Ngày HĐ", "Mã KH", "Khách hàng", "Mã SP", "Tên SP", "SL đặt", "Giá", "SL còn" }
                widthSIRemain,
                out dtPicked
            );
            if (dtPicked == null || dtPicked.Rows.Count == 0) return;

            // 3) Ràng buộc: cùng 1 HĐ + cùng KH
            string siId = dtPicked.Rows[0]["SIMasterId"].ToString();
            string cus = dtPicked.Rows[0]["CustomerId"].ToString();
            foreach (DataRow r in dtPicked.Rows)
            {
                if (r["SIMasterId"].ToString() != siId)
                { Alert.Error("Vui lòng chọn các dòng thuộc CÙNG một hóa đơn bán."); return; }
                if (r["CustomerId"].ToString() != cus)
                { Alert.Error("Các dòng có khách hàng khác nhau."); return; }
            }

            _selectedSIId = siId; // gắn vào PXK
            txtCustomerId.Text = cus;
            lblCustomerName.Text = Models.SQL.GetValue(
                "SELECT Name FROM Customer WHERE Id=@Id",
                new object[,] { { "@Id", cus } });

            dtpVoucherDate.Value = Convert.ToDateTime(dtPicked.Rows[0]["DocDate"]);
            txtNote.Text = "Tạo từ HĐ bán: " + _selectedSIId;

            // 4) Đổ các dòng đã chọn vào lưới chi tiết + set SalesInvoiceDetailId để lần sau loại bỏ
            foreach (DataRow r in dtPicked.Rows)
            {
                string pid = r["ProductId"].ToString();
                string pname = r["ProductName"].ToString();
                decimal remain = ToDec(r["RemainQty"].ToString());
                decimal price = ToDec(r["UnitPrice"].ToString());
                decimal sum = remain * price;

                var nr = dtDetail.NewRow();
                nr["ProductsId"] = pid;
                nr["ProductsName"] = pname;
                nr["Amount"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", remain);
                nr["Price"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", price);
                nr["TotalMoney"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", sum);
                nr["Note"] = "Từ HĐ: " + _selectedSIId + " | Dòng: " + r["SIDetailId"].ToString();
                nr["SalesInvoiceDetailId"] = r["SIDetailId"].ToString();
                dtDetail.Rows.Add(nr);
            }



            dtDetail.AcceptChanges();
            Total();
        }

        private void btnChooseCustomer_Click(object sender, EventArgs e)
        {
            string OutCustomerId, OutCustomerName;
            Lookup.SearchLookupSingle(strQueryCustomer, columnsCustomerText, widthCustomer, txtCustomerId.Text,
                out OutCustomerId, out OutCustomerName);
            txtCustomerId.Text = OutCustomerId;
            lblCustomerName.Text = OutCustomerName;
        }

        // ===== GRID =====
        private void dtgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dtgvDetail.Columns[e.ColumnIndex].Name == "Amount" || dtgvDetail.Columns[e.ColumnIndex].Name == "Price")
            {
                string amountValue = (dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value ?? "0").ToString().Replace(",", "");
                string priceValue = (dtgvDetail.Rows[e.RowIndex].Cells["Price"].Value ?? "0").ToString().Replace(",", "");
                if (amountValue == "") amountValue = "0";
                if (priceValue == "") priceValue = "0";
                try
                {
                    decimal amount = Convert.ToDecimal(amountValue);
                    decimal price = Convert.ToDecimal(priceValue);
                    decimal totalMoney = amount * price;

                    dtgvDetail.Rows[e.RowIndex].Cells["TotalMoney"].Value = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalMoney);
                    dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value = string.Format(CultureInfo.InvariantCulture, "{0:N0}", amount);
                    dtgvDetail.Rows[e.RowIndex].Cells["Price"].Value = string.Format(CultureInfo.InvariantCulture, "{0:N0}", price);
                    Total();
                }
                catch (FormatException ex) { MessageBox.Show("Lỗi định dạng: " + ex.Message); }
            }
            else if (dtgvDetail.Columns[e.ColumnIndex].Name == "ProductsId")
            {
                string productId = (dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value ?? "").ToString();
                string OutProductId, OutProductName;
                string KeySearch = " WHERE Id LIKE N'%" + productId + "%' OR Name LIKE N'%" + productId + "%'";
                Lookup.SearchLookupSingle(strQueryProduct, columnsProductText, widthProduct, productId, out OutProductId, out OutProductName, KeySearch);
                dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value = OutProductId;
                dtgvDetail.Rows[e.RowIndex].Cells["ProductsName"].Value = OutProductName;
            }
        }
        private void dtgvDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dtgvDetail.Columns[e.ColumnIndex].Name == "Amount" || dtgvDetail.Columns[e.ColumnIndex].Name == "Price")
            {
                string inputValue = e.FormattedValue.ToString().Trim();
                if (!string.IsNullOrEmpty(inputValue) &&
                    !System.Text.RegularExpressions.Regex.IsMatch(inputValue.Replace(",", ""), @"^\d+(\.\d+)?$"))
                {
                    e.Cancel = true;
                }
            }
        }
        private void dtgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3) dtDetail.Rows.Add(dtDetail.NewRow());
            if (e.KeyCode == Keys.F8)
            {
                if (dtgvDetail.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dtgvDetail.SelectedRows[0];
                    dtDetail.Rows.RemoveAt(selectedRow.Index);
                    Total();
                }
                else Alert.Error("Vui lòng chọn một dòng để xóa!");
            }
        }

        // ===== SAVE: query + kiểm tra tồn =====
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWarehouseId.Text))
            { Alert.Error("Chưa chọn kho xuất"); txtWarehouseId.Focus(); return; }

            if (string.IsNullOrEmpty(txtCustomerId.Text))
            { Alert.Error("Chưa nhập thông tin khách hàng"); txtCustomerId.Focus(); return; }

            // Lọc dòng rỗng/không hợp lệ
            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                var r = dtgvDetail.Rows[i];
                if (r.IsNewRow) { dtgvDetail.Rows.RemoveAt(i); continue; }

                if (r.Cells["ProductsId"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["ProductsId"].Value)) ||
                    r.Cells["Amount"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["Amount"].Value)))
                {
                    dtgvDetail.Rows.RemoveAt(i);
                }
                else
                {
                    bool ok = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id='" + r.Cells["ProductsId"].Value.ToString() + "'");
                    if (!ok) dtgvDetail.Rows.RemoveAt(i);
                }
            }
            if (dtgvDetail.Rows.Count == 0) { Alert.Error("Chưa nhập thông tin chi tiết"); return; }

            // Kiểm tra tồn (theo PostData đến ngày chứng từ)
            string err = "";
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;
                string pid = (r.Cells["ProductsId"].Value ?? "").ToString();
                decimal qtyNeed = ToDec((r.Cells["Amount"].Value ?? "0").ToString());
                decimal onHand = GetOnHandByWarehouse(pid, txtWarehouseId.Text, dtpVoucherDate.Value);
                if (qtyNeed > onHand)
                {
                    string pname = (r.Cells["ProductsName"].Value ?? pid).ToString();
                    err += $"- {pname}: cần {qtyNeed:N0}, tồn {onHand:N0}\n";
                }
            }
            if (!string.IsNullOrEmpty(err))
            {
                Alert.Error("Không đủ tồn để xuất:\n" + err);
                return;
            }

            // SAU khi lọc rỗng & TRƯỚC khi tính Total()
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                var drv = r.DataBoundItem as DataRowView;
                string sid = null;
                if (drv != null && drv.Row.Table.Columns.Contains("SalesInvoiceDetailId"))
                    sid = drv.Row["SalesInvoiceDetailId"] == DBNull.Value ? null : drv.Row["SalesInvoiceDetailId"].ToString();

                if (!string.IsNullOrEmpty(sid))
                {
                    // SL đã đặt trong HĐ
                    var orderedStr = Models.SQL.GetValue(
                        "SELECT CONVERT(decimal(18,3), Qty) FROM SalesInvoiceDetail WHERE Id=@SID",
                        new object[,] { { "@SID", sid } });
                    decimal ordered = string.IsNullOrEmpty(orderedStr) ? 0m : ToDec(orderedStr);

                    // Đã xuất trước đó (các PXK khác)
                    var shippedStr = Models.SQL.GetValue(
                        "SELECT CONVERT(decimal(18,3), ISNULL(SUM(Amount),0)) FROM ExportStockDetail WHERE SalesInvoiceDetailId=@SID",
                        new object[,] { { "@SID", sid } });
                    decimal shipped = string.IsNullOrEmpty(shippedStr) ? 0m : ToDec(shippedStr);

                    // SL đang chuẩn bị xuất ở PHIẾU HIỆN TẠI (chỉ tính tổng các dòng cùng SID trong lưới)
                    decimal qtyInThisVoucher = 0m;
                    foreach (DataGridViewRow rr in dtgvDetail.Rows)
                    {
                        if (rr.IsNewRow) continue;
                        var drv2 = rr.DataBoundItem as DataRowView;
                        string sid2 = null;
                        if (drv2 != null && drv2.Row.Table.Columns.Contains("SalesInvoiceDetailId"))
                            sid2 = drv2.Row["SalesInvoiceDetailId"] == DBNull.Value ? null : drv2.Row["SalesInvoiceDetailId"].ToString();
                        if (sid2 == sid)
                            qtyInThisVoucher += ToDec((rr.Cells["Amount"].Value ?? "0").ToString());
                    }

                    decimal remain = ordered - shipped;
                    if (qtyInThisVoucher > remain + 0.0001m)
                    {
                        string pname = (r.Cells["ProductsName"].Value ?? "").ToString();
                        Alert.Error($"Dòng HĐ đã vượt còn lại:\n- SP: {pname}\n- Đặt: {ordered:N3}, Đã xuất: {shipped:N3}, Còn: {remain:N3}\n- Bạn đang xuất: {qtyInThisVoucher:N3}");
                        return;
                    }
                }
            }

            // Tổng
            Total();
            decimal totalQty = ToDec(txtTotalAmount.Text);
            decimal totalAmt = ToDec(txtTotalMoney.Text);

            // MASTER
            if (frmExportStockMaster.isSave || frmExportStockMaster.isCopy)
            {
                string sqlIns = @"
                    INSERT INTO ExportStockMaster
                     (Id, VoucherDate, WarehouseId, CustomerId, Note, TotalAmount, TotalMoney,
                      UserCreate, DateCreate, SalesInvoiceId)
                    VALUES
                     (@Id, @Date, @Wh, @Cus, @Note, @TQty, @TMoney, @User, GETDATE(), @SI)";

                Models.SQL.RunQuery(sqlIns, new object[,] {
                    {"@Id", txtId.Text.Trim()}, {"@Date", dtpVoucherDate.Value},
                    {"@Wh", txtWarehouseId.Text.Trim()}, {"@Cus", txtCustomerId.Text.Trim()},
                    {"@Note", txtNote.Text.Trim()}, {"@TQty", totalQty}, {"@TMoney", totalAmt},
                    {"@User", frmLogin.UserName}, {"@SI", string.IsNullOrEmpty(_selectedSIId) ? (object)DBNull.Value : _selectedSIId}
                });
            }
            else
            {
                string sqlUpd = @"
                UPDATE ExportStockMaster
                SET VoucherDate=@Date, WarehouseId=@Wh, CustomerId=@Cus, Note=@Note,
                    TotalAmount=@TQty, TotalMoney=@TMoney,
                    UserUpdate=@User, DateUpdate=GETDATE(),
                    SalesInvoiceId=COALESCE(NULLIF(@SI,''), SalesInvoiceId)
                WHERE Id=@Id";

                Models.SQL.RunQuery(sqlUpd, new object[,] {
                    {"@Id", txtId.Text.Trim()}, {"@Date", dtpVoucherDate.Value},
                    {"@Wh", txtWarehouseId.Text.Trim()}, {"@Cus", txtCustomerId.Text.Trim()},
                    {"@Note", txtNote.Text.Trim()}, {"@TQty", totalQty}, {"@TMoney", totalAmt},
                    {"@User", frmLogin.UserName}, {"@SI", string.IsNullOrEmpty(_selectedSIId) ? (object)DBNull.Value : _selectedSIId}
                });
                // Xóa detail & PostData cũ
                Models.SQL.RunQuery("DELETE FROM ExportStockDetail WHERE ExportStockMasterId=@Id",
                    new object[,] { { "@Id", txtId.Text.Trim() } });
                Models.SQL.RunQuery("DELETE FROM PostData WHERE VoucherId=@Id AND VoucherCode='PXK'",
                    new object[,] { { "@Id", txtId.Text.Trim() } });
            }

            // DETAIL + PostData âm
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                string prod = (r.Cells["ProductsId"].Value ?? "").ToString();
                decimal qty = ToDec((r.Cells["Amount"].Value ?? "0").ToString());
                decimal price = ToDec((r.Cells["Price"].Value ?? "0").ToString());
                decimal sum = ToDec((r.Cells["TotalMoney"].Value ?? "0").ToString());
                string note = (r.Cells["Note"].Value ?? "").ToString();

                string siDetailId = null;
                var drv = r.DataBoundItem as DataRowView;
                if (drv != null && drv.Row.Table.Columns.Contains("SalesInvoiceDetailId"))
                {
                    var v = drv.Row["SalesInvoiceDetailId"];
                    siDetailId = v == DBNull.Value ? null : v?.ToString();
                }

                string insDet = @"
                                INSERT INTO ExportStockDetail
                                    (Id, ExportStockMasterId, ProductsId, Amount, Price, TotalMoney, Note, SalesInvoiceDetailId)
                                VALUES
                                    (@Id, @Mid, @Pid, @Qty, @Price, @Sum, @Note, @SIDetailId)";
                Models.SQL.RunQuery(insDet, new object[,]
                {
                                {"@Id",  Guid.NewGuid().ToString("N")},
                                {"@Mid", txtId.Text.Trim()},
                                {"@Pid", prod},
                                {"@Qty", qty},
                                {"@Price", price},
                                {"@Sum", sum},
                                {"@Note", note},
                                {"@SIDetailId", string.IsNullOrEmpty(siDetailId) ? (object)DBNull.Value : siDetailId}
});

                string insPost = @"
                    INSERT INTO PostData
                      (VoucherId, VoucherCode, VoucherDate, WarehouseId, ProductsId, Amount, Price, TotalMoney)
                    VALUES
                      (@Vid, 'PXK', @Vdate, @Wh, @Pid, @Amt, @Price, @TMoney)";

                Models.SQL.RunQuery(insPost, new object[,] {
                      {"@Vid", txtId.Text.Trim()}, {"@Vdate", dtpVoucherDate.Value},
                      {"@Wh", txtWarehouseId.Text.Trim()},
                      {"@Pid", prod}, {"@Amt", -qty}, {"@Price", price}, {"@TMoney", -sum}
                    });

            }


            frmExportStockMaster.isLoad = true;
            Alert.Infor("Đã lưu phiếu xuất.");
            this.Close();
        }

        // ===== Helper =====
        private static decimal ToDec(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            decimal.TryParse(s.Replace(",", ""), out var d); return d;
        }
        private void Total()
        {
            decimal totalMoney = 0, totalAmount = 0;
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;
                totalMoney += ToDec((r.Cells["TotalMoney"].Value ?? "0").ToString());
                totalAmount += ToDec((r.Cells["Amount"].Value ?? "0").ToString());
            }
            txtTotalAmount.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalAmount);
            txtTotalMoney.Text = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalMoney);
        }

        private decimal GetOnHandByWarehouse(string productId, string warehouseId, DateTime toDate)
        {
            var val = Models.SQL.GetValue(@"
                    SELECT CONVERT(decimal(18,3), ISNULL(SUM(Amount),0))
                    FROM PostData
                    WHERE ProductsId=@P AND VoucherDate<=@D
                      AND (@W='' OR WarehouseId=@W)",
                new object[,] { { "@P", productId }, { "@D", toDate }, { "@W", warehouseId ?? "" } });
            return string.IsNullOrEmpty(val) ? 0m : ToDec(val);
        }

    }
}
