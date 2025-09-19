using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditImportStockMaster : Form
    {
        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Warehouse";
        string[] columnsWareText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new int[] { 60, 120, 250 };

        string strQueryDetail =
                "SELECT a.ProductsId, b.Name AS ProductsName," +
                "CONVERT(VARCHAR, a.Amount) AS Amount, CONVERT(VARCHAR, a.Price) AS Price," +
                "CONVERT(VARCHAR, a.TotalMoney) AS TotalMoney, a.Note, a.PurchaseInvoiceDetailId " +
                "FROM ImportStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";

        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá nhập", "Tổng tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 120, 120, 120, 250 };
        DataTable dtDetail = new DataTable();

        string strQueryProduct =
            "SELECT CAST(0 AS BIT) AS xtag,Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Tồn tối thiểu", "Tồn tối đa", "Mô tả", "HSD", "Trạng thái" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 120, 120, 120, 250, 120, 180 };

        string strQuerySupplier = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Supplier";
        string[] columnsSupplierText = new string[] { "Chọn", "Mã ncc", "Tên ncc" };
        int[] widthSupplier = new int[] { 60, 120, 250 };

        // Chi tiết HĐ mua còn lại chưa nhập (RemainQty > 0)
        string strQueryPIRemainingDetail = @"
            SELECT CAST(0 AS BIT) AS xtag,
                   m.Id  AS PIMasterId, d.Id AS PIDetailId,
                   m.DocDate, m.SupplierId, s.Name AS SupplierName,
                   d.ProductId, p.Name AS ProductName,
                   d.Qty AS QtyOrdered,
                   (d.Qty - ISNULL(r.ReceivedQty,0)) AS RemainQty,
                   ISNULL(d.UnitPrice,0) AS UnitPrice
            FROM PurchaseInvoiceDetail d
            JOIN PurchaseInvoiceMaster m  ON d.PIMasterId = m.Id
            JOIN Products p               ON d.ProductId  = p.Id
            LEFT JOIN Supplier s          ON m.SupplierId = s.Id
            LEFT JOIN (
               SELECT PurchaseInvoiceDetailId, SUM(Amount) AS ReceivedQty
               FROM ImportStockDetail
               GROUP BY PurchaseInvoiceDetailId
            ) r ON r.PurchaseInvoiceDetailId = d.Id
            WHERE (d.Qty - ISNULL(r.ReceivedQty,0)) > 0";

        string[] columnsPIRemainText = {
            "Chọn","Mã HĐ","Mã dòng","Ngày HĐ","Mã NCC", "Tên NCC",
            "Mã SP","Tên SP","SL HĐ","Còn lại","Đơn giá"
        };
        int[] widthPIRemain = { 60, 120, 120, 110, 120, 140, 120, 240, 90, 90, 110 };


        // Lưu lại HĐ mua đã chọn (không cần control ẩn)
        string _selectedPIId = null;

        public frmEditImportStockMaster()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);

            btnSave.Click += btnSave_Click;
            btnChooseWarehouse.Click += btnChooseWarehouse_Click;
            btnChooseSupplier.Click += btnChooseSupplier_Click;
            Load += frm_Load;

            dtgvDetail.CellEndEdit += dtgvDetail_CellEndEdit;
            dtgvDetail.CellValidating += dtgvDetail_CellValidating;
            dtgvDetail.KeyDown += dtgvDetail_KeyDown;

            btnLayTuHoaDonMua.Click += btnLayTuHoaDonMua_Click;
        }

        private void frm_Load(object sender, EventArgs e)
        {
            lblWarehouseName.Text = !string.IsNullOrEmpty(txtWarehouseId.Text)
                ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id=@Id", new object[,] { { "@Id", txtWarehouseId.Text } })
                : "";
            lblSupplierName.Text = !string.IsNullOrEmpty(txtSupplierId.Text)
                ? Models.SQL.GetValue("SELECT Name FROM Supplier WHERE Id=@Id", new object[,] { { "@Id", txtSupplierId.Text } })
                : "";

            if (frmImportStockMaster.isSave && !frmImportStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PNK");
            }
            else
            {
                string formattedTotalMoney = string.Format(CultureInfo.InvariantCulture, "{0:N0}", ToDec(txtTotalMoney.Text));
                string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", ToDec(txtTotalAmount.Text));
                txtTotalAmount.Text = formattedAmount;
                txtTotalMoney.Text = formattedTotalMoney;
            }
            txtId.ReadOnly = true;

            string key = " WHERE ImportStockMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + key, columnsNameDetail, widthDetail, out dtDetail);

            if (!dtDetail.Columns.Contains("PurchaseInvoiceDetailId"))
                dtDetail.Columns.Add("PurchaseInvoiceDetailId", typeof(string));

            // Ẩn cột trên lưới (AutoGenerateColumns = true)
            if (dtgvDetail.Columns.Contains("PurchaseInvoiceDetailId"))
                dtgvDetail.Columns["PurchaseInvoiceDetailId"].Visible = false;

            // Nếu mở phiếu đã lưu, map lại PIDetail vào dtDetail để lần sau lookup tự loại bỏ
            var dtMap = Models.SQL.GetData(@"
            SELECT ProductsId,
                   CONVERT(VARCHAR, Amount) AS Amount,
                   CONVERT(VARCHAR, Price)  AS Price,
                   PurchaseInvoiceDetailId
            FROM ImportStockDetail
            WHERE ImportStockMasterId = '" + txtId.Text + "'");

            foreach (DataRow m in dtMap.Rows)
            {
                var found = dtDetail.Select(
                    $"ProductsId = '{m["ProductsId"]}' AND Amount = '{m["Amount"]}' AND Price = '{m["Price"]}'");
                if (found.Length > 0)
                    found[0]["PurchaseInvoiceDetailId"] =
                        m["PurchaseInvoiceDetailId"] == DBNull.Value ? null : m["PurchaseInvoiceDetailId"].ToString();
            }

            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            dtgvDetail.Columns["TotalMoney"].ReadOnly = true;

            if (frmImportStockMaster.isCopy)
                txtId.Text = DLLSystem.GenCode("PNK");
        }

        // ====== Lookup buttons ======
        private void btnChooseWarehouse_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, txtWarehouseId.Text, out outId, out outName);
            txtWarehouseId.Text = outId; lblWarehouseName.Text = outName;
        }
        private void btnChooseSupplier_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQuerySupplier, columnsSupplierText, widthSupplier, txtSupplierId.Text, out outId, out outName);
            txtSupplierId.Text = outId; lblSupplierName.Text = outName;
        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            dtpVoucherDate.DataBindings.Add("Text", binSource, "VoucherDate");
            txtWarehouseId.DataBindings.Add("Text", binSource, "WarehouseId");
            txtSupplierId.DataBindings.Add("Text", binSource, "SupplierId");
            txtNote.DataBindings.Add("Text", binSource, "Note");
            txtTotalAmount.DataBindings.Add("Text", binSource, "TotalAmount");
            txtTotalMoney.DataBindings.Add("Text", binSource, "TotalMoney");
        }

        // ====== LẤY TỪ HÓA ĐƠN MUA ======
        private string BuildExcludeWhereFromGrid()
        {
            var ids = new List<string>();
            foreach (DataRow r in dtDetail.Rows)
            {
                if (r.RowState == DataRowState.Deleted) continue;
                var pid = Convert.ToString(r["PurchaseInvoiceDetailId"]);
                if (!string.IsNullOrEmpty(pid)) ids.Add(pid.Replace("'", "''"));
            }
            if (ids.Count == 0) return string.Empty;
            return " AND d.Id NOT IN ('" + string.Join("','", ids) + "')";
        }

        private void btnLayTuHoaDonMua_Click(object sender, EventArgs e)
        {
            // 1) Ràng buộc theo HĐ đã chọn (nếu có) + loại bỏ dòng đã nằm trên grid
            string whereExtra = "";
            if (!string.IsNullOrEmpty(_selectedPIId))
                whereExtra += " AND m.Id = '" + _selectedPIId.Replace("'", "''") + "'";
            whereExtra += BuildExcludeWhereFromGrid();

            string finalQuery = strQueryPIRemainingDetail + whereExtra;

            // 2) Chọn nhiều dòng còn lại
            DataTable dtPicked;
            Lookup.SearchLookupMulti(finalQuery, columnsPIRemainText, widthPIRemain, out dtPicked);
            if (dtPicked == null || dtPicked.Rows.Count == 0) return;

            // 3) Ràng buộc: cùng 1 HĐ & cùng NCC
            string piId = dtPicked.Rows[0]["PIMasterId"].ToString();
            string supp = dtPicked.Rows[0]["SupplierId"].ToString();
            foreach (DataRow r in dtPicked.Rows)
            {
                if (r["PIMasterId"].ToString() != piId)
                { Alert.Error("Vui lòng chọn các dòng thuộc CÙNG một hóa đơn mua."); return; }
                if (r["SupplierId"].ToString() != supp)
                { Alert.Error("Các dòng có nhà cung cấp khác nhau."); return; }
            }

            _selectedPIId = piId; // gắn vào PNK
            txtSupplierId.Text = supp;
            lblSupplierName.Text = Models.SQL.GetValue("SELECT Name FROM Supplier WHERE Id=@Id",
                new object[,] { { "@Id", supp } });

            // Ngày chứng từ theo HĐ
            dtpVoucherDate.Value = Convert.ToDateTime(dtPicked.Rows[0]["DocDate"]);
            txtNote.Text = "Tạo từ HĐ mua: " + _selectedPIId;

            // Kho: nếu HĐ có WarehouseId thì ưu tiên, nếu không thì hỏi chọn
            var wh = Models.SQL.GetValue("SELECT WarehouseId FROM PurchaseInvoiceMaster WHERE Id=@Id",
                new object[,] { { "@Id", _selectedPIId } });
            if (!string.IsNullOrEmpty(wh))
            {
                txtWarehouseId.Text = wh;
                if (string.IsNullOrEmpty(lblWarehouseName.Text))
                    lblWarehouseName.Text = Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id=@Id",
                        new object[,] { { "@Id", wh } });
            }
            else if (string.IsNullOrEmpty(txtWarehouseId.Text))
            {
                string outWhId, outWhName;
                Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, "", out outWhId, out outWhName);
                if (string.IsNullOrEmpty(outWhId)) { Alert.Error("Chưa chọn kho nhập"); return; }
                txtWarehouseId.Text = outWhId; lblWarehouseName.Text = outWhName;
            }

            // 4) Đổ dòng đã chọn vào lưới + set PurchaseInvoiceDetailId
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
                nr["Note"] = "Từ HĐ: " + _selectedPIId + " | Dòng: " + r["PIDetailId"].ToString();
                nr["PurchaseInvoiceDetailId"] = r["PIDetailId"].ToString();
                dtDetail.Rows.Add(nr);
            }

            dtDetail.AcceptChanges();
            Total();
        }

        // ====== GRID ======
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

                    dtgvDetail.Rows[e.RowIndex].Cells["TotalMoney"].Value =
                        string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalMoney);
                    dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value =
                        string.Format(CultureInfo.InvariantCulture, "{0:N0}", amount);
                    dtgvDetail.Rows[e.RowIndex].Cells["Price"].Value =
                        string.Format(CultureInfo.InvariantCulture, "{0:N0}", price);
                    Total();
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Lỗi định dạng: " + ex.Message);
                }
            }
            else if (dtgvDetail.Columns[e.ColumnIndex].Name == "ProductsId")
            {
                string productId = (dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value ?? "").ToString();
                string outId, outName;
                string key = " WHERE Id LIKE N'%" + productId + "%' OR Name LIKE N'%" + productId + "%'";
                Lookup.SearchLookupSingle(strQueryProduct, columnsProductText, widthProduct, productId, out outId, out outName, key);
                dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value = outId;
                dtgvDetail.Rows[e.RowIndex].Cells["ProductsName"].Value = outName;
            }
        }
        private void dtgvDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dtgvDetail.Columns[e.ColumnIndex].Name == "Amount" || dtgvDetail.Columns[e.ColumnIndex].Name == "Price")
            {
                string inputValue = e.FormattedValue.ToString().Trim();
                if (!string.IsNullOrEmpty(inputValue) && !System.Text.RegularExpressions.Regex.IsMatch(inputValue.Replace(",", ""), @"^\d+(\.\d+)?$"))
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

        // ====== SAVE ======
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWarehouseId.Text))
            { Alert.Error("Chưa nhập thông tin kho hàng"); txtWarehouseId.Focus(); return; }
            if (string.IsNullOrEmpty(txtSupplierId.Text))
            { Alert.Error("Chưa nhập thông tin nhà cung cấp"); txtSupplierId.Focus(); return; }

            // Lọc chi tiết rỗng hoặc mã SP không tồn tại
            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                var r = dtgvDetail.Rows[i];
                if (r.IsNewRow) { dtgvDetail.Rows.RemoveAt(i); continue; }

                if (r.Cells["ProductsId"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["ProductsId"].Value)) ||
                    r.Cells["Amount"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["Amount"].Value)) ||
                    r.Cells["Price"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["Price"].Value)))
                {
                    dtgvDetail.Rows.RemoveAt(i);
                }
                else
                {
                    bool ok = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id= '" + r.Cells["ProductsId"].Value.ToString() + "'");
                    if (!ok) dtgvDetail.Rows.RemoveAt(i);
                }
            }
            if (dtgvDetail.Rows.Count == 0) { Alert.Error("Chưa nhập thông tin chi tiết"); return; }

            // Không cho nhập vượt còn lại theo từng PIDetail
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                var drv = r.DataBoundItem as DataRowView;
                string pidDetail = null;
                if (drv != null && drv.Row.Table.Columns.Contains("PurchaseInvoiceDetailId"))
                    pidDetail = drv.Row["PurchaseInvoiceDetailId"] == DBNull.Value ? null : drv.Row["PurchaseInvoiceDetailId"].ToString();

                if (!string.IsNullOrEmpty(pidDetail))
                {
                    // SL đặt trong HĐ
                    var orderedStr = Models.SQL.GetValue(
                        "SELECT CONVERT(decimal(18,3), Qty) FROM PurchaseInvoiceDetail WHERE Id=@PID",
                        new object[,] { { "@PID", pidDetail } });
                    decimal ordered = string.IsNullOrEmpty(orderedStr) ? 0m : ToDec(orderedStr);

                    // Đã nhập trước đó (các PNK khác)
                    var receivedStr = Models.SQL.GetValue(
                        "SELECT CONVERT(decimal(18,3), ISNULL(SUM(Amount),0)) FROM ImportStockDetail WHERE PurchaseInvoiceDetailId=@PID",
                        new object[,] { { "@PID", pidDetail } });
                    decimal received = string.IsNullOrEmpty(receivedStr) ? 0m : ToDec(receivedStr);

                    // SL đang chuẩn bị nhập ở phiếu hiện tại (cộng các dòng cùng PIDetail)
                    decimal qtyInThisVoucher = 0m;
                    foreach (DataGridViewRow rr in dtgvDetail.Rows)
                    {
                        if (rr.IsNewRow) continue;
                        var drv2 = rr.DataBoundItem as DataRowView;
                        string pid2 = null;
                        if (drv2 != null && drv2.Row.Table.Columns.Contains("PurchaseInvoiceDetailId"))
                            pid2 = drv2.Row["PurchaseInvoiceDetailId"] == DBNull.Value ? null : drv2.Row["PurchaseInvoiceDetailId"].ToString();
                        if (pid2 == pidDetail)
                            qtyInThisVoucher += ToDec((rr.Cells["Amount"].Value ?? "0").ToString());
                    }

                    decimal remain = ordered - received;
                    if (qtyInThisVoucher > remain + 0.0001m)
                    {
                        string pname = (r.Cells["ProductsName"].Value ?? "").ToString();
                        Alert.Error($"Dòng HĐ đã vượt còn lại:\n- SP: {pname}\n- Đặt: {ordered:N3}, Đã nhập: {received:N3}, Còn: {remain:N3}\n- Bạn đang nhập: {qtyInThisVoucher:N3}");
                        return;
                    }
                }
            }


            // TÍNH TỔNG & CHUẨN HÓA
            Total();
            decimal totalQty = ToDec(txtTotalAmount.Text); // TotalAmount trong Import = tổng SL
            decimal totalAmt = ToDec(txtTotalMoney.Text);  // Tổng tiền

            if (frmImportStockMaster.isSave || frmImportStockMaster.isCopy)
            {
                string sqlIns =
                    @"INSERT INTO ImportStockMaster
                (Id, VoucherDate, WarehouseId, SupplierId, Note,
                 TotalAmount, TotalMoney, UserCreate, DateCreate, PurchaseInvoiceId)
              VALUES
                (@Id, @Date, @Wh, @Supp, @Note,
                 @TQty, @TMoney, @User, GETDATE(), @PI)";
                Models.SQL.RunQuery(sqlIns, new object[,]
                {
            {"@Id", txtId.Text.Trim()},
            {"@Date", dtpVoucherDate.Value},
            {"@Wh", txtWarehouseId.Text.Trim()},
            {"@Supp", txtSupplierId.Text.Trim()},
            {"@Note", txtNote.Text.Trim()},
            {"@TQty", totalQty},
            {"@TMoney", totalAmt},
            {"@User", frmLogin.UserName},
            {"@PI", string.IsNullOrEmpty(_selectedPIId) ? (object)DBNull.Value : _selectedPIId}
                });
            }
            else
            {
                string sqlUpd =
                    @"UPDATE ImportStockMaster
              SET VoucherDate=@Date, WarehouseId=@Wh, SupplierId=@Supp, Note=@Note,
                  TotalAmount=@TQty, TotalMoney=@TMoney,
                  UserUpdate=@User, DateUpdate=GETDATE(),
                  PurchaseInvoiceId=COALESCE(NULLIF(@PI, ''), PurchaseInvoiceId) 
              WHERE Id=@Id";
                Models.SQL.RunQuery(sqlUpd, new object[,]
                {
            {"@Id", txtId.Text.Trim()},
            {"@Date", dtpVoucherDate.Value},
            {"@Wh", txtWarehouseId.Text.Trim()},
            {"@Supp", txtSupplierId.Text.Trim()},
            {"@Note", txtNote.Text.Trim()},
            {"@TQty", totalQty},
            {"@TMoney", totalAmt},
            {"@User", frmLogin.UserName},
            {"@PI", string.IsNullOrEmpty(_selectedPIId) ? (object)DBNull.Value : _selectedPIId}
                });

                // Xóa chi tiết cũ để ghi lại
                Models.SQL.RunQuery("DELETE FROM ImportStockDetail WHERE ImportStockMasterId=@Id",
                    new object[,] { { "@Id", txtId.Text.Trim() } });

                // Xóa PostData cũ của phiếu này
                Models.SQL.RunQuery("DELETE FROM PostData WHERE VoucherId=@Id AND VoucherCode='PNK'",
                    new object[,] { { "@Id", txtId.Text.Trim() } });
            }

            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                string prod = (r.Cells["ProductsId"].Value ?? "").ToString();
                decimal qty = ToDec((r.Cells["Amount"].Value ?? "0").ToString());
                decimal price = ToDec((r.Cells["Price"].Value ?? "0").ToString());
                decimal sum = ToDec((r.Cells["TotalMoney"].Value ?? "0").ToString());
                string note = (r.Cells["Note"].Value ?? "").ToString();

                string insDet = @"
                        INSERT INTO ImportStockDetail
                            (Id, ImportStockMasterId, ProductsId, Amount, Price, TotalMoney, Note, PurchaseInvoiceDetailId)
                        VALUES
                            (@Id, @Mid, @Pid, @Qty, @Price, @Sum, @Note, @PIDetailId)";
                Models.SQL.RunQuery(insDet, new object[,]
                {
                        {"@Id",  Guid.NewGuid().ToString("N")},
                        {"@Mid", txtId.Text.Trim()},
                        {"@Pid", prod},
                        {"@Qty", qty},
                        {"@Price", price},
                        {"@Sum", sum},
                        {"@Note", note},
                        {"@PIDetailId",(r.DataBoundItem is DataRowView drv && drv.Row.Table.Columns.Contains("PurchaseInvoiceDetailId")) ? (object)(drv.Row["PurchaseInvoiceDetailId"] ?? DBNull.Value) : DBNull.Value }
                });

                string insPost = @"
                    INSERT INTO PostData
                      (VoucherId, VoucherCode, VoucherDate, WarehouseId, ProductsId, Amount, Price, TotalMoney)
                    VALUES
                      (@Vid, 'PNK', @Vdate, @Wh, @Pid, @Amt, @Price, @TMoney)";

                Models.SQL.RunQuery(insPost, new object[,] {
                      {"@Vid", txtId.Text.Trim()}, {"@Vdate", dtpVoucherDate.Value},
                      {"@Wh", txtWarehouseId.Text.Trim()},
                      {"@Pid", prod}, {"@Amt", qty}, {"@Price", price}, {"@TMoney", sum}
                    });

            }

            frmImportStockMaster.isLoad = true;
            Alert.Infor("Đã lưu phiếu nhập.");
            Close();
        }

        // ====== Helper ======
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
    }
}
