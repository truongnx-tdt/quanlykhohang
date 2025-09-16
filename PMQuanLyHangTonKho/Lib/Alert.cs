using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public class Alert
    {
        public static void Error(string Content)
        {
            MessageBox.Show(Content, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Question(string Content, out Boolean isOK)
        {
            isOK = false;
            DialogResult result = MessageBox.Show(Content, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                isOK = true;
            }
            else if (result == DialogResult.No)
            {
                isOK = false;
            }
        }
        public static void Infor(string Content)
        {
            MessageBox.Show(Content, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
