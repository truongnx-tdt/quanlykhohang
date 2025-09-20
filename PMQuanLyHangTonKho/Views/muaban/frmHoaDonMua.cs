using ClosedXML.Excel;
using PMQuanLyHangTonKho.Lib;
using PMQuanLyHangTonKho.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmHoaDonMua : Form
    {
        string strQueryMaster =
            @"SELECT Id, DocDate, SupplierId, PriceListId, Notes,
                     TotalMoney, TotalQty, UserCreate, DateCreate, UserUpdate, DateUpdate
              FROM PurchaseInvoiceMaster";

        string strQueryDetail =
            @"SELECT PIMasterId, a.ProductId, b.Name,
                     a.Qty, a.UnitPrice, a.LineAmount, a.Notes
              FROM PurchaseInvoiceDetail a
              INNER JOIN Products b ON a.ProductId = b.Id";

        string[] columnsNameMaster = new string[]
        {
            "Mã HĐ","Ngày HĐ","Nhà cung cấp","Bảng giá","Ghi chú",
            "Tổng tiền","Tổng số lượng","Người tạo","Ngày tạo","Người sửa","Ngày sửa"
        };
        int[] widthMaster = new int[] { 120, 120, 150, 150, 250, 120, 120, 120, 140, 120, 140 };

        string[] columnsNameDetail = new string[]
        { "Mã HĐ","Mã SP","Tên SP","Số lượng","Giá","Thành tiền","Ghi chú" };
        int[] widthDetail = new int[] { 120, 120, 250, 110, 110, 120, 250 };

        string[] columnsValuesSearch = new string[] { "SupplierId", "PriceListId", "Notes" };
        string[] columnsTextSearch = new string[] { "Nhà cung cấp", "Bảng giá", "Ghi chú" };

        public static bool isSave = true;
        public static bool isLoad = false;
        public static bool isDelete = false;
        public static bool isCopy = false;

        public frmHoaDonMua()
        {
            InitializeComponent();
            DLLSystem.Init(this);
            this.Load += frm_Load;

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
            DLLSystem.RoleMenu(this, menuStrip, menuThem, menuSua, menuXoa, menuXuatExcel);
        }

        public void LoadData(string key = null)
        {
            Lib.CssDatagridview.LoadData(binSource, dtgvMaster, strQueryMaster + (key ?? ""), columnsNameMaster, widthMaster);

            string filter;
            if (dtgvMaster.Rows.Count > 0)
            {
                var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();
                filter = " WHERE PIMasterId = '" + id + "'";
            }
            else filter = " WHERE 1=0";

            Lib.CssDatagridview.LoadData(null, dtgvDetail, strQueryDetail + filter, columnsNameDetail, widthDetail);
        }

        private void dtgvMaster_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string id = dtgvMaster.Rows[e.RowIndex].Cells[0].Value.ToString();
            string filter = " WHERE PIMasterId = '" + id + "'";
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
            using (var frm = new frmEditHoaDonMua())
            {
                frm.Text = "Thêm hóa đơn";
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuSua_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để sửa"); return; }
            isSave = false;
            using (var frm = new frmEditHoaDonMua())
            {
                frm.Text = "Sửa hóa đơn";
                frm.SetValue(binSource);
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuXoa_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để xóa"); return; }

            bool ok;
            Alert.Question("Bạn có chắc chắn xóa hóa đơn mua này không?", out ok);
            if (!ok) return;

            var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();
            ListEdit.Delete("PurchaseInvoiceDetail", "PIMasterId", id);
            ListEdit.Delete("PurchaseInvoiceMaster", "Id", id);

            LoadData();
        }

        private void menuXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0)
            {
                Alert.Error("Chưa có dữ liệu để xuất Excel");
                return;
            }

            // Lấy dữ liệu từ DataGridView
            var priceListDTO = new List<PIDTO>();
            var customers = new List<PIDetailDTO>();
            foreach (DataGridViewRow row in dtgvMaster.Rows)
            {
                priceListDTO.Add(new PIDTO
                {
                    Id = Helper.GetCell<string>(row, "Id"),
                    DocDate = Helper.GetCell<string>(row, "DocDate"),
                    SupplierId = Helper.GetCell<string>(row, "SupplierId"),
                    PriceListId = Helper.GetCell<string>(row, "PriceListId"),
                    Notes = Helper.GetCell<string>(row, "Notes"),
                    TotalMoney = Helper.GetCell<string>(row, "TotalMoney"),
                    TotalQty = Helper.GetCell<string>(row, "TotalQty"),
                    UserCreate = Helper.GetCell<string>(row, "UserCreate"),
                    DateCreate = Helper.GetCell<DateTime?>(row, "DateCreate") ?? DateTime.MinValue,
                    UserUpdate = Helper.GetCell<string>(row, "UserUpdate"),
                    DateUpdate = Helper.GetCell<DateTime?>(row, "DateUpdate") ?? DateTime.MinValue
                });

                // Chi tiết from Data base
                var queryDetail = $@"SELECT PIMasterId, a.ProductId, b.Name,
                                             a.Qty, a.UnitPrice, a.LineAmount, a.Notes
                                      FROM PurchaseInvoiceDetail a
                                      INNER JOIN Products b ON a.ProductId = b.Id
                                      WHERE a.PIMasterId = @PIMasterId";
                var dtDetail = Models.SQL.QueryList<PIDetailDTO>(queryDetail, new Dictionary<string, object> { { "PIMasterId", Helper.GetCell<string>(row, "Id") } },
                    projector: r => new PIDetailDTO
                    {
                        PIMasterId = r["PIMasterId"].ToString(),
                        ProductId = r["ProductId"].ToString(),
                        Name = r["Name"].ToString(),
                        Qty = (r["Qty"]).ToString(),
                        UnitPrice = (r["UnitPrice"]).ToString(),
                        LineAmount = (r["LineAmount"]).ToString(),
                        Notes = r["Notes"] as string
                    });

                foreach (var item in dtDetail)
                {
                    customers.Add(item);
                }
            }

            var sheets = new List<ExcelExporter.ObjectSheetSpec>
            {
                new ExcelExporter.ObjectSheetSpec(
                    "Hóa đơn mua",
                    priceListDTO,
                    new List<ExcelExporter.ObjectColumnSpec>{
                        new ExcelExporter.ObjectColumnSpec{ Header="Mã HĐ", Selector=o=>((PIDTO)o).Id },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ngày HĐ", Selector=o=>((PIDTO)o).DocDate, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy HH:mm:ss" },
                        new ExcelExporter.ObjectColumnSpec{ Header="Nhà cung cấp", Selector=o=>((PIDTO)o).SupplierId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Bảng giá", Selector=o=>((PIDTO)o).PriceListId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ghi chú", Selector=o=>((PIDTO)o).Notes },
                        new ExcelExporter.ObjectColumnSpec{ Header="Tổng tiền", Selector=o=>((PIDTO)o).TotalMoney },
                        new ExcelExporter.ObjectColumnSpec{ Header="Tổng số lượng", Selector=o=>((PIDTO)o).TotalQty },
                        new ExcelExporter.ObjectColumnSpec{ Header="Người tạo", Selector=o=>((PIDTO)o).UserCreate },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ngày tạo", Selector=o=>((PIDTO)o).DateCreate, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy HH:mm:ss" },
                        new ExcelExporter.ObjectColumnSpec{ Header="Người sửa", Selector=o=>((PIDTO)o).UserUpdate },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ngày sửa", Selector=o=>((PIDTO)o).DateUpdate, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy HH:mm:ss" }
                    }
                ),
                new ExcelExporter.ObjectSheetSpec(
                    "Chi tiết",
                    customers,
                    new List<ExcelExporter.ObjectColumnSpec>{
                        new ExcelExporter.ObjectColumnSpec{ Header="Mã HĐ", Selector=o=>((PIDetailDTO)o).PIMasterId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Mã SP", Selector=o=>((PIDetailDTO)o).ProductId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Tên SP", Selector=o=>((PIDetailDTO)o).Name },
                        new ExcelExporter.ObjectColumnSpec{ Header="Số lượng", Selector=o=>((PIDetailDTO)o).Qty },
                        new ExcelExporter.ObjectColumnSpec{ Header="Giá", Selector=o=>((PIDetailDTO)o).UnitPrice },
                        new ExcelExporter.ObjectColumnSpec{ Header="Thành tiền", Selector=o=>((PIDetailDTO)o).LineAmount },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ghi chú", Selector=o=>((PIDetailDTO)o).Notes },
                    }
                )
            };
            ExcelExporter.ExportObjectsMulti("BaoCao_hdm.xlsx", sheets);
        }

    }
}
