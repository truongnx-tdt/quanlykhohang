using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditImportStockMaster : Form
    {
        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Warehouse";
        string[] columnsWareText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new int[] { 60, 120, 250 };

        string strQueryDetail =
            "SELECT ProductsId,b.Name as ProductsName,CONVERT(VARCHAR, Amount) AS Amount," +
            "CONVERT(VARCHAR, Price) AS Price,CONVERT(VARCHAR, TotalMoney) AS TotalMoney,Note " +
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

        // Lookup HĐ mua CHƯA nhập kho
        string strQueryPIUnreceived =
            @"SELECT CAST(0 AS BIT) AS xtag, a.Id, a.DocDate, a.SupplierId, a.TotalQty, a.TotalMoney
              FROM PurchaseInvoiceMaster a
              WHERE NOT EXISTS (SELECT 1 FROM ImportStockMaster m WHERE m.PurchaseInvoiceId = a.Id)";
        string[] columnsPIText = new string[] { "Chọn", "Mã HĐ", "Ngày HĐ", "NCC", "Tổng SL", "Tổng tiền" };
        int[] widthPI = new int[] { 60, 120, 120, 140, 100, 120 };

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
        private void btnLayTuHoaDonMua_Click(object sender, EventArgs e)
        {
            string outId, outText;
            Lookup.SearchLookupSingle(strQueryPIUnreceived, columnsPIText, widthPI, "", out outId, out outText);
            if (string.IsNullOrEmpty(outId)) return;

            _selectedPIId = outId;

            // lấy header HĐ
            var dtPI = Models.SQL.GetData(
                @"SELECT DocDate, SupplierId, WarehouseId, Notes, TotalQty, TotalMoney
                  FROM PurchaseInvoiceMaster WHERE Id= '" + _selectedPIId + "'");

            if (dtPI.Rows.Count == 0) { Alert.Error("Không tìm thấy hóa đơn mua."); return; }
            var h = dtPI.Rows[0];

            // Supplier
            txtSupplierId.Text = h["SupplierId"].ToString();
            lblSupplierName.Text = Models.SQL.GetValue("SELECT Name FROM Supplier WHERE Id=@Id",
                new object[,] { { "@Id", txtSupplierId.Text } });

            // Date
            dtpVoucherDate.Value = Convert.ToDateTime(h["DocDate"]);

            // Warehouse: nếu HĐ chưa có kho thì bắt chọn
            string wh = h["WarehouseId"] == DBNull.Value ? "" : h["WarehouseId"].ToString();
            if (string.IsNullOrEmpty(wh))
            {
                string outWhId, outWhName;
                Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, "", out outWhId, out outWhName);
                if (string.IsNullOrEmpty(outWhId)) { Alert.Error("Chưa chọn kho nhập"); return; }
                wh = outWhId; lblWarehouseName.Text = outWhName;
            }
            txtWarehouseId.Text = wh;
            if (string.IsNullOrEmpty(lblWarehouseName.Text))
                lblWarehouseName.Text = Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id=@Id", new object[,] { { "@Id", wh } });

            // Note
            txtNote.Text = "Tạo từ HĐ mua: " + _selectedPIId;

            // Đổ chi tiết từ HĐ
            var dtLines = Models.SQL.GetData(
                @"SELECT a.ProductId, b.Name AS ProductsName, a.Qty, a.UnitPrice, a.LineAmount, a.Notes
                  FROM PurchaseInvoiceDetail a INNER JOIN Products b ON a.ProductId=b.Id
                  WHERE a.PIMasterId= '" + _selectedPIId + "'");

            dtDetail.Rows.Clear();
            foreach (DataRow r in dtLines.Rows)
            {
                var nr = dtDetail.NewRow();
                nr["ProductsId"] = r["ProductId"].ToString();
                nr["ProductsName"] = r["ProductsName"].ToString();
                nr["Amount"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", r["Qty"]);
                nr["Price"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", r["UnitPrice"]);
                nr["TotalMoney"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", r["LineAmount"]);
                nr["Note"] = r["Notes"]?.ToString() ?? "";
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

                string insDet =
                    @"INSERT INTO ImportStockDetail
                (Id, ImportStockMasterId, ProductsId, Amount, Price, TotalMoney, Note)
              VALUES
                (@Id, @Mid, @Pid, @Qty, @Price, @Sum, @Note)";
                Models.SQL.RunQuery(insDet, new object[,]
                {
            {"@Id",  Guid.NewGuid().ToString("N")},
            {"@Mid", txtId.Text.Trim()},
            {"@Pid", prod},
            {"@Qty", qty},
            {"@Price", price},
            {"@Sum", sum},
            {"@Note", note}
                });

                string insPost =
                    @"INSERT INTO PostData
                (VoucherId, VoucherCode, VoucherDate, ProductsId, Amount, Price, TotalMoney)
              VALUES
                (@Vid, 'PNK', @Vdate, @Pid, @Amt, @Price, @TMoney)";
                Models.SQL.RunQuery(insPost, new object[,]
                {
            {"@Vid", txtId.Text.Trim()},
            {"@Vdate", dtpVoucherDate.Value},
            {"@Pid", prod},
            {"@Amt", qty},
            {"@Price", price},
            {"@TMoney", sum}
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
