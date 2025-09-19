using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PMQuanLyHangTonKho.Lib;
using PMQuanLyHangTonKho.Views.Reports;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmExportStockMaster : Form
    {
        string strQueryMaster = "SELECT * FROM ExportStockMaster";
        string strQueryDetail = "SELECT ExportStockMasterId,ProductsId,b.Name,Amount,Price,TotalMoney,Note FROM ExportStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id";
        string[] columnsNameMaster = new string[] { "Mã phiếu", "Ngày xuất", "Khách hàng", "Ghi chú", "Tổng số lượng", "Tổng tiền", "Người tạo", "Ngày tạo", "Người sửa", "Ngày sửa", "Mã hóa đơn" };
        int[] widthMaster = new int[] { 120, 120, 120, 250, 120, 120, 120, 120, 120, 120 };
        string[] columnsNameDetail = new string[] { "Mã phiếu", "Mã sản phẩm", "Tên sản phẩm", "Số lượng", "Giá nhập", "Tổng tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 120, 250, 120, 120, 120, 250 };

        string[] columnsValuesSearch = new string[] { "CustomerId", "Note" };
        string[] columnsTextSearch = new string[] { "Khách hàng", "Ghichú" };


        public static Boolean isSave = true;
        public static Boolean isLoad = false;
        public static Boolean isDelete = false;
        public static Boolean isCopy = false;
        public frmExportStockMaster()
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
            this.dtgvMaster.CellClick += dtgvMaster_CellClick;
            this.menuIn.Click += menuIn_Click;
        }

        private void menuIn_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.Rows.Count == 0) return;
            frmRptExportStock frm = new frmRptExportStock(dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString());
            frm.ShowDialog();
        }

        private void menuXuatExcel_Click(object sender, EventArgs e)
        {
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount > 0)
            {
                isCopy = true;
                frmEditExportStockMaster frm = new frmEditExportStockMaster();
                frm.Text = "Copy";
                frm.SetValue(binSource);
                frm.ShowDialog();
                isCopy = false;
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
            if (dtgvMaster.Rows.Count == 0)
            {
                Alert.Error("Chưa có dữ liệu để xóa");
                return;
            }
            Alert.Question("Bạn có chắc chắn xóa dòng dữ liệu này không ?", out isDelete);
            if (isDelete)
            {
                ListEdit.DeletePostData(dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString());
                ListEdit.Delete("ExportStockDetail", "ExportStockMasterId", dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString());
                ListEdit.Delete("ExportStockMaster", "Id", dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString());
                LoadData();
            }
        }

        private void frm_Load(object sender, EventArgs e)
        {
            menuXuatExcel.Visible = false;
            LoadData();
            DLLSystem.RoleMenu(this, menuStrip, menuThem, menuSua, menuXoa, menuXuatExcel, menuCopy);
        }
        public void LoadData(string KeySearch = null)
        {
            string ExportStockMasterId = "";
            Lib.CssDatagridview.LoadData(binSource, dtgvMaster, strQueryMaster + KeySearch ?? "", columnsNameMaster, widthMaster);
            if (dtgvMaster.Rows.Count > 0)
            {
                ExportStockMasterId = " WHERE ExportStockMasterId = '" + dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString() + "' ";
            }
            else
            {
                ExportStockMasterId = " WHERE 1 = 0 ";
            }
            Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + ExportStockMasterId, columnsNameDetail, widthDetail);
        }
        private void menuTimKiem_Click(object sender, EventArgs e)
        {
            string KeySearch = "";
            ListEdit.LoadFormSearchList(columnsValuesSearch, columnsTextSearch, new System.Drawing.Point(400, 185), out KeySearch, true);
            LoadData(KeySearch);
        }

        private void menuThem_Click(object sender, EventArgs e)
        {
            isSave = true;
            frmEditExportStockMaster frm = new frmEditExportStockMaster();
            frm.Text = "Thêm mới";
            frm.ShowDialog();
            if (isLoad)
            {
                LoadData();
            }
        }

        private void menuSua_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount > 0)
            {
                isSave = false;
                frmEditExportStockMaster frm = new frmEditExportStockMaster();
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
        private void dtgvMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dtgvMaster.Rows[e.RowIndex];
                string ExportStockMasterId = " WHERE ExportStockMasterId = '" + row.Cells[0].Value.ToString() + "'";
                Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + ExportStockMasterId, columnsNameDetail, widthDetail);
            }
        }
    }
}
