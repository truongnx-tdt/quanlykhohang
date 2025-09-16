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
    public partial class frmEditUsers : Form
    {
        public frmEditUsers()
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

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            txtName.DataBindings.Add("Text", binSource, "Name");
            txtPassword.DataBindings.Add("Text", binSource, "Password");
            txtRePassword.DataBindings.Add("Text", binSource, "Password");
            chkAdmin.DataBindings.Add("Checked", binSource, "Admin");
            txtJobPosition.DataBindings.Add("Text", binSource, "JobPosition");
            txtPhone.DataBindings.Add("Text", binSource, "Phone");
            txtId.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtPassword.Text != txtRePassword.Text)
            {
                Alert.Error("Nhập lại mật khẩu không đúng");
                txtRePassword.Focus();
                return;
            } 
            ListEdit.SaveOrUpdate(this, frmUsers.isSave, out frmUsers.isLoad);
        }
    }
}
