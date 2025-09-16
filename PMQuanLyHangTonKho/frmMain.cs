using Microsoft.Office.Interop.Excel;
using PMQuanLyHangTonKho.Lib;
using PMQuanLyHangTonKho.Views;
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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            this.Region = new Region(GetRoundedRectangle(this.ClientRectangle, 15));
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblInfor.Text = "SQL : "+Models.SQL.ServerName+" | Data : "+Models.SQL.Data+" | Người sử dụng : "+frmLogin.UserName+" | Ngày : "+DateTime.Now.ToString("dd/MM/yyyy")+"";
        }
        private GraphicsPath GetRoundedRectangle(System.Drawing.Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90); // Góc trên trái
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90); // Góc trên phải
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90); // Góc dưới phải
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90); // Góc dưới trái
            path.CloseAllFigures();
            return path;
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void menuTaiKhoan_Click(object sender, EventArgs e)
        {
            if(frmLogin.Admin == "False")
            {
                Alert.Error("Chưa được phân quyền để sử dụng chức năng này");
                return;
            }
            frmPassLogin frm = new frmPassLogin();
            frm.ShowDialog();
        }

        private void menuNhanVien_Click(object sender, EventArgs e)
        {
            Boolean isCheck =  DLLSystem.RoleForm("frmEmployees");
            if (!isCheck) { return; }
            frmEmployees frm = new frmEmployees();
            frm.ShowDialog();
        }

        private void menuKho_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmWarehouse");
            if (!isCheck) { return; }
            frmWarehouse frm = new frmWarehouse();
            frm.ShowDialog();
        }

        private void menuNhomHH_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmProductCategory");
            if (!isCheck) { return; }
            frmProductCategory frm = new frmProductCategory();
            frm.ShowDialog();
        }

        private void menuHangHoa_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmProducts");
            if (!isCheck) { return; }
            frmProducts frm = new frmProducts();
            frm.ShowDialog();
        }

        private void menuNhaCungCap_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmSupplier");
            if (!isCheck) { return; }
            frmSupplier frm = new frmSupplier();
            frm.ShowDialog();
        }

        private void menuKhachHang_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmCustomer");
            if (!isCheck) { return; }
            frmCustomer frm = new frmCustomer();
            frm.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Alert.Question("Bạn có chắc chắn thoát không ?", out Boolean isExit);
            if (isExit)
            {
                this.Close();
            }
        }

        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void menuNhapKho_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmImportStockMaster");
            if (!isCheck) { return; }
            frmImportStockMaster frm = new frmImportStockMaster();
            frm.ShowDialog();
        }

        private void menuXuatKho_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmExportStockMaster");
            if (!isCheck) { return; }
            frmExportStockMaster frm = new frmExportStockMaster();
            frm.ShowDialog();
        }

        private void menuDieuChuyen_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmTransferStockMaster");
            if (!isCheck) { return; }
            frmTransferStockMaster frm = new frmTransferStockMaster();
            frm.ShowDialog();
        }

        private void menuKiemKho_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmTakingStockMaster");
            if (!isCheck) { return; }
            frmTakingStockMaster frm = new frmTakingStockMaster();
            frm.ShowDialog();
        }

        private void menuThongKe_Click(object sender, EventArgs e)
        {
            Boolean isCheck = DLLSystem.RoleForm("frmReportInventory");
            if (!isCheck) { return; }
            frmReportInventory frm = new frmReportInventory();
            frm.ShowDialog();
        }

        private void menuMuaBan_Click(object sender, EventArgs e)
        {
            frmMenuMuaBan frm = new frmMenuMuaBan();
            frm.ShowDialog();
        }
    }
}
