using PMQuanLyHangTonKho.Lib;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmEditHoaDonMua : Form
    {
        string strQueryDetail =
            "SELECT a.ProductId, b.Name AS ProductsName, " +
            "       a.Qty        AS Quantity, " +
            "       a.UnitPrice  AS UnitPrice, " +
            "       a.LineAmount AS LineAmount, " +
            "       a.Notes " +
            "FROM PurchaseInvoiceDetail a " +
            "INNER JOIN Products b ON a.ProductId = b.Id";

        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá", "Thành tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 110, 110, 120, 250 };
        DataTable dtDetail = new DataTable();

        string strQueryProduct =
            "SELECT CAST(0 AS BIT) AS xtag, Id, Name, ProductCategoryId, Unit, MinimumStock, MaximumStock, Description, ExpirationDate, Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng", "Tên hàng", "Mã nhóm", "ĐVT", "Tồn min", "Tồn max", "Mô tả", "HSD", "Trạng thái" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 100, 100, 100, 220, 120, 120 };

        string strQuerySupplier = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM Supplier";
        string[] columnsSupplierText = new string[] { "Chọn", "Mã NCC", "Tên NCC" };
        int[] widthSupplier = new int[] { 60, 120, 250 };

        string strQueryPriceList = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM PriceListMaster WHERE IsActive=1 AND Type='P'";
        string[] columnsPriceListText = new string[] { "Chọn", "Mã BG", "Tên BG" };
        int[] widthPriceList = new int[] { 60, 120, 280 };

        public frmEditHoaDonMua()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);

            this.Load += frm_Load;
            btnSave.Click += btnSave_Click;
            btnExit.Click += (s, e) => Close();

            btnChooseSupplier.Click += btnChooseSupplier_Click;
            btnChoosePriceList.Click += btnChoosePriceList_Click;

            dtgvDetail.CellEndEdit += dtgvDetail_CellEndEdit;
            dtgvDetail.CellValidating += dtgvDetail_CellValidating;
            dtgvDetail.KeyDown += dtgvDetail_KeyDown;
        }

        public void SetValue(BindingSource bs)
        {
            txtId.DataBindings.Add("Text", bs, "Id");
            dtpVoucherDate.DataBindings.Add("Text", bs, "DocDate");
            txtSupplierId.DataBindings.Add("Text", bs, "SupplierId");
            txtPriceListId.DataBindings.Add("Text", bs, "PriceListId");
            txtNote.DataBindings.Add("Text", bs, "Notes");
            txtTotalQuantity.DataBindings.Add("Text", bs, "TotalQty");
            txtTotalAmount.DataBindings.Add("Text", bs, "TotalMoney");
        }

        private void frm_Load(object sender, EventArgs e)
        {
            lblSupplierName.Text = !string.IsNullOrEmpty(txtSupplierId.Text)
                ? Models.SQL.GetValue("SELECT Name FROM Supplier WHERE Id=@Id", new object[,] { { "@Id", txtSupplierId.Text } })
                : "";

            lblPriceListName.Text = !string.IsNullOrEmpty(txtPriceListId.Text)
                ? Models.SQL.GetValue("SELECT Name FROM PriceListMaster WHERE Id=@Id", new object[,] { { "@Id", txtPriceListId.Text } })
                : "";

            if (frmHoaDonMua.isSave && !frmHoaDonMua.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("HDM");
            }
            else if (frmHoaDonMua.isCopy)
            {
                FormatTotals();
                txtId.Text = DLLSystem.GenCode("HDM");
            }

            txtId.ReadOnly = true;

            string key = " WHERE PIMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + key, columnsNameDetail, widthDetail, out dtDetail);

            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            dtgvDetail.Columns["LineAmount"].ReadOnly = true; // thành tiền tự tính
        }

        private void btnChooseSupplier_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQuerySupplier, columnsSupplierText, widthSupplier, txtSupplierId.Text, out outId, out outName);
            txtSupplierId.Text = outId; lblSupplierName.Text = outName;
        }

        private void btnChoosePriceList_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryPriceList, columnsPriceListText, widthPriceList, txtPriceListId.Text, out outId, out outName);
            txtPriceListId.Text = outId; lblPriceListName.Text = outName;
        }

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
                if (string.IsNullOrWhiteSpace(qtyStr)) qtyStr = "0";
                if (string.IsNullOrWhiteSpace(priceStr)) priceStr = "0";

                try
                {
                    decimal qty = Convert.ToDecimal(qtyStr);

                    if (col == "Quantity" && !string.IsNullOrEmpty(txtPriceListId.Text))
                    {
                        string prod = (dtgvDetail.Rows[e.RowIndex].Cells["ProductId"].Value ?? "").ToString();
                        if (!string.IsNullOrEmpty(prod))
                        {
                            string sqlGet =
                                @"DECLARE @q DECIMAL(18,3) = @Qty;
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

                    decimal price = ParseDec(priceStr.ToString());
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSupplierId.Text))
            {
                Alert.Error("Chưa chọn nhà cung cấp"); txtSupplierId.Focus(); return;
            }

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
                    bool ok = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id= '" + r.Cells["ProductId"].Value.ToString() + "'");
                    if (!ok) dtgvDetail.Rows.RemoveAt(i);
                }
            }
            if (dtgvDetail.Rows.Count == 0) { Alert.Error("Chưa nhập chi tiết"); return; }

            Total();

            decimal totalQty = ParseDec(txtTotalQuantity.Text);
            decimal totalAmt = ParseDec(txtTotalAmount.Text);

            if (frmHoaDonMua.isSave || frmHoaDonMua.isCopy)
            {
                string ins =
                    @"INSERT INTO PurchaseInvoiceMaster
                        (Id, DocNo, DocDate, SupplierId, WarehouseId, PriceListId, Status,
                         TotalQty, TotalMoney, Notes, UserCreate, DateCreate)
                      VALUES
                        (@Id, NULL, @Date, @Supp, NULL, @PL, 0,
                         @TQty, @TMoney, @Notes, @User, GETDATE())";
                Models.SQL.RunQuery(ins, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Date", dtpVoucherDate.Value },
                    {"@Supp", txtSupplierId.Text.Trim()},
                    {"@PL", string.IsNullOrEmpty(txtPriceListId.Text)? (object)DBNull.Value : txtPriceListId.Text.Trim()},
                    {"@TQty", totalQty},
                    {"@TMoney", totalAmt},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", frmLogin.UserName}
                });
            }
            else
            {
                string upd =
                    @"UPDATE PurchaseInvoiceMaster
                      SET DocDate=@Date, SupplierId=@Supp, PriceListId=@PL,
                          TotalQty=@TQty, TotalMoney=@TMoney, Notes=@Notes,
                          UserUpdate=@User, DateUpdate=GETDATE()
                      WHERE Id=@Id";
                Models.SQL.RunQuery(upd, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Date", dtpVoucherDate.Value },
                    {"@Supp", txtSupplierId.Text.Trim()},
                    {"@PL", string.IsNullOrEmpty(txtPriceListId.Text)? (object)DBNull.Value : txtPriceListId.Text.Trim()},
                    {"@TQty", totalQty},
                    {"@TMoney", totalAmt},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", frmLogin.UserName}
                });

                Models.SQL.RunQuery("DELETE FROM PurchaseInvoiceDetail WHERE PIMasterId=@Id",
                    new object[,] { { "@Id", txtId.Text.Trim() } });
            }

            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                string prod = (r.Cells["ProductId"].Value ?? "").ToString();
                decimal qty = ParseDec((r.Cells["Quantity"].Value ?? "0").ToString());
                decimal price = ParseDec((r.Cells["UnitPrice"].Value ?? "0").ToString());
                decimal line = ParseDec((r.Cells["LineAmount"].Value ?? "0").ToString());
                string note = (r.Cells["Notes"].Value ?? "").ToString();

                string insLine =
                    @"INSERT INTO PurchaseInvoiceDetail
                        (Id, PIMasterId, ProductId, Qty, UnitPrice,
                         DiscountRate, TaxRate, LineAmount, Notes)
                      VALUES
                        (@Id, @Mid, @Pid, @Qty, @Price,
                         0, 0, @Line, @Note)";
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
            }

            frmHoaDonMua.isLoad = true;
            Alert.Infor("Đã lưu hóa đơn mua.");
            Close();
        }

        private void Total()
        {
            decimal sumQty = 0m, sumMoney = 0m;
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                if (r.IsNewRow) continue;

                sumQty += ParseDec((r.Cells["Quantity"].Value ?? "0").ToString());
                sumMoney += ParseDec((r.Cells["LineAmount"].Value ?? "0").ToString());
            }
            txtTotalQuantity.Text = FormatN0(sumQty);
            txtTotalAmount.Text = FormatN0(sumMoney);
        }

        private void FormatTotals()
        {
            txtTotalQuantity.Text = FormatN0(ParseDec((txtTotalQuantity.Text ?? "0").ToString()));
            txtTotalAmount.Text = FormatN0(ParseDec((txtTotalAmount.Text ?? "0").ToString()));
        }

        private static decimal ParseDec(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return 0m;
            decimal.TryParse(s.Replace(",", ""), out var d);
            return d;
        }
        private static string FormatN0(decimal? v, string fallback = "")
            => v.HasValue ? v.Value.ToString("N0", CultureInfo.CurrentCulture) : fallback;
    }
}
