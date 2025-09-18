using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditExportStockMaster : Form
    {
        string strQueryDetail =
            "SELECT ProductsId,b.Name as ProductsName,CONVERT(VARCHAR, Amount) AS Amount," +
            "CONVERT(VARCHAR, Price) AS Price,CONVERT(VARCHAR, TotalMoney) AS TotalMoney,Note " +
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
                       m.Id       AS SIMasterId,
                       d.Id       AS SIDetailId,
                       m.DocDate,
                       m.CustomerId,
                       d.ProductId,
                       p.Name     AS ProductName,
                       d.Qty      AS OrderedQty,
                       ISNULL((
                           SELECT SUM(ed.Amount)
                           FROM ExportStockDetail ed
                           JOIN ExportStockMaster em ON em.Id = ed.ExportStockMasterId
                           WHERE em.SalesInvoiceId       = m.Id
                             AND ed.SalesInvoiceDetailId = d.Id
                       ),0)        AS ShippedQty,
                       (d.Qty - ISNULL((
                           SELECT SUM(ed.Amount)
                           FROM ExportStockDetail ed
                           JOIN ExportStockMaster em ON em.Id = ed.ExportStockMasterId
                           WHERE em.SalesInvoiceId       = m.Id
                             AND ed.SalesInvoiceDetailId = d.Id
                       ),0))       AS RemainQty,
                       d.UnitPrice
                FROM SalesInvoiceDetail d
                JOIN SalesInvoiceMaster m ON m.Id = d.SIMasterId AND m.IsRetail = 0
                JOIN Products p          ON p.Id = d.ProductId
                WHERE (d.Qty - ISNULL((
                           SELECT SUM(ed.Amount)
                           FROM ExportStockDetail ed
                           JOIN ExportStockMaster em ON em.Id = ed.ExportStockMasterId
                           WHERE em.SalesInvoiceId       = m.Id
                             AND ed.SalesInvoiceDetailId = d.Id
                       ),0)) > 0
                ORDER BY m.Id, d.Id";
        string[] columnsSIRemainText = {
            "Chọn","Mã HĐ","Mã dòng","Ngày HĐ","Mã KH","Mã SP","Tên SP",
            "SL HĐ","Đã xuất","Còn lại","Đơn giá"
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

            // NÚT mới: lấy từ HĐ bán (đặt tên button là btnLayTuHoaDonBan)
            this.btnLayTuHoaDonBan.Click += btnLayTuHoaDonBan_Click;
        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            dtpVoucherDate.DataBindings.Add("Text", binSource, "VoucherDate");
            txtCustomerId.DataBindings.Add("Text", binSource, "CustomerId");
            txtNote.DataBindings.Add("Text", binSource, "Note");
            txtTotalAmount.DataBindings.Add("Text", binSource, "TotalAmount");
            txtTotalMoney.DataBindings.Add("Text", binSource, "TotalMoney");
        }

        private void frm_Load(object sender, EventArgs e)
        {
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

        // ===== LẤY TỪ HÓA ĐƠN BÁN =====
        private void btnLayTuHoaDonBan_Click(object sender, EventArgs e)
        {
            while (true)
            {
                string outId, outText;
                // Dùng single-pick để lấy 1 dòng (SIDetailId)
                Lookup.SearchLookupSingle(
                    strQuerySIRemainingDetail,
                    columnsSIRemainText, widthSIRemain,
                    "", out outId, out outText);
                if (string.IsNullOrEmpty(outId)) break;   // người dùng bấm Thoát

                // Lấy lại dòng vừa chọn
                var dtPicked = Models.SQL.GetData(
                    "SELECT TOP 1 * FROM (" + strQuerySIRemainingDetail + ") X WHERE SIDetailId='" + outId + "'");
                if (dtPicked.Rows.Count == 0) continue;

                var r = dtPicked.Rows[0];
                string siId = r["SIMasterId"].ToString();
                string cus = r["CustomerId"].ToString();

                // Lần đầu chọn -> set Customer & SalesInvoice
                if (string.IsNullOrEmpty(_selectedSIId))
                {
                    _selectedSIId = siId;
                    txtCustomerId.Text = cus;
                    lblCustomerName.Text = Models.SQL.GetValue("SELECT Name FROM Customer WHERE Id=@Id",
                        new object[,] { { "@Id", cus } });
                    dtpVoucherDate.Value = Convert.ToDateTime(r["DocDate"]);
                    txtNote.Text = "Tạo từ HĐ bán: " + _selectedSIId;
                }
                // Ràng buộc cùng HĐ
                if (_selectedSIId != siId) { Alert.Error("Mỗi PXK chỉ chọn dòng của 1 HĐ bán."); continue; }

                decimal remain = ToDec(r["RemainQty"].ToString());
                decimal price = ToDec(r["UnitPrice"].ToString());
                decimal sum = remain * price;

                var nr = dtDetail.NewRow();
                nr["ProductsId"] = r["ProductId"].ToString();
                nr["ProductsName"] = r["ProductName"].ToString();
                nr["Amount"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", ToDec(r["RemainQty"].ToString()));
                nr["Price"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}", ToDec(r["UnitPrice"].ToString()));
                nr["TotalMoney"] = string.Format(CultureInfo.InvariantCulture, "{0:N0}",
                                                   ToDec(r["RemainQty"].ToString()) * ToDec(r["UnitPrice"].ToString()));
                nr["Note"] = "Từ HĐ: " + _selectedSIId + " | Dòng: " + r["SIDetailId"].ToString();
                nr["SalesInvoiceDetailId"] = r["SIDetailId"].ToString();
                dtDetail.Rows.Add(nr);

                dtDetail.AcceptChanges();
                Total();
            }
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
                decimal onHand = GetOnHand(pid, dtpVoucherDate.Value); // tổng nhập - xuất tới ngày
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

            // Tổng
            Total();
            decimal totalQty = ToDec(txtTotalAmount.Text);
            decimal totalAmt = ToDec(txtTotalMoney.Text);

            // MASTER
            if (frmExportStockMaster.isSave || frmExportStockMaster.isCopy)
            {
                string sqlIns =
                    @"INSERT INTO ExportStockMaster
                        (Id, VoucherDate, CustomerId, Note, TotalAmount, TotalMoney,
                         UserCreate, DateCreate, SalesInvoiceId)
                      VALUES
                        (@Id, @Date, @Cus, @Note, @TQty, @TMoney,
                         @User, GETDATE(), @SI)";
                Models.SQL.RunQuery(sqlIns, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Date", dtpVoucherDate.Value},
                    {"@Cus", txtCustomerId.Text.Trim()},
                    {"@Note", txtNote.Text.Trim()},
                    {"@TQty", totalQty},
                    {"@TMoney", totalAmt},
                    {"@User", frmLogin.UserName},
                    {"@SI", string.IsNullOrEmpty(_selectedSIId) ? (object)DBNull.Value : _selectedSIId}
                });
            }
            else
            {
                string sqlUpd =
                    @"UPDATE ExportStockMaster
                      SET VoucherDate=@Date, CustomerId=@Cus, Note=@Note,
                          TotalAmount=@TQty, TotalMoney=@TMoney,
                          UserUpdate=@User, DateUpdate=GETDATE(),
                          SalesInvoiceId=COALESCE(NULLIF(@SI,''), SalesInvoiceId)
                      WHERE Id=@Id";
                Models.SQL.RunQuery(sqlUpd, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Date", dtpVoucherDate.Value},
                    {"@Cus", txtCustomerId.Text.Trim()},
                    {"@Note", txtNote.Text.Trim()},
                    {"@TQty", totalQty},
                    {"@TMoney", totalAmt},
                    {"@User", frmLogin.UserName},
                    {"@SI", string.IsNullOrEmpty(_selectedSIId) ? (object)DBNull.Value : _selectedSIId}
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
                if (dtDetail.Columns.Contains("SalesInvoiceDetailId"))
                {
                    var v = dtDetail.Rows[r.Index]["SalesInvoiceDetailId"];
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
                                        (VoucherId, VoucherCode, VoucherDate, ProductsId, Amount, Price, TotalMoney)
                                    VALUES
                                        (@Vid, 'PXK', @Vdate, @Pid, @Amt, @Price, @TMoney)";
                Models.SQL.RunQuery(insPost, new object[,]
                {
                                    {"@Vid",   txtId.Text.Trim()},
                                    {"@Vdate", dtpVoucherDate.Value},
                                    {"@Pid",   prod},
                                    {"@Amt",  -qty},
                                    {"@Price", price},
                                    {"@TMoney",-sum}
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

        // Tồn kho đến ngày chứng từ (tổng hợp PostData)
        private decimal GetOnHand(string productId, DateTime toDate)
        {
            var val = Models.SQL.GetValue(
                @"SELECT CONVERT(decimal(18,3),ISNULL(SUM(Amount),0))
                  FROM PostData
                  WHERE ProductsId=@P AND VoucherDate<=@D",
                new object[,] { { "@P", productId }, { "@D", toDate } });
            return string.IsNullOrEmpty(val) ? 0m : ToDec(val);
        }
    }
}
