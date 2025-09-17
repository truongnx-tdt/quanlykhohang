using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using PMQuanLyHangTonKho.Lib;
using PMQuanLyHangTonKho.Models.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmBangGia : Form
    {
        // Master + Detail
        string strQueryMaster =
              @"SELECT  pl.Id                          ,
                        pl.Name                        ,
                        CASE pl.Type WHEN 'P' THEN N'Mua' ELSE N'Bán' END AS [Loại],
                        pl.StartDate                   ,
                        pl.EndDate                     ,
                        CASE pl.IsActive WHEN 1 THEN N'Hoạt động' ELSE N'Ngừng' END AS [Trạng thái],
                        pl.Notes                       ,
                        pl.UserCreate                  ,
                        pl.DateCreate                  ,
                        pl.UserUpdate                  ,
                        pl.DateUpdate                  ,
                        pl.Type,
                        pl.IsActive
                FROM PriceListMaster pl";

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
            this.menuXuatExcel.Click += menuXuatExcel_Click;
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
                frm.SetValue(binSource);
                frm.ShowDialog();
            }
            if (isLoad) LoadData();
        }

        private void menuXuatExcel_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0)
            {
                Alert.Error("Chưa có dữ liệu để xuất Excel");
                return;
            }

            // Lấy dữ liệu từ DataGridView
            var priceListDTO = new List<PriceListDTO>();
            var customers = new List<PriceListDetailDTO>();
            foreach (DataGridViewRow row in dtgvMaster.Rows)
            {
                priceListDTO.Add(new PriceListDTO
                {
                    PriceId = Helper.GetCell<string>(row, "Id"),
                    PriceName = Helper.GetCell<string>(row, "Name"),
                    PriceType = Helper.GetCell<string>(row, "Loại"),
                    EffectiveFrom = Helper.GetCell<DateTime?>(row, "StartDate") ?? DateTime.MinValue,
                    EffectiveTo = Helper.GetCell<DateTime?>(row, "EndDate") ?? DateTime.MinValue,
                    Status = Helper.GetCell<string>(row, "Trạng thái"),
                    Note = Helper.GetCell<string>(row, "Notes"),
                    CreatedBy = Helper.GetCell<string>(row, "UserCreate"),
                    CreatedDate = Helper.GetCell<DateTime?>(row, "DateCreate") ?? DateTime.MinValue,
                    ModifiedBy = Helper.GetCell<string>(row, "UserUpdate"),
                    ModifiedDate = Helper.GetCell<DateTime?>(row, "DateUpdate") ?? DateTime.MinValue
                });

                // Chi tiết from Data base
                var queryDetail = $@"SELECT  d.PriceListId,
                                              d.ProductId,
                                              p.Name,
                                              d.FromQty, d.ToQty, d.UnitPrice, d.Notes
                                      FROM PriceListDetail d
                                      INNER JOIN Products p ON d.ProductId = p.Id 
                                      WHERE d.PriceListId = @PriceListId
                                      ORDER BY d.ProductId, d.FromQty";
                var dtDetail = Models.SQL.QueryList<PriceListDetailDTO>(queryDetail, new Dictionary<string, object> { { "PriceListId", Helper.GetCell<string>(row, "Id") } },
                    projector: r => new PriceListDetailDTO
                    {
                        PriceListId = r["PriceListId"].ToString(),
                        ProductId = r["ProductId"].ToString(),
                        Name = r["Name"].ToString(),
                        FromQty = Convert.ToDecimal(r["FromQty"]),
                        ToQty = Convert.ToDecimal(r["ToQty"]),
                        UnitPrice = Convert.ToDecimal(r["UnitPrice"]),
                        Notes = r["Notes"] as string
                    });

                foreach (var item in dtDetail)
                {
                    customers.Add(item);
                }
            }





            var sheets = new List<ExcelExporter.ObjectSheetSpec>
            {
        //         string[] columnsNameMaster = new string[]
        //{
        //    "Mã bảng giá","Tên","Loại","Hiệu lực từ","Hiệu lực đến","Trạng thái",
        //    "Ghi chú","Người tạo","Ngày tạo","Người sửa","Ngày sửa"
        //};

     
                new ExcelExporter.ObjectSheetSpec(
                    "Bảng giá",
                    priceListDTO,
                    new List<ExcelExporter.ObjectColumnSpec>{
                        new ExcelExporter.ObjectColumnSpec{ Header="Mã bảng giá", Selector=o=>((PriceListDTO)o).PriceId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Tên", Selector=o=>((PriceListDTO)o).PriceName },
                        new ExcelExporter.ObjectColumnSpec{ Header="Loại", Selector=o=>((PriceListDTO)o).PriceType },
                        new ExcelExporter.ObjectColumnSpec{ Header="Hiệu lực từ", Selector=o=>((PriceListDTO)o).EffectiveFrom, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy" },
                        new ExcelExporter.ObjectColumnSpec{ Header="Hiệu lực đến", Selector=o=>((PriceListDTO)o).EffectiveTo, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy" },
                        new ExcelExporter.ObjectColumnSpec{ Header="Trạng thái", Selector=o=>((PriceListDTO)o).Status },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ghi chú", Selector=o=>((PriceListDTO)o).Note },
                        new ExcelExporter.ObjectColumnSpec{ Header="Người tạo", Selector=o=>((PriceListDTO)o).CreatedBy },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ngày tạo", Selector=o=>((PriceListDTO)o).CreatedDate, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy HH:mm:ss" },
                        new ExcelExporter.ObjectColumnSpec{ Header="Người sửa", Selector=o=>((PriceListDTO)o).ModifiedBy },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ngày sửa", Selector=o=>((PriceListDTO)o).ModifiedDate, DataType=XLDataType.DateTime, NumberFormat="dd/MM/yyyy HH:mm:ss" }
                    }
                ),
        //           string[] columnsNameDetail = new string[]
        //{
        //    "Mã bảng giá","Mã hàng","Tên hàng","Từ SL","Đến SL","Đơn giá","Ghi chú"
        //};
                new ExcelExporter.ObjectSheetSpec(
                    "Chi tiết",
                    customers,
                    new List<ExcelExporter.ObjectColumnSpec>{
                        new ExcelExporter.ObjectColumnSpec{ Header="Mã bảng giá", Selector=o=>((PriceListDetailDTO)o).PriceListId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Mã hàng", Selector=o=>((PriceListDetailDTO)o).ProductId },
                        new ExcelExporter.ObjectColumnSpec{ Header="Tên hàng", Selector=o=>((PriceListDetailDTO)o).Name },
                        new ExcelExporter.ObjectColumnSpec{ Header="Từ SL", Selector=o=>((PriceListDetailDTO)o).FromQty, DataType=XLDataType.Number },
                        new ExcelExporter.ObjectColumnSpec{ Header="Đến SL", Selector=o=>((PriceListDetailDTO)o).ToQty, DataType=XLDataType.Number },
                        new ExcelExporter.ObjectColumnSpec{ DataType=XLDataType.Number, Header="Đơn giá", Selector=o=>((PriceListDetailDTO)o).UnitPrice, NumberFormat="#,##0.00" },
                        new ExcelExporter.ObjectColumnSpec{ Header="Ghi chú", Selector=o=>((PriceListDetailDTO)o).Notes }
                    }
                )
            };
            ExcelExporter.ExportObjectsMulti("BaoCao.xlsx", sheets);
        }

        private void menuXoa_Click(object sender, EventArgs e)
        {
            if (dtgvMaster.RowCount == 0) { Alert.Error("Chưa có dữ liệu để xóa"); return; }

            bool confirm;
            Alert.Question("Bạn chắc chắn muốn xóa bảng giá này?", out confirm);
            if (!confirm) return;

            var id = dtgvMaster.Rows[dtgvMaster.CurrentCell.RowIndex].Cells[0].Value.ToString();
            ListEdit.Delete("PriceListMaster", "Id", id);

            LoadData();
        }

        private void menuLamMoi_Click(object sender, EventArgs e) => LoadData();
    }
}
