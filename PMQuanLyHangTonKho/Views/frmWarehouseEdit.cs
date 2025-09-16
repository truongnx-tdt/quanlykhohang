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
    public partial class frmEditWarehouse : Form
    {
        public frmEditWarehouse()
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
            this.Load += frm_Load;
        }

        private void frm_Load(object sender, EventArgs e)
        {
            if(frmWarehouse.isSave)
            {
                chkStatus.Checked = true;
            }    
        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            txtName.DataBindings.Add("Text", binSource, "Name");
            txtLocation.DataBindings.Add("Text", binSource, "Location");
            chkStatus.DataBindings.Add("Checked", binSource, "Status");
            txtId.ReadOnly = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ListEdit.SaveOrUpdate(this, frmWarehouse.isSave, out frmWarehouse.isLoad,true);
        }
    }
}
