using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditProducts : Form
    {
        string strQuery = "SELECT CAST(0 AS BIT) AS xtag,* FROM ProductCategory";
        string[] columnsName = new string[] { "Chọn", "Mã nhóm", "Tên nhóm","Mô tả" };
        int[] width = new int[] { 60, 120, 250 };
        public frmEditProducts()
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
            this.btnChooseCategory.Click += btnChooseCategory_Click;
            this.Load += frm_Load;
            txtMaximumStock.KeyPress += txtMaximumStock_KeyPress;
            txtMaximumStock.TextChanged += txtMaximumStock_TextChanged;
            txtMinimumStock.KeyPress += txtMinimumStock_KeyPress;
            txtMinimumStock.TextChanged += txtMinimumStock_TextChanged;
        }

        private void txtMinimumStock_TextChanged(object sender, EventArgs e)
        {
            string text = txtMinimumStock.Text;
            text = string.Join("", text.Where(c => char.IsDigit(c)));

            if (!string.IsNullOrEmpty(text))
            {
                long number;
                if (long.TryParse(text, out number))
                {
                    txtMinimumStock.Text = string.Format("{0:N0}", number);
                    txtMinimumStock.SelectionStart = txtMinimumStock.Text.Length;
                }
            }
        }

        private void txtMinimumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void txtMaximumStock_TextChanged(object sender, EventArgs e)
        {
            string text = txtMaximumStock.Text;
            text = string.Join("", text.Where(c => char.IsDigit(c)));

            if (!string.IsNullOrEmpty(text))
            {
                long number;
                if (long.TryParse(text, out number))
                {
                    txtMaximumStock.Text = string.Format("{0:N0}", number);
                    txtMaximumStock.SelectionStart = txtMaximumStock.Text.Length;
                }
            }
        }

        private void txtMaximumStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void frm_Load(object sender, EventArgs e)
        {
            lblProductCategoryName.Text = !string.IsNullOrEmpty(txtProductCategoryId.Text) ? Models.SQL.GetValue("SELECT Name FROM ProductCategory WHERE Id = '" + txtProductCategoryId.Text + "'") : "";
            if(frmProducts.isSave)
            {
                chkStatus.Checked = true;
            }    
        }

        private void btnChooseCategory_Click(object sender, EventArgs e)
        {
            string OutProductCategoryId, OutProductCategoryName;
            Lookup.SearchLookupSingle(strQuery, columnsName, width, txtProductCategoryId.Text, out OutProductCategoryId, out OutProductCategoryName);
            txtProductCategoryId.Text = OutProductCategoryId;
            lblProductCategoryName.Text = OutProductCategoryName;
        }

        public void SetValue(BindingSource binSource,Boolean isCopy = false)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            txtName.DataBindings.Add("Text", binSource, "Name");
            txtProductCategoryId.DataBindings.Add("Text", binSource, "ProductCategoryId");
            txtUnit.DataBindings.Add("Text", binSource, "Unit");
            txtMinimumStock.DataBindings.Add("Text", binSource, "MinimumStock");
            txtMaximumStock.DataBindings.Add("Text", binSource, "MaximumStock");
            txtDescription.DataBindings.Add("Text", binSource, "Description");
            dtpExpirationDate.DataBindings.Add("Text", binSource, "ExpirationDate");
            chkStatus.DataBindings.Add("Checked", binSource, "Status");
            if(!isCopy)
            {
                txtId.ReadOnly = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ListEdit.SaveOrUpdate(this, frmProducts.isSave, out frmProducts.isLoad,true);
        }
    }
}
