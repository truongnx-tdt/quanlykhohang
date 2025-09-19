using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using PMQuanLyHangTonKho.Lib;
using PMQuanLyHangTonKho.Views.Reports;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmReportInventory: Form
    {
        // Lookup kho
        string strQueryWare = "SELECT CAST(0 AS BIT) AS xtag, Id, Name FROM Warehouse";
        string[] columnsWareText = new[] { "Chọn", "Mã kho", "Tên kho" };
        int[] widthWare = new[] { 60, 120, 250 };

        string[] columnsValues = new string[] { "STT", "ProductsName", "Amount"};
        string[] columnsName = new string[] { "STT", "Tên sản phẩm", "Số lượng tồn"};
        int[] width = new int[] { 60, 250, 180};
        public frmReportInventory()
        {
            InitializeComponent();
            DLLSystem.Init(this);
            CssButton.StyleButton(btnFind,"find.bmp");
            CssButton.StyleButton(btnXuatExcel,"excel.bmp");
            CssButton.StyleButton(btnIn,"print.bmp");
            LoadData();
            btnFind.Click += bntFind_Click;
            btnXuatExcel.Click += btnXuatExcel_Click;
            btnIn.Click += btnIn_Click;
            // giả sử bạn đã có 3 control: txtWarehouseId, lblWarehouseName, btnChooseWarehouse
            this.btnChooseWarehouse.Click += btnChooseWarehouse_Click;

        }

        private void btnChooseWarehouse_Click(object sender, EventArgs e)
        {
            string outId, outName;
            Lookup.SearchLookupSingle(strQueryWare, columnsWareText, widthWare,
                                      txtWarehouseId.Text, out outId, out outName);
            txtWarehouseId.Text = outId;
            lblWarehouseName.Text = outName;
            LoadData();
        }



        private void btnIn_Click(object sender, EventArgs e)
        {
            frmRptReportInventory frm = new frmRptReportInventory(dtpFrom.Value.ToString("dd/MM/yyyy"), dtpTo.Value.ToString("dd/MM/yyyy"),(System.Data.DataTable)dtgvMain.DataSource);
            frm.ShowDialog();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            DLLSystem.ExportExcel(dtgvMain);
        }

        private void bntFind_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            var from = dtpFrom.Value.ToString("yyyyMMdd");
            var to = dtpTo.Value.ToString("yyyyMMdd");
            var wh = (txtWarehouseId.Text ?? "").Trim().Replace("'", "''");

            string strQuery =
                "SELECT b.Name AS ProductsName, SUM(a.Amount) AS Amount INTO #t " +
                "FROM PostData a " +
                "INNER JOIN Products b ON a.ProductsId = b.Id " +
                $"WHERE a.VoucherDate BETWEEN '{from}' AND '{to}' " +
                (string.IsNullOrEmpty(wh) ? "" : $"AND a.WarehouseId = '{wh}' ") +
                "GROUP BY b.Name; " +
                "SELECT ROW_NUMBER() OVER (ORDER BY ProductsName) AS STT, * FROM #t";

            Lib.CssDatagridview.LoadData(null, dtgvMain, strQuery, columnsName, width);
        }

    }
}
