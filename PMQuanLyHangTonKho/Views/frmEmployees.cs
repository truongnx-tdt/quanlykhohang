using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEmployees : Form
    {
        string strQuery = "SELECT Id,Name,DateOfBirth,CASE WHEN Gender = '1' THEN 'Nam' ELSE N'Nữ' end as Gender,Phone,Email,PermanentAddress,TemporaryAddress,HireDate,Position,UserCreate,DateCreate,UserUpdate,DateUpdate FROM Employees";
        string[] columnsValues = new string[] { "Id", "Name", "Phone", "Email", "PermanentAddress", "TemporaryAddress", "Position" };
        string[] columnsName = new string[] { "Mã nhân viên", "Tên nhân viên", "Ngày sinh", "Giới tính", "Điện thoại","Email","Địa chỉ thường trú","Địa chỉ tạm trú","Ngày vào làm","Chức vụ","Người tạo", "Ngày tạo", "Người sửa","Ngày sửa" };
        string[] columnsSearch = new string[] { "Mã nhân viên", "Tên nhân viên", "Điện thoại", "Email", "Địa chỉ thường trú", "Địa chỉ tạm trú","Chức vụ" };
        int[] width = new int[] { 120, 250, 120, 120,120,120, 250, 250,120,150,120,120,120,120 };
        public static Boolean isSave = true;
        public static Boolean isLoad = false;
        public static Boolean isDelete = false;
        public frmEmployees()
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
            this.menuXuatExcel.Click += menuXuatExcel_Click;
            this.menuCopy.Click += menuCopy_Click;
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if (dtgvMain.RowCount > 0)
            {
                isSave = true;
                frmEditEmployees frm = new frmEditEmployees();
                frm.Text = "Copy";
                frm.SetValue(dtgvMain.Rows[dtgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString(),false);
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

        private void menuXuatExcel_Click(object sender, EventArgs e)
        {
            DLLSystem.ExportExcel(dtgvMain);
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
                ListEdit.Delete("Employees", "Id", dtgvMain.Rows[dtgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());
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
            ListEdit.LoadFormSearchList(columnsValues, columnsSearch, new Point(300, 300), out KeySearch);
            LoadData(KeySearch);
        }

        private void menuThem_Click(object sender, EventArgs e)
        {
            isSave = true;
            frmEditEmployees frm = new frmEditEmployees();
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
                frmEditEmployees frm = new frmEditEmployees();
                frm.Text = "Sửa";
                frm.SetValue(dtgvMain.Rows[dtgvMain.CurrentCell.RowIndex].Cells[0].Value.ToString());
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
