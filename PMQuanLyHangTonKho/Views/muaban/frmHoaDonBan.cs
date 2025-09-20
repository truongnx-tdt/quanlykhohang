using PMQuanLyHangTonKho.Lib;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmHoaDonBan : Form
    {
        string strQueryMaster =
            @"SELECT Id, DocDate, CustomerId, WarehouseId, PriceListId, IsRetail,
                     Notes, TotalMoney, TotalQty, UserCreate, DateCreate, UserUpdate, DateUpdate
              FROM SalesInvoiceMaster";

        string strQueryDetail =
            @"SELECT SIMasterId, a.ProductId, b.Name,
                     a.Qty, a.UnitPrice, a.LineAmount, a.Notes
              FROM SalesInvoiceDetail a
              INNER JOIN Products b ON a.ProductId = b.Id";

        string[] columnsNameMaster = new string[]
        {
            "Mã HĐ","Ngày HĐ","Khách hàng","Kho","Bảng giá","Bán lẻ",
            "Ghi chú","Tổng tiền","Tổng SL","Người tạo","Ngày tạo","Người sửa","Ngày sửa"
        };
        int[] widthMaster = new int[] { 120, 120, 150, 120, 120, 80, 250, 120, 100, 120, 140, 120, 140 };

        string[] columnsNameDetail = new string[] { "Mã HĐ", "Mã SP", "Tên SP", "Số lượng", "Giá", "Thành tiền", "Ghi chú" };
        int[] widthDetail = new int[] { 120, 120, 250, 110, 110, 120, 250 };

        string[] columnsValuesSearch = new string[] { "CustomerId", "PriceListId", "IsRetail", "Notes" };
        string[] columnsTextSearch = new string[] { "Khách hàng", "Bảng giá", "Bán lẻ", "Ghi chú" };

        public static bool isSave = true;
        public static bool isLoad = false;
        public static bool isDelete = false;
        public static bool isCopy = false;

        public frmHoaDonBan()
        {
            InitializeComponent();
            DLLSystem.Init(this);
            Load += frm_Load;

            menuTimKiem.Click += menuTimKiem_Click;
            menuThem.Click += menuThem_Click;
            menuSua.Click += menuSua_Click;
            menuXoa.Click += menuXoa_Click;
            menuLamMoi.Click += (s, e) => LoadData();
            menuThoat.Click += (s, e) => Close();

            dtgvMaster.CellClick += dtgvMaster_CellClick;
        }

        private void frm_Load(object sender, EventArgs e)
        {
            LoadData();
            DLLSystem.RoleMenu(this, menuStrip, menuThem, menuSua, menuXoa, null);
        }

        public void LoadData(string key = null)
        {
            Lib.CssDatagridview.LoadData(binSource, dtgvMaster, strQueryMaster + (key ?? ""), columnsNameMaster, widthMaster);

            string filter = " WHERE 1=0";
            if (dtgvMaster.Rows.Count > 0)
            {
                var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();
                filter = " WHERE SIMasterId = '" + id + "'";
            }
            Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + filter, columnsNameDetail, widthDetail);
        }

        private void dtgvMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string id = dtgvMaster.Rows[e.RowIndex].Cells[0].Value.ToString();
            string filter = " WHERE SIMasterId = '" + id + "'";
            Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + filter, columnsNameDetail, widthDetail);
        }

        private void menuTimKiem_Click(object sender, EventArgs e)
        {
            string key = "";
            ListEdit.LoadFormSearchListV2(columnsValuesSearch, columnsTextSearch, new Point(400, 185), out key, true);
            LoadData(key);
        }

        private void menuThem_Click(object sender, EventArgs e)
        {
            isSave = true;
            using (var frm = new frmEditHoaDonBan())
            {
                frm.Text = "Thêm hóa đơn bán";
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuSua_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để sửa"); return; }
            isSave = false;
            using (var frm = new frmEditHoaDonBan())
            {
                frm.Text = "Sửa hóa đơn bán";
                frm.SetValue(binSource);
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuXoa_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để xóa"); return; }
            bool ok; Alert.Question("Bạn có chắc chắn xóa hóa đơn bán này không?", out ok);
            if (!ok) return;

            var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();

            Models.SQL.RunQuery("DELETE FROM SalesInvoiceDetail WHERE SIMasterId=@Id", new object[,] { { "@Id", id } });
            Models.SQL.RunQuery("DELETE FROM PostData WHERE VoucherId=@Id AND VoucherCode='HDB'", new object[,] { { "@Id", id } });
            Models.SQL.RunQuery("DELETE FROM SalesInvoiceMaster WHERE Id=@Id", new object[,] { { "@Id", id } });

            LoadData();
        }

       
    }
}
