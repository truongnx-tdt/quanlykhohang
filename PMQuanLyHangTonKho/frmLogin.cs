using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho
{
    public partial class frmLogin : Form
    {
        public static string UserId;
        public static string UserName;
        public static string Password;
        public static string Admin;
        public frmLogin()
        {
            InitializeComponent();
            this.Region = new Region(GetRoundedRectangle(this.ClientRectangle, 15));
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Alert.Question("Bạn có chắc chắn thoát không ?", out Boolean isExit);
            if (isExit)
            {
                this.Close();
            }
        }
        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90); // Góc trên trái
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90); // Góc trên phải
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90); // Góc dưới phải
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90); // Góc dưới trái
            path.CloseAllFigures();
            return path;
        }

        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control is TextBox && control.Tag != null && control.Tag.ToString().Contains("NB") && string.IsNullOrEmpty(control.Text))
                {
                    Alert.Error("Trường dữ liệu bắt buộc nhập");
                    control.Focus();
                    return;
                }
            }
            DataTable dt = new DataTable();
            dt = Models.SQL.GetData($@"SELECT * FROM Users WHERE Id = '{txtId.Text}' AND Password = '{txtPassWord.Text}'");
            if (dt.Rows.Count == 0)
            {
                Alert.Error("Thông tin đăng nhập không đúng");
                return;
            }
            UserId = dt.Rows[0]["Id"].ToString() + ",";
            UserName = dt.Rows[0]["Name"].ToString();
            Password = dt.Rows[0]["Password"].ToString();
            Admin = dt.Rows[0]["Admin"].ToString();
            this.Hide();
            frmMain frm = new frmMain();
            frm.ShowDialog();
            this.Close();
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPass.Checked == true)
            {
                txtPassWord.PasswordChar = '\0';
            }
            else
            {
                txtPassWord.PasswordChar = '*';
            }
        }
    }
}
