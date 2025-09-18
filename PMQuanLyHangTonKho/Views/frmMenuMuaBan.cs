using PMQuanLyHangTonKho.Views.muaban;
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
    public partial class frmMenuMuaBan : Form
    {
        public frmMenuMuaBan()
        {
            InitializeComponent();
        }

        private void menuBangGia_Click(object sender, EventArgs e)
        {
            frmBangGia frm = new frmBangGia();
            frm.ShowDialog();
        }

        private void menuHoaDonMua_Click(object sender, EventArgs e)
        {
            frmHoaDonMua frm = new frmHoaDonMua();
            frm.ShowDialog();
        }

        private void menuHoaDonBan_Click(object sender, EventArgs e)
        {
            frmHoaDonBan f = new frmHoaDonBan();
            f.ShowDialog();
        }

        private void menuBaoCao_Click(object sender, EventArgs e)
        {

        }
    }
}
