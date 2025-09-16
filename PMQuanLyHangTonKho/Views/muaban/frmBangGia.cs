using System;
using System.Data;
using System.Windows.Forms;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmBangGia : Form
    {
        // Master + Detail
        string strQueryMaster =
            @"SELECT  pl.Id,
                      pl.Name,
                      CASE pl.Type WHEN 'P' THEN N'Mua' ELSE N'Bán' END AS [Loại],
                      pl.StartDate, pl.EndDate,
                      CASE pl.IsActive WHEN 1 THEN N'Đang dùng' ELSE N'Ngừng' END AS [Trạng thái],
                      pl.Notes, pl.UserCreate, pl.DateCreate, pl.UserUpdate, pl.DateUpdate
              FROM PriceListMaster pl ";

        string strQueryDetail =
            @"SELECT  d.PriceListId,
                      d.ProductId,
                      p.Name AS [Tên hàng],
                      d.FromQty, d.ToQty, d.UnitPrice, d.Notes
              FROM PriceListDetail d
              INNER JOIN Products p ON d.ProductId = p.Id ";

        string[] columnsNameMaster = new string[]
        {
            "Mã bảng giá","Tên","Loại","Hiệu lực từ","Hiệu lực đến","Trạng thái",
            "Ghi chú","Người tạo","Ngày tạo","Người sửa","Ngày sửa"
        };
        int[] widthMaster = new int[] { 120, 200, 80, 110, 110, 100, 220, 120, 140, 120, 140 };

        string[] columnsNameDetail = new string[]
        {
            "Mã bảng giá","Mã hàng","Tên hàng","Từ SL","Đến SL","Đơn giá","Ghi chú"
        };
        int[] widthDetail = new int[] { 120, 120, 240, 90, 90, 110, 220 };

        // Tìm kiếm nhanh
        string[] columnsValuesSearch = new string[] { "pl.Name", "pl.Type", "pl.IsActive" };
        string[] columnsTextSearch = new string[] { "Tên", "Loại (P/S)", "Trạng thái (1/0)" };

        public static bool isSave = true;
        public static bool isLoad = false;
        public static bool isDelete = false;
        public static bool isCopy = false;

        public frmBangGia()
        {
            InitializeComponent();
            DLLSystem.Init(this);

            this.Load += frm_Load;
            this.menuTimKiem.Click += menuTimKiem_Click;
            this.menuThem.Click += menuThem_Click;
            this.menuSua.Click += menuSua_Click;
            this.menuXoa.Click += menuXoa_Click;
            this.menuLamMoi.Click += menuLamMoi_Click;
            this.menuXuatExcel.Click += menuCopy_Click;
            this.menuThoat.Click += (s, e) => Close();
            this.dtgvMaster.CellClick += dtgvMaster_CellClick;
        }

        private void frm_Load(object sender, EventArgs e)
        {
            LoadData();
            DLLSystem.RoleMenu(this, menuStrip, menuThem, menuSua, menuXoa, menuXuatExcel);
        }

        public void LoadData(string keySearch = null)
        {
            string where = keySearch ?? "";
            Lib.CssDatagridview.LoadData(binSource, dtgvMaster, strQueryMaster + where + " ORDER BY pl.DateCreate DESC", columnsNameMaster, widthMaster);

            string filterDetail;
            if (dtgvMaster.Rows.Count > 0)
            {
                var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();
                filterDetail = " WHERE d.PriceListId = '" + id + "' ";
            }
            else filterDetail = " WHERE 1=0 ";

            Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + filterDetail + " ORDER BY d.ProductId, d.FromQty", columnsNameDetail, widthDetail);
        }

        private void dtgvMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string id = dtgvMaster.Rows[e.RowIndex].Cells[0].Value.ToString();
            string filter = " WHERE d.PriceListId = '" + id + "'";
            Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + filter + " ORDER BY d.ProductId, d.FromQty", columnsNameDetail, widthDetail);
        }

        private void menuTimKiem_Click(object sender, EventArgs e)
        {
            string key = "";
            ListEdit.LoadFormSearchList(columnsValuesSearch, columnsTextSearch, new System.Drawing.Point(400, 185), out key, true);
            LoadData(key);
        }

        private void menuThem_Click(object sender, EventArgs e)
        {
            isSave = true;
            using (var frm = new frmEditBangGia())
            {
                frm.Text = "Thêm bảng giá";
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuSua_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để sửa"); return; }
            isSave = false;
            using (var frm = new frmEditBangGia())
            {
                frm.Text = "Sửa bảng giá";
                //frm.SetValue(binSource); 
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để sao chép"); return; }
            isCopy = true;
            using (var frm = new frmEditBangGia())
            {
                frm.Text = "Sao chép bảng giá";
                //frm.SetValue(binSource);
                frm.ShowDialog();
            }
            isCopy = false;
            if (isLoad) LoadData();
        }

        private void menuXoa_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để xóa"); return; }

            bool confirm;
            Alert.Question("Bạn chắc chắn muốn xóa bảng giá này?", out confirm);
            if (!confirm) return;

            var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();
            ListEdit.Delete("PriceListDetail", "PriceListId", id);
            ListEdit.Delete("PriceListMaster", "Id", id);

            LoadData();
        }

        private void menuLamMoi_Click(object sender, EventArgs e) => LoadData();
    }
}
