using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Interop.Excel;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditImportStockMaster: Form
    {
        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Warehouse";
        string[] columnsWareText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new int[] { 60, 120, 250 };

        string strQueryDetail = "SELECT ProductsId,b.Name as ProductsName,CONVERT(VARCHAR, Amount) AS Amount,CONVERT(VARCHAR, Price) AS Price,CONVERT(VARCHAR, TotalMoney) AS TotalMoney,Note FROM ImportStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";
        string[] columnsNameDetail = new string[] {"Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá nhập", "Tổng tiền", "Ghi chú" };
        int[] widthDetail = new int[] {120, 250, 120, 120, 120, 250 };
        System.Data.DataTable dtDetail = new System.Data.DataTable();

        string strQueryProduct = "SELECT CAST(0 AS BIT) AS xtag,Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn","Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Số lượng tồn tối thiểu", "Số lượng tồn tối đa", "Mô tả", "Hạn sử dụng", "Trạng thái sử dụng"};
        int[] widthProduct = new int[] {60, 120, 250, 120, 120, 120, 120, 250, 120, 180 };

        string strQuerySupplier = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Supplier";
        string[] columnsSupplierText = new string[] { "Chọn", "Mã ncc", "Tên ncc" };
        int[] widthSupplier = new int[] { 60, 120, 250 };

        public frmEditImportStockMaster()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
            this.btnChooseWarehouse.Click += btnChooseWarehouse_Click;
            this.btnChooseSupplier.Click += btnChooseSupplier_Click;
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
            lblWarehouseName.Text = !string.IsNullOrEmpty(txtWarehouseId.Text) ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id = '" + txtWarehouseId.Text + "'") : "";
            lblSupplierName.Text = !string.IsNullOrEmpty(txtSupplierId.Text) ? Models.SQL.GetValue("SELECT Name FROM Supplier WHERE Id = '" + txtSupplierId.Text + "'") : "";
            if (frmImportStockMaster.isSave && !frmImportStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PNK");
            }
            else if(!frmImportStockMaster.isSave || frmImportStockMaster.isCopy)
            {
                string formattedTotalMoney = string.Format(CultureInfo.InvariantCulture, "{0:N0}", Convert.ToDecimal(txtTotalMoney.Text));
                string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", Convert.ToDecimal(txtTotalAmount.Text));
                txtTotalAmount.Text = formattedAmount;
                txtTotalMoney.Text = formattedTotalMoney;
            }
            txtId.ReadOnly = true;
            string KeySearch = " WHERE ImportStockMasterId = '"+txtId.Text+"'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + KeySearch, columnsNameDetail, widthDetail, out dtDetail);
            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            dtgvDetail.Columns["TotalMoney"].ReadOnly = true;
            if(frmImportStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PNK");
            }    
        }

        private void btnChooseWarehouse_Click(object sender, EventArgs e)
        {
            string OutWarehouseId, OutWarehouseName;
            Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, txtWarehouseId.Text, out OutWarehouseId, out OutWarehouseName);
            txtWarehouseId.Text = OutWarehouseId;
            lblWarehouseName.Text = OutWarehouseName;
        }
        private void btnChooseSupplier_Click(object sender, EventArgs e)
        {
            string OutSupplierId, OutSupplierName;
            Lookup.SearchLookupSingle(strQuerySupplier, columnsSupplierText, widthSupplier, txtSupplierId.Text, out OutSupplierId, out OutSupplierName);
            txtSupplierId.Text = OutSupplierId;
            lblSupplierName.Text = OutSupplierName;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtWarehouseId.Text))
            {
                Alert.Error("Chưa nhập thông tin kho hàng");
                txtWarehouseId.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtSupplierId.Text))
            {
                Alert.Error("Chưa nhập thông tin nhà cung cấp");
                txtSupplierId.Focus();
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
                    Boolean isCheck = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id = '"+ dtgvDetail.Rows[i].Cells["ProductsId"].Value.ToString() + "'");
                    if(!isCheck)
                    {
                        dtgvDetail.Rows.RemoveAt(i);
                    }    
                }    
            }
            if(dtgvDetail.Rows.Count == 0)
            {
                Alert.Error("Chưa nhập thông tin chi tiết");
                return;
            }    
            ListEdit.SaveOrUpdate(this, frmImportStockMaster.isSave, out frmImportStockMaster.isLoad, true);
            ListEdit.SaveOrUpdateDetail(dtgvDetail, "ImportStockDetail", "ImportStockMasterId", txtId.Text);
            ListEdit.SavePostData("ImportStockDetail", "ImportStockMaster", txtId.Text,"PNK", dtpVoucherDate.Value, "+");
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
            txtTotalMoney.Text  = formattedTotalMoney.ToString();
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }
    }
}
