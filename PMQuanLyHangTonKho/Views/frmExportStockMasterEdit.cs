using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditExportStockMaster: Form
    {

        string strQueryDetail = "SELECT ProductsId,b.Name as ProductsName,CONVERT(VARCHAR, Amount) AS Amount,CONVERT(VARCHAR, Price) AS Price,CONVERT(VARCHAR, TotalMoney) AS TotalMoney,Note FROM ExportStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";
        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá xuất", "Tổng tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 120, 120, 120, 250 };
        System.Data.DataTable dtDetail = new System.Data.DataTable();

        string strQueryProduct = "SELECT CAST(0 AS BIT) AS xtag,Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Số lượng tồn tối thiểu", "Số lượng tồn tối đa", "Mô tả", "Hạn sử dụng", "Trạng thái sử dụng" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 120, 120, 120, 250, 120, 180 };

        string strQueryCustomer = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Customer";
        string[] columnsCustomerText = new string[] { "Chọn", "Mã kh", "Tên kh" };
        int[] widthCustomer = new int[] { 60, 120, 250 };

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
        }
        private void dtgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvDetail.Columns[e.ColumnIndex].Name == "Amount" || dtgvDetail.Columns[e.ColumnIndex].Name == "Price")
            {
                string amountValue = dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value.ToString().Replace(",", "");
                string priceValue = dtgvDetail.Rows[e.RowIndex].Cells["Price"].Value.ToString().Replace(",", "");
                if (amountValue == "")
                {
                    amountValue = "0";
                }
                if (priceValue == "")
                {
                    priceValue = "0";
                }
                try
                {
                    decimal amount = Convert.ToDecimal(amountValue);
                    decimal price = Convert.ToDecimal(priceValue);
                    decimal totalMoney = amount * price;
                    dtgvDetail.Rows[e.RowIndex].Cells["TotalMoney"].Value = totalMoney;

                    string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", amount);
                    string formattedPrice = string.Format(CultureInfo.InvariantCulture, "{0:N0}", price);
                    string formattedTotalMoney = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalMoney);

                    dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value = formattedAmount;
                    dtgvDetail.Rows[e.RowIndex].Cells["Price"].Value = formattedPrice;
                    dtgvDetail.Rows[e.RowIndex].Cells["TotalMoney"].Value = formattedTotalMoney;
                    Total();
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Lỗi định dạng: " + ex.Message);
                }
            }
            else if (dtgvDetail.Columns[e.ColumnIndex].Name == "ProductsId")
            {
                string productId = dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value.ToString();
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
                if (!string.IsNullOrEmpty(inputValue) && !IsValidAmount(inputValue))
                {
                    e.Cancel = true;
                }
            }
        }
        private void dtgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                dtDetail.Rows.Add(dtDetail.NewRow());
            }
            if (e.KeyCode == Keys.F8)
            {
                if (dtgvDetail.SelectedRows.Count > 0)
                {
                    DataGridViewRow selectedRow = dtgvDetail.SelectedRows[0];
                    dtDetail.Rows.RemoveAt(selectedRow.Index);
                }
                else
                {
                    Alert.Error("Vui lòng chọn một dòng để xóa!");
                }
            }
        }
        private void frm_Load(object sender, EventArgs e)
        {
            lblCustomerName.Text = !string.IsNullOrEmpty(txtCustomerId.Text) ? Models.SQL.GetValue("SELECT Name FROM Customer WHERE Id = '" + txtCustomerId.Text + "'") : "";
            if (frmExportStockMaster.isSave && !frmExportStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PXK");
            }
            else if (!frmExportStockMaster.isSave || frmExportStockMaster.isCopy)
            {
                string formattedTotalMoney = string.Format(CultureInfo.InvariantCulture, "{0:N0}", Convert.ToDecimal(txtTotalMoney.Text));
                string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", Convert.ToDecimal(txtTotalAmount.Text));
                txtTotalAmount.Text = formattedAmount;
                txtTotalMoney.Text = formattedTotalMoney;
            }
            txtId.ReadOnly = true;
            string KeySearch = " WHERE ExportStockMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + KeySearch, columnsNameDetail, widthDetail, out dtDetail);
            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            dtgvDetail.Columns["TotalMoney"].ReadOnly = true;
            if (frmExportStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PXK");
            }
        }
        private void btnChooseCustomer_Click(object sender, EventArgs e)
        {
            string OutCustomerId, OutCustomerName;
            Lookup.SearchLookupSingle(strQueryCustomer, columnsCustomerText, widthCustomer, txtCustomerId.Text, out OutCustomerId, out OutCustomerName);
            txtCustomerId.Text = OutCustomerId;
            lblCustomerName.Text = OutCustomerName;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerId.Text))
            {
                Alert.Error("Chưa nhập thông tin khách hàng");
                txtCustomerId.Focus();
                return;
            }
            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                if (dtgvDetail.Rows[i].Cells["ProductsId"].Value == DBNull.Value || string.IsNullOrEmpty(dtgvDetail.Rows[i].Cells["ProductsId"].Value.ToString()) || dtgvDetail.Rows[i].Cells["Amount"].Value == DBNull.Value || string.IsNullOrEmpty(dtgvDetail.Rows[i].Cells["Amount"].Value.ToString()) || dtgvDetail.Rows[i].Cells["Price"].Value == DBNull.Value || string.IsNullOrEmpty(dtgvDetail.Rows[i].Cells["Price"].Value.ToString()))
                {
                    dtgvDetail.Rows.RemoveAt(i);
                }
                else
                {
                    Boolean isCheck = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id = '" + dtgvDetail.Rows[i].Cells["ProductsId"].Value.ToString() + "'");
                    if (!isCheck)
                    {
                        dtgvDetail.Rows.RemoveAt(i);
                    }
                }
            }
            if (dtgvDetail.Rows.Count == 0)
            {
                Alert.Error("Chưa nhập thông tin chi tiết");
                return;
            }
            ListEdit.SaveOrUpdate(this, frmExportStockMaster.isSave, out frmExportStockMaster.isLoad, true);
            ListEdit.SaveOrUpdateDetail(dtgvDetail, "ExportStockDetail", "ExportStockMasterId", txtId.Text);
            ListEdit.SavePostData("ExportStockDetail", "ExportStockMaster", txtId.Text, "PXK", dtpVoucherDate.Value, "-");
            this.Close();
        }

        private bool IsValidAmount(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d+$");
        }
        private void Total()
        {
            decimal totalMoney = 0;
            decimal totalAmount = 0;
            for (int i = 0; i < dtgvDetail.Rows.Count; i++)
            {
                if (dtgvDetail.Rows[i].Cells["Amount"].Value.ToString() != "")
                {
                    totalMoney += Convert.ToDecimal(dtgvDetail.Rows[i].Cells["TotalMoney"].Value.ToString().Replace(",", ""));
                }
                if (dtgvDetail.Rows[i].Cells["Amount"].Value.ToString() != "")
                {
                    totalAmount += Convert.ToDecimal(dtgvDetail.Rows[i].Cells["Amount"].Value.ToString().Replace(",", ""));
                }
            }
            string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalAmount);
            string formattedTotalMoney = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalMoney);
            txtTotalAmount.Text = formattedAmount.ToString();
            txtTotalMoney.Text = formattedTotalMoney.ToString();
        }
    }
}
