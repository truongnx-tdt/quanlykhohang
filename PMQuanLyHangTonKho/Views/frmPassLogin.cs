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
    public partial class frmPassLogin : Form
    {
        public frmPassLogin()
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            btnSave.Size = new Size(66, 35);
        }

        private void frmPassLogin_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (frmLogin.Password != txtPassword.Text)
            {
                Alert.Error("Mật khẩu không đúng");
                return;
            }
            this.Hide();
            frmUsers frm = new frmUsers();
            frm.ShowDialog();
            this.Close();
        }
    }
}
