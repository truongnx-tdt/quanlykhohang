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
    public partial class frmEditTakingStockMaster: Form
    {
        string strQueryDetail = "SELECT ProductsId,b.Name as ProductsName,CONVERT(VARCHAR, Amount) AS Amount,Note FROM TakingStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";
        string[] columnsNameDetail = new string[] { "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 250, 120, 250 };
        System.Data.DataTable dtDetail = new System.Data.DataTable();

        string strQueryProduct = "SELECT CAST(0 AS BIT) AS xtag,Id,Name,ProductCategoryId,Unit,MinimumStock,MaximumStock,Description,ExpirationDate,Status FROM Products";
        string[] columnsProductText = new string[] { "Chọn", "Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Số lượng tồn tối thiểu", "Số lượng tồn tối đa", "Mô tả", "Hạn sử dụng", "Trạng thái sử dụng" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 120, 120, 120, 250, 120, 180 };

        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Warehouse";
        string[] columnsWareText = new string[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new int[] { 60, 120, 250 };
        
        string strQueryEmployeesId1 = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Employees";
        string[] columnsEmployeesId1Text = new string[] { "Chọn", "Mã nhân viên", "Tên nhân viên" };
        int[] widthEmployeesId1 = new int[] { 60, 120, 250 };

        string strQueryEmployeesId2 = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Employees";
        string[] columnsEmployeesId2Text = new string[] { "Chọn", "Mã nhân viên", "Tên nhân viên" };
        int[] widthEmployeesId2 = new int[] { 60, 120, 250 };

        string strQueryEmployeesId3 = "SELECT CAST(0 AS BIT) AS xtag,Id,Name FROM Employees";
        string[] columnsEmployeesId3Text = new string[] { "Chọn", "Mã nhân viên", "Tên nhân viên" };
        int[] widthEmployeesId3 = new int[] { 60, 120, 250 };

        public frmEditTakingStockMaster()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
            this.btnChooseWarehouse.Click += btnChooseWarehouseId_Click;
            this.btnChooseEmployeesId1.Click += btnChooseEmployeesId1_Click;
            this.btnChooseEmployeesId2.Click += btnChooseEmployeesId2_Click;
            this.btnChooseEmployeesId3.Click += btnChooseEmployeesId3_Click;
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
            lblWarehouseName.Text = !string.IsNullOrEmpty(txtWarehouseId.Text) ? Models.SQL.GetValue("SELECT Name FROM Warehouse WHERE Id = '" + txtWarehouseId.Text + "'") : "";
            lblEmployeesId1.Text = !string.IsNullOrEmpty(txtEmployeesId1.Text) ? Models.SQL.GetValue("SELECT Name FROM Employees WHERE Id = '" + txtEmployeesId1.Text + "'") : "";
            lblEmployeesId2.Text = !string.IsNullOrEmpty(txtEmployeesId2.Text) ? Models.SQL.GetValue("SELECT Name FROM Employees WHERE Id = '" + txtEmployeesId2.Text + "'") : "";
            lblEmployeesId3.Text = !string.IsNullOrEmpty(txtEmployeesId3.Text) ? Models.SQL.GetValue("SELECT Name FROM Employees WHERE Id = '" + txtEmployeesId3.Text + "'") : "";
            if (frmTakingStockMaster.isSave && !frmTakingStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PKK");
            }
            else if (!frmTakingStockMaster.isSave || frmTakingStockMaster.isCopy)
            {
                string formattedAmount = string.Format(CultureInfo.InvariantCulture, "{0:N0}", Convert.ToDecimal(txtTotalAmount.Text));
                txtTotalAmount.Text = formattedAmount;
            }
            txtId.ReadOnly = true;
            string KeySearch = " WHERE TakingStockMasterId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + KeySearch, columnsNameDetail, widthDetail, out dtDetail);
            dtgvDetail.Columns["ProductsName"].ReadOnly = true;
            if (frmTakingStockMaster.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("PKK");
            }
        }

        private void btnChooseWarehouseId_Click(object sender, EventArgs e)
        {
            string OutWarehouseId, OutWarehouseName;
            Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare, txtWarehouseId.Text, out OutWarehouseId, out OutWarehouseName);
            txtWarehouseId.Text = OutWarehouseId;
            lblWarehouseName.Text = OutWarehouseName;
        }
        private void btnChooseEmployeesId1_Click(object sender, EventArgs e)
        {
            string OutEmployeesId, OutEmployeesName;
            Lookup.SearchLookupSingle(strQueryEmployeesId1, columnsEmployeesId1Text, widthEmployeesId1, txtEmployeesId1.Text, out OutEmployeesId, out OutEmployeesName);
            txtEmployeesId1.Text = OutEmployeesId;
            lblEmployeesId1.Text = OutEmployeesName;
        }
        private void btnChooseEmployeesId2_Click(object sender, EventArgs e)
        {
            string OutEmployeesId, OutEmployeesName;
            Lookup.SearchLookupSingle(strQueryEmployeesId2, columnsEmployeesId2Text, widthEmployeesId2, txtEmployeesId2.Text, out OutEmployeesId, out OutEmployeesName);
            txtEmployeesId2.Text = OutEmployeesId;
            lblEmployeesId2.Text = OutEmployeesName;
        }
        private void btnChooseEmployeesId3_Click(object sender, EventArgs e)
        {
            string OutEmployeesId, OutEmployeesName;
            Lookup.SearchLookupSingle(strQueryEmployeesId3, columnsEmployeesId3Text, widthEmployeesId3, txtEmployeesId3.Text, out OutEmployeesId, out OutEmployeesName);
            txtEmployeesId3.Text = OutEmployeesId;
            lblEmployeesId3.Text = OutEmployeesName;
        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            dtpVoucherDate.DataBindings.Add("Text", binSource, "VoucherDate");
            txtWarehouseId.DataBindings.Add("Text", binSource, "WarehouseId");
            txtEmployeesId1.DataBindings.Add("Text", binSource, "EmployeesId1");
            txtEmployeesId2.DataBindings.Add("Text", binSource, "EmployeesId2");
            txtEmployeesId3.DataBindings.Add("Text", binSource, "EmployeesId3");
            txtNote.DataBindings.Add("Text", binSource, "Note");
            txtTotalAmount.DataBindings.Add("Text", binSource, "TotalAmount");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtWarehouseId.Text))
            {
                Alert.Error("Chưa nhập thông tin kho");
                txtWarehouseId.Focus();
                return;
            }
            if (string.IsNullOrEmpty(txtEmployeesId1.Text))
            {
                Alert.Error("Chưa nhập thông tin người kiểm kê 1");
                txtEmployeesId1.Focus();
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
            ListEdit.SaveOrUpdate(this, frmTakingStockMaster.isSave, out frmTakingStockMaster.isLoad, true);
            ListEdit.SaveOrUpdateDetail(dtgvDetail, "TakingStockDetail", "TakingStockMasterId", txtId.Text, true);
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
