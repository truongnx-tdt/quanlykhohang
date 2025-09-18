using DocumentFormat.OpenXml.Wordprocessing;
using PMQuanLyHangTonKho.Lib;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmEditHoaDonBan : Form
    {
        // ====== DETAIL LOAD ======
        string strQueryDetail =
            @"SELECT a.ProductId, b.Name AS ProductsName,
                     a.Qty AS Quantity,
                     a.UnitPrice AS UnitPrice,
                     a.LineAmount AS LineAmount, a.Notes
              FROM SalesInvoiceDetail a
              INNER JOIN Products b ON a.ProductId = b.Id";

        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá", "Thành tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 110, 110, 120, 250 };
        DataTable dtDetail = new DataTable();

        // ====== LOOKUP ======
        string strQueryProduct =
            "SELECT CAST(0 AS BIT) AS xtag, Id, Name, ProductCategoryId, Unit, MinimumStock, MaximumStock, Description, ExpirationDate, Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng", "Tên hàng", "Mã nhóm", "ĐVT", "Tồn min", "Tồn max", "Mô tả", "HSD", "Trạng thái" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 100, 100, 100, 220, 120, 120 };

        string strQueryCustomer = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM Customer";
        string[] columnsCustomerText = new string[] { "Chọn", "Mã KH", "Tên KH" };
        int[] widthCustomer = new int[] { 60, 120, 250 };

        // PriceList bán
        string strQueryPriceList = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM PriceListMaster WHERE IsActive=1 AND Type='S'";
        string[] columnsPriceListText = new string[] { "Chọn", "Mã BG", "Tên BG" };
        int[] widthPriceList = new int[] { 60, 120, 280 };

        // Kho (nếu bạn cho chọn kho ở hóa đơn bán)
        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM Warehouse";
        string[] columnsWareText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new int[] { 60, 120, 240 };

        public frmEditHoaDonBan()
        {
            InitializeComponent();
            KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);

            Load += frm_Load;

            btnSave.Click += btnSave_Click;
            btnExit.Click += (s, e) => Close();

            btnChooseCustomer.Click += btnChooseCustomer_Click;
            btnChoosePriceList.Click += btnChoosePriceList_Click;
            btnChooseWarehouse.Click += btnChooseWarehouse_Click;

            dtgvDetail.CellEndEdit += dtgvDetail_CellEndEdit;
            dtgvDetail.CellValidating += dtgvDetail_CellValidating;
            dtgvDetail.KeyDown += dtgvDetail_KeyDown;
        }

        // ====== BIND ======
        public void SetValue(BindingSource bs)
        {
            txtId.DataBindings.Add("Text", bs, "Id");
            dtpVoucherDate.DataBindings.Add("Text", bs, "DocDate");
            txtCustomerId.DataBindings.Add("Text", bs, "CustomerId");
            txtWarehouseId.DataBindings.Add("Text", bs, "WarehouseId");
            txtPriceListId.DataBindings.Add("Text", bs, "PriceListId");
            chkRetail.DataBindings.Add("Checked", bs, "IsRetail");
            txtNote.DataBindings.Add("Text", bs, "Notes");
            txtTotalQuantity.DataBindings.Add("Text", bs, "TotalQty");
            txtTotalAmount.DataBindings.Add("Text", bs, "TotalMoney");
        }

        // ====== LOAD ======
        private void frm_Load(object sender, EventArgs e)
        {
            lblCustomerName.Text = !string.IsNullOrEmpty(txtCustomerId.Text) ? Models.SQL.GetValue("SELECT Name FROM Customer WHERE Id=@Id", new object[,] { { "@Id", txtCustomerId.Text } }) : "";
            lblPriceListName.Text = !string.IsNullOrEmpty(txtPriceListId.Text) ? Models.SQL.GetValue("SELECT Name FROM PriceListMaster WHERE Id=@Id", new object[,] { { "@Id", txtPriceListId.Text } }) : "";
            lblWarehouseName.Text = !string.IsNullOrEmpty(txtWarehouseId.Text) ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id=@Id", new object[,] { { "@Id", txtWarehouseId.Text } }) : "";

            if (frmHoaDonBan.isSave && !frmHoaDonBan.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("HDB");
                chkRetail.Checked = true; // mặc định bán lẻ
            }
            else if (frmHoaDonBan.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("HDB");
                FormatTotals();
            }

            txtId.ReadOnly = true;

            string key = " WHERE SIMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + key, columnsNameDetail, widthDetail, out dtDetail);

            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            dtgvDetail.Columns["LineAmount"].ReadOnly = true;
        }

        // ====== LOOKUPS ======
        private void btnChooseCustomer_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryCustomer, columnsCustomerText, widthCustomer, txtCustomerId.Text, out outId, out outName);
            txtCustomerId.Text = outId; lblCustomerName.Text = outName;
        }
        private void btnChoosePriceList_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryPriceList, columnsPriceListText, widthPriceList, txtPriceListId.Text, out outId, out outName);
            txtPriceListId.Text = outId; lblPriceListName.Text = outName;
        }
        private void btnChooseWarehouse_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, txtWarehouseId.Text, out outId, out outName);
            txtWarehouseId.Text = outId; lblWarehouseName.Text = outName;
        }

        // ====== GRID EVENTS ======
        private void dtgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var col = dtgvDetail.Columns[e.ColumnIndex].Name;

            if (col == "ProductId")
            {
                string input = (dtgvDetail.Rows[e.RowIndex].Cells["ProductId"].Value ?? "").ToString();
                string search = " WHERE Id LIKE N'%" + input + "%' OR Name LIKE N'%" + input + "%'";
                string outId, outName;
                Lookup.SearchLookupSingle(strQueryProduct, columnsProductText, widthProduct, input, out outId, out outName, search);
                dtgvDetail.Rows[e.RowIndex].Cells["ProductId"].Value = outId;
                dtgvDetail.Rows[e.RowIndex].Cells["ProductsName"].Value = outName;
                AutoFillPrice(e.RowIndex);
            }
            else if (col == "Quantity" || col == "UnitPrice")
            {
                string qtyStr = (dtgvDetail.Rows[e.RowIndex].Cells["Quantity"].Value ?? "0").ToString().Replace(",", "");
                string priceStr = (dtgvDetail.Rows[e.RowIndex].Cells["UnitPrice"].Value ?? "0").ToString().Replace(",", "");
                if (qtyStr == "") qtyStr = "0";
                if (priceStr == "") priceStr = "0";
                try
                {
                    decimal qty = Convert.ToDecimal(qtyStr);

                    if (col == "Quantity" && !string.IsNullOrEmpty(txtPriceListId.Text))
                    {
                        string prod = (dtgvDetail.Rows[e.RowIndex].Cells["ProductId"].Value ?? "").ToString();
                        if (!string.IsNullOrEmpty(prod))
                        {
                            string sqlGet =
                                @"DECLARE @q DECIMAL(18,3)=@Qty;
                                  SELECT TOP 1 UnitPrice
                                  FROM PriceListDetail
                                  WHERE PriceListId=@PL AND ProductId=@P
                                        AND @q >= FromQty AND (@q <= ISNULL(ToQty,@q))
                                  ORDER BY FromQty DESC";
                            var p = Models.SQL.GetValue(sqlGet, new object[,] {
                                {"@Qty", qty }, {"@PL", txtPriceListId.Text }, {"@P", prod }
                            });
                            if (!string.IsNullOrEmpty(p)) priceStr = p;
                        }
                    }

                    decimal price = Convert.ToDecimal(priceStr);
                    decimal line = qty * price;

                    dtgvDetail.Rows[e.RowIndex].Cells["Quantity"].Value = (qty);
                    dtgvDetail.Rows[e.RowIndex].Cells["UnitPrice"].Value = (price);
                    dtgvDetail.Rows[e.RowIndex].Cells["LineAmount"].Value = (line);
                    Total();
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Lỗi định dạng: " + ex.Message);
                }
            }
        }

        private void AutoFillPrice(int rowIndex)
        {
            if (string.IsNullOrEmpty(txtPriceListId.Text)) return;

            string prod = (dtgvDetail.Rows[rowIndex].Cells["ProductId"].Value ?? "").ToString();
            if (string.IsNullOrEmpty(prod)) return;

            decimal qty = 0m;
            var cellQty = dtgvDetail.Rows[rowIndex].Cells["Quantity"].Value;
            if (cellQty != null) decimal.TryParse(cellQty.ToString().Replace(",", ""), out qty);
            if (qty <= 0) qty = 1;

            string sqlGet =
                @"DECLARE @q DECIMAL(18,3) = @Qty;
                  SELECT TOP 1 UnitPrice
                  FROM PriceListDetail
                  WHERE PriceListId=@PL AND ProductId=@P
                        AND @q >= FromQty AND (@q <= ISNULL(ToQty,@q))
                  ORDER BY FromQty DESC";
            var priceStr = Models.SQL.GetValue(sqlGet, new object[,] { { "@Qty", qty }, { "@PL", txtPriceListId.Text }, { "@P", prod } });

            if (!string.IsNullOrEmpty(priceStr))
            {
                decimal price = Convert.ToDecimal(priceStr);
                dtgvDetail.Rows[rowIndex].Cells["UnitPrice"].Value = FormatN0(price);
                decimal total = price * qty;
                dtgvDetail.Rows[rowIndex].Cells["LineAmount"].Value = FormatN0(total);
                Total();
            }
        }

        private void dtgvDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = dtgvDetail.Columns[e.ColumnIndex].Name;
            if (col == "Quantity" || col == "UnitPrice")
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

        // ====== SAVE (query thuần) ======
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId.Text))
            { Alert.Error("Chưa chọn khách hàng"); txtCustomerId.Focus(); return; }

            // Lọc chi tiết
            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                var r = dtgvDetail.Rows[i];
                if (r.IsNewRow) { dtgvDetail.Rows.RemoveAt(i); continue; }

                if (r.Cells["ProductId"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["ProductId"].Value)) ||
                    r.Cells["Quantity"].Value == DBNull.Value ||
                    string.IsNullOrWhiteSpace(Convert.ToString(r.Cells["Quantity"].Value)))
                {
                    dtgvDetail.Rows.RemoveAt(i);
                }
                else
                {
                    bool ok = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id='" + r.Cells["ProductId"].Value.ToString() + "'");
                    if (!ok) dtgvDetail.Rows.RemoveAt(i);
                }
            }
            if (dtgvDetail.Rows.Count == 0) { Alert.Error("Chưa nhập chi tiết"); return; }

            // Tổng cộng
            Total();
            decimal totalQty = ParseDec(txtTotalQuantity.Text);
            decimal totalAmt = ParseDec(txtTotalAmount.Text);

            // MASTER
            if (frmHoaDonBan.isSave || frmHoaDonBan.isCopy)
            {
                string ins =
                    @"INSERT INTO SalesInvoiceMaster
                        (Id, DocNo, DocDate, CustomerId, WarehouseId, PriceListId, IsRetail, Status,
                         TotalQty, TotalMoney, Notes, UserCreate, DateCreate)
                      VALUES
                        (@Id, NULL, @Date, @Cus, @Wh, @PL, @Retail, 0,
                         @TQty, @TMoney, @Notes, @User, GETDATE())";
                Models.SQL.RunQuery(ins, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Date", dtpVoucherDate.Value},
                    {"@Cus", txtCustomerId.Text.Trim()},
                    {"@Wh",  string.IsNullOrEmpty(txtWarehouseId.Text) ? (object)DBNull.Value : txtWarehouseId.Text.Trim()},
                    {"@PL",  string.IsNullOrEmpty(txtPriceListId.Text)? (object)DBNull.Value : txtPriceListId.Text.Trim()},
                    {"@Retail", chkRetail.Checked ? 1 : 0},
                    {"@TQty", totalQty},
                    {"@TMoney", totalAmt},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", frmLogin.UserName}
                });
            }
            else
            {
                string upd =
                    @"UPDATE SalesInvoiceMaster
                      SET DocDate=@Date, CustomerId=@Cus, WarehouseId=@Wh, PriceListId=@PL, IsRetail=@Retail,
                          TotalQty=@TQty, TotalMoney=@TMoney, Notes=@Notes,
                          UserUpdate=@User, DateUpdate=GETDATE()
                      WHERE Id=@Id";
                Models.SQL.RunQuery(upd, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Date", dtpVoucherDate.Value},
                    {"@Cus", txtCustomerId.Text.Trim()},
                    {"@Wh",  string.IsNullOrEmpty(txtWarehouseId.Text) ? (object)DBNull.Value : txtWarehouseId.Text.Trim()},
                    {"@PL",  string.IsNullOrEmpty(txtPriceListId.Text)? (object)DBNull.Value : txtPriceListId.Text.Trim()},
                    {"@Retail", chkRetail.Checked ? 1 : 0},
                    {"@TQty", totalQty},
                    {"@TMoney", totalAmt},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", frmLogin.UserName}
                });

                // Xóa detail & PostData cũ
                Models.SQL.RunQuery("DELETE FROM SalesInvoiceDetail WHERE SIMasterId=@Id",
                    new object[,] { { "@Id", txtId.Text.Trim() } });
                Models.SQL.RunQuery("DELETE FROM PostData WHERE VoucherId=@Id AND VoucherCode='HDB'",
                    new object[,] { { "@Id", txtId.Text.Trim() } });
            }

            // DETAIL
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                string prod = (r.Cells["ProductId"].Value ?? "").ToString();
                decimal qty = ParseDec((r.Cells["Quantity"].Value ?? "0").ToString());
                decimal price = ParseDec((r.Cells["UnitPrice"].Value ?? "0").ToString());
                decimal line = ParseDec((r.Cells["LineAmount"].Value ?? "0").ToString());
                string note = (r.Cells["Notes"].Value ?? "").ToString();

                string insLine =
                    @"INSERT INTO SalesInvoiceDetail
                        (Id, SIMasterId, ProductId, Qty, UnitPrice, DiscountRate, TaxRate, LineAmount, Notes)
                      VALUES
                        (@Id, @Mid, @Pid, @Qty, @Price, 0, 0, @Line, @Note)";
                Models.SQL.RunQuery(insLine, new object[,]
                {
                    {"@Id",  Guid.NewGuid().ToString("N")},
                    {"@Mid", txtId.Text.Trim()},
                    {"@Pid", prod},
                    {"@Qty", qty},
                    {"@Price", price},
                    {"@Line", line},
                    {"@Note", note}
                });

                // Nếu bán lẻ => ghi PostData xuất kho (số âm)
                if (chkRetail.Checked)
                {
                    string insPost =
                        @"INSERT INTO PostData
                            (VoucherId, VoucherCode, VoucherDate, ProductsId, Amount, Price, TotalMoney)
                          VALUES
                            (@Vid, 'HDB', @Vdate, @Pid, @Amt, @Price, @TMoney)";
                    Models.SQL.RunQuery(insPost, new object[,]
                    {
                        {"@Vid",   txtId.Text.Trim()},
                        {"@Vdate", dtpVoucherDate.Value},
                        {"@Pid",   prod},
                        {"@Amt",  -qty},              // xuất: âm
                        {"@Price", price},
                        {"@TMoney",-line}
                    });
                }
            }

            frmHoaDonBan.isLoad = true;
            Alert.Infor("Đã lưu hóa đơn bán.");
            Close();
        }

        // ====== TÍNH TỔNG ======
        private void Total()
        {
            decimal sumQty = 0m, sumAmt = 0m;
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;
                sumQty += ParseDec((r.Cells["Quantity"].Value ?? "0").ToString());
                sumAmt += ParseDec((r.Cells["LineAmount"].Value ?? "0").ToString());
            }
            txtTotalQuantity.Text = FormatN0(sumQty);
            txtTotalAmount.Text = FormatN0(sumAmt);
        }
        private void FormatTotals()
        {
            txtTotalQuantity.Text = FormatN0(ParseDec(txtTotalQuantity.Text));
            txtTotalAmount.Text = FormatN0(ParseDec(txtTotalAmount.Text));
        }

        // ====== Helpers ======
        private static decimal ParseDec(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            decimal.TryParse(s.Replace(",", ""), out var d); return d;
        }
        private static string FormatN0(decimal v)
            => string.Format(CultureInfo.InvariantCulture, "{0:N0}", v);
    }
}
