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
    public partial class frmProductCategory : Form
    {
        string strQuery = "SELECT * FROM ProductCategory";
        string[] columnsValues = new string[] { "Id", "Name", "Description" };
        string[] columnsName = new string[] { "Mã nhóm", "Tên nhóm", "Mô tả" };
        string[] columnsSearch = new string[] { "Mã nhóm", "Tên nhóm", "Mô tả" };
        int[] width = new int[] { 120, 250, 250 };
        public static Boolean isSave = true;
        public static Boolean isLoad = false;
        public static Boolean isDelete = false;
        public frmProductCategory()
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
                ListEdit.Delete("ProductCategory", "Id", dtgvMain.Rows[dtgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());
                LoadData();
            }
        }

        private void frm_Load(object sender, EventArgs e)
        {
            LoadData();
            DLLSystem.RoleMenu(this, menuStrip, menuThem, menuSua, menuXoa, menuXuatExcel);
        }
        public void LoadData(string KeySearch = null)
        {
            Lib.CssDatagridview.LoadData(binSource, dtgvMain, strQuery + KeySearch ?? "", columnsName, width);
        }
        private void menuTimKiem_Click(object sender, EventArgs e)
        {
            string KeySearch = "";
            ListEdit.LoadFormSearchList(columnsValues, columnsSearch, new Point(300, 180),out KeySearch);
            LoadData(KeySearch);
        }

        private void menuThem_Click(object sender, EventArgs e)
        {
            isSave = true;
            frmEditProductCategory frm = new frmEditProductCategory();
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
                frmEditProductCategory frm = new frmEditProductCategory();
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
