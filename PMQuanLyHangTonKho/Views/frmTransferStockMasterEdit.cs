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
    public partial class frmEditTransferStockMaster: Form
    {
        string strQueryDetail = "SELECT ProductsId,b.Name as ProductsName,CONVERT(VARCHAR, Amount) AS Amount,Note FROM TransferStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";
        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 120, 250 };
        System.Data.DataTable dtDetail = new System.Data.DataTable();

        string strQueryProduct = "SELECT CAST(0 AS BIT) AS xtag,Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Số lượng tồn tối thiểu", "Số lượng tồn tối đa", "Mô tả", "Hạn sử dụng", "Trạng thái sử dụng" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 120, 120, 120, 250, 120, 180 };

        string strQueryWareTo = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Warehouse";
        string[] columnsWareToText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWareTo = new int[] { 60, 120, 250 };

        string strQueryWareFrom = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Warehouse";
        string[] columnsWareFromText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWareFrom = new int[] { 60, 120, 250 };

        public frmEditTransferStockMaster()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
            this.btnChooseWarehouseIdFrom.Click += btnChooseWarehouseIdFrom_Click;
            this.btnChooseWarehouseIdTo.Click += btnChooseWarehouseIdTo_Click;
            this.Load += frm_Load;
            this.dtgvDetail.CellEndEdit += dtgvDetail_CellEndEdit;
            this.dtgvDetail.CellValidating += dtgvDetail_CellValidating;
            this.dtgvDetail.KeyDown += dtgvDetail_KeyDown;
        }
        private void dtgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgvDetail.Columns[e.ColumnIndex].Name == "Amount")
            {
                string amountValue = dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value.ToString().Replace(",", "");
                if (amountValue == "")
                {
                    amountValue = "0";
                }
                try
                {
                    decimal amount = Convert.ToDecimal(amountValue);
                    string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", amount);
                    dtgvDetail.Rows[e.RowIndex].Cells["Amount"].Value = formattedAmount;
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
            if (dtgvDetail.Columns[e.ColumnIndex].Name == "Amount")
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
            lblWarehouseNameFrom.Text = !string.IsNullOrEmpty(txtWarehouseIdFrom.Text) ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id = '" + txtWarehouseIdFrom.Text + "'") : "";
            lblWarehouseNameTo.Text = !string.IsNullOrEmpty(txtWarehouseIdTo.Text) ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id = '" + txtWarehouseIdTo.Text + "'") : "";
            if (frmTransferStockMaster.isSave && !frmTransferStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PDC");
            }
            else if (!frmTransferStockMaster.isSave || frmTransferStockMaster.isCopy)
            {
                string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", Convert.ToDecimal(txtTotalAmount.Text));
                txtTotalAmount.Text = formattedAmount;
            }
            txtId.ReadOnly = true;
            string KeySearch = " WHERE TransferStockMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + KeySearch, columnsNameDetail, widthDetail, out dtDetail);
            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            if (frmTransferStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PDC");
            }
        }

        private void btnChooseWarehouseIdTo_Click(object sender, EventArgs e)
        {
            string OutWarehouseIdTo, OutWarehouseNameTo;
            Lookup.SearchLookupSingle(strQueryWareTo, columnsWareToText, widthWareTo, txtWarehouseIdTo.Text, out OutWarehouseIdTo, out OutWarehouseNameTo);
            txtWarehouseIdTo.Text = OutWarehouseIdTo;
            lblWarehouseNameTo.Text = OutWarehouseNameTo;
        }
        private void btnChooseWarehouseIdFrom_Click(object sender, EventArgs e)
        {
            string OutWarehouseIdFrom, OutWarehouseNameFrom;
            Lookup.SearchLookupSingle(strQueryWareFrom, columnsWareFromText, widthWareFrom, txtWarehouseIdFrom.Text, out OutWarehouseIdFrom, out OutWarehouseNameFrom);
            txtWarehouseIdFrom.Text = OutWarehouseIdFrom;
            lblWarehouseNameFrom.Text = OutWarehouseNameFrom;
        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            dtpVoucherDate.DataBindings.Add("Text", binSource, "VoucherDate");
            txtWarehouseIdFrom.DataBindings.Add("Text", binSource, "WarehouseIdFrom");
            txtWarehouseIdTo.DataBindings.Add("Text", binSource, "WarehouseIdTo");
            txtNote.DataBindings.Add("Text", binSource, "Note");
            txtTotalAmount.DataBindings.Add("Text", binSource, "TotalAmount");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWarehouseIdFrom.Text))
            {
                Alert.Error("Chưa nhập thông tin kho đi");
                txtWarehouseIdFrom.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtWarehouseIdTo.Text))
            {
                Alert.Error("Chưa nhập thông tin kho đến");
                txtWarehouseIdTo.Focus();
                return;
            }
            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                if (dtgvDetail.Rows[i].Cells["ProductsId"].Value == DBNull.Value || string.IsNullOrEmpty(dtgvDetail.Rows[i].Cells["ProductsId"].Value.ToString()) || dtgvDetail.Rows[i].Cells["Amount"].Value == DBNull.Value || string.IsNullOrEmpty(dtgvDetail.Rows[i].Cells["Amount"].Value.ToString()))
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
            ListEdit.SaveOrUpdate(this, frmTransferStockMaster.isSave, out frmTransferStockMaster.isLoad, true);
            ListEdit.SaveOrUpdateDetail(dtgvDetail, "TransferStockDetail", "TransferStockMasterId", txtId.Text,true);
            this.Close();
        }

        private bool IsValidAmount(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d+$");
        }
        private void Total()
        {
            decimal totalAmount = 0;
            for (int i = 0; i < dtgvDetail.Rows.Count; i++)
            {
                if (dtgvDetail.Rows[i].Cells["Amount"].Value.ToString() != "")
                {
                    totalAmount += Convert.ToDecimal(dtgvDetail.Rows[i].Cells["Amount"].Value.ToString().Replace(",", ""));
                }
            }
            string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", totalAmount);
            txtTotalAmount.Text = formattedAmount.ToString();
        }
    }
}
