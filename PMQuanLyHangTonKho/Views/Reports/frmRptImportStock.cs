using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using PMQuanLyHangTonKho.Lib;

namespace PMQuanLyHangTonKho.Views.Reports
{
    public partial class frmRptImportStock: Form
    {
        public frmRptImportStock(string Id)
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            DataTable dtMaster = new DataTable();
            DataTable dtDetail = new DataTable();
            string strMaster = "SELECT VoucherDate,b.Name as NameWarehouse,c.Name as NameSupplier,Note FROM ImportStockMaster a INNER JOIN Warehouse b ON a.WarehouseId = b.Id INNER JOIN Supplier c ON a.SupplierId = c.Id WHERE a.Id = '" + Id + "'";
            string strDetail = "SELECT ROW_NUMBER() OVER (ORDER BY a.Id) AS ProductsId,b.Name as ProductsName,a.Amount,a.Price,a.TotalMoney FROM ImportStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id WHERE ImportStockMasterId = '" + Id + "'";
            dtMaster = Models.SQL.GetData(strMaster);
            dtDetail = Models.SQL.GetData(strDetail);

            decimal totalAmount = 0;
            decimal totalPrice = 0;
            decimal totalMoney = 0;
            foreach (DataRow dr in dtDetail.Rows)
            {
                totalAmount += decimal.Parse(dr["Amount"].ToString());
                totalPrice += decimal.Parse(dr["Price"].ToString());
                totalMoney += decimal.Parse(dr["TotalMoney"].ToString());
            }

            DataRow newRow = dtDetail.NewRow();
            newRow["ProductsId"] = dtDetail.Rows.Count + 1;
            newRow["ProductsName"] = "Tổng cộng";
            newRow["Amount"] = totalAmount;
            newRow["Price"] = totalPrice;
            newRow["TotalMoney"] = totalMoney;
            dtDetail.Rows.Add(newRow);

            reportViewer1.LocalReport.ReportEmbeddedResource = "PMQuanLyHangTonKho.rptImportStock.rdlc";
            ReportDataSource rds = new ReportDataSource("DataSet1", dtDetail);
            ReportParameter[] parameters = new ReportParameter[4];
            DateTime voucherDate = DateTime.Parse(dtMaster.Rows[0]["VoucherDate"].ToString());
            string date = $@"Ngày {voucherDate.Day} Tháng {voucherDate.Month} Năm {voucherDate.Year}";
            string dateNow = $@"Ngày {DateTime.Now.Day} Tháng {DateTime.Now.Month} Năm {DateTime.Now.Year}";
            parameters[0] = new ReportParameter("VoucherDate", date, true);
            parameters[1] = new ReportParameter("Warehouse", dtMaster.Rows[0]["NameWarehouse"].ToString(), true);
            parameters[2] = new ReportParameter("Supplier", dtMaster.Rows[0]["NameSupplier"].ToString(), true);
            parameters[3] = new ReportParameter("DatetimeNow", dateNow, true);
            reportViewer1.LocalReport.SetParameters(parameters);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            dtMaster.Dispose();
            dtDetail.Dispose();
        }

        private void frmRptImportStock_Load(object sender, EventArgs e)
        {
            
        }
    }
}
