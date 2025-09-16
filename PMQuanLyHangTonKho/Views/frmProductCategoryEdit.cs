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
    public partial class frmEditProductCategory : Form
    {
        public frmEditProductCategory()
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
            txtDescription.DataBindings.Add("Text", binSource, "Description");
            txtId.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ListEdit.SaveOrUpdate(this, frmProductCategory.isSave, out frmProductCategory.isLoad);
        }
    }
}
