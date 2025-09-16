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
    public partial class frmCustomer : Form
    {
        string strQuery = "SELECT * FROM Customer";
        string[] columnsValues = new string[] { "Id", "Name", "Address", "Phone", "Email" };
        string[] columnsName = new string[] { "Mã khách hàng", "Tên khách hàng", "Địa chỉ", "Điện thoại","Email" };
        string[] columnsSearch = new string[] { "Mã khách hàng", "Tên khách hàng", "Địa chỉ", "Điện thoại", "Email" };
        int[] width = new int[] { 120, 250, 250, 120,120 };
        public static Boolean isSave = true;
        public static Boolean isLoad = false;
        public static Boolean isDelete = false;
        public frmCustomer()
        {
            InitializeComponent();
            DLLSystem.Init(this);
            this.Load += frm_Load;
            this.menuTimKiem.Click += menuTimKiem_Click;
            this.menuThem.Click += menuThem_Click;
            this.menuSua.Click += menuSua_Click;
            this.menuLamMoi.Click += menuLamMoi_Click;
            this.menuXoa.Click += menuXoa_Click;
            this.menuThoat.Click += menuThoat_Click;
            this.menuCopy.Click += menuCopy_Click;
            this.menuXuatExcel.Click += menuXuatExcel_Click;
        }

        private void menuXuatExcel_Click(object sender, EventArgs e)
        {
            DLLSystem.ExportExcel(dtgvMain);
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if (dtgvMain.RowCount > 0)
            {
                isSave = true;
                frmEditCustomer frm = new frmEditCustomer();
                frm.Text = "Copy";
                frm.SetValue(binSource,true);
                frm.ShowDialog();
                if (isLoad)
                {
                    LoadData();
                }
            }
            else
            {
                Alert.Error("Chưa có dữ liệu để Copy");
            }
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuXoa_Click(object sender, EventArgs e)
        {
            if (dtgvMain.Rows.Count == 0)
            {
                Alert.Error("Chưa có dữ liệu để xóa");
                return;
            }
            Alert.Question("Bạn có chắc chắn xóa dòng dữ liệu này không ?", out isDelete);
            if (isDelete)
            {
                ListEdit.Delete("Customer", "Id", dtgvMain.Rows[dtgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());
                LoadData();
            }
        }

        private void frm_Load(object sender, EventArgs e)
        {
            LoadData();
            DLLSystem.RoleMenu(this, menuStrip, menuThem, menuSua, menuXoa, menuXuatExcel,menuCopy);
        }
        public void LoadData(string KeySearch = null)
        {
            Lib.CssDatagridview.LoadData(binSource, dtgvMain, strQuery + KeySearch ?? "", columnsName, width);
        }
        private void menuTimKiem_Click(object sender, EventArgs e)
        {
            string KeySearch = "";
            ListEdit.LoadFormSearchList(columnsValues, columnsSearch, new Point(300, 240),out KeySearch);
            LoadData(KeySearch);
        }

        private void menuThem_Click(object sender, EventArgs e)
        {
            isSave = true;
            frmEditCustomer frm = new frmEditCustomer();
            frm.Text = "Thêm mới";
            frm.ShowDialog();
            if (isLoad)
            {
                LoadData();
            }
        }

        private void menuSua_Click(object sender, EventArgs e)
        {
            if (dtgvMain.RowCount > 0)
            {
                isSave = false;
                frmEditCustomer frm = new frmEditCustomer();
                frm.Text = "Sửa";
                frm.SetValue(binSource);
                frm.ShowDialog();
                if (isLoad)
                {
                    LoadData();
                }
            }
            else
            {
                Alert.Error("Chưa có dữ liệu để sửa");
            }
        }

        private void menuLamMoi_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
