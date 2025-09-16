using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditSupplier : Form
    {
        public frmEditSupplier()
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
            this.Load += frm_Load;
        }
        private void frm_Load(object sender, EventArgs e)
        {
        }

        public void SetValue(BindingSource binSource,Boolean isCopy = false)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            txtName.DataBindings.Add("Text", binSource, "Name");
            txtContactName.DataBindings.Add("Text", binSource, "ContactName");
            txtContactTitle.DataBindings.Add("Text", binSource, "ContactTitle");
            txtAddress.DataBindings.Add("Text", binSource, "Address");
            txtPhone.DataBindings.Add("Text", binSource, "Phone");
            txtEmail.DataBindings.Add("Text", binSource, "Email");
            txtWebsite.DataBindings.Add("Text", binSource, "Website");
            if (!isCopy)
            {
                txtId.ReadOnly = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ListEdit.SaveOrUpdate(this, frmSupplier.isSave, out frmSupplier.isLoad);
        }
    }
}
