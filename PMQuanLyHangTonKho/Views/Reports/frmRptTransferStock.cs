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
    public partial class frmRptTransferStock: Form
    {
        public frmRptTransferStock(string Id)
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            DataTable dtMaster = new DataTable();
            DataTable dtDetail = new DataTable();
            string strMaster = "SELECT VoucherDate,b.Name as NameWarehouseFrom,c.Name as NameWarehouseTo,Note FROM TransferStockMaster a INNER JOIN Warehouse b ON a.WarehouseIdFrom = b.Id INNER JOIN Warehouse c ON a.WarehouseIdTo = c.Id WHERE a.Id = '" + Id + "'";
            string strDetail = "SELECT ROW_NUMBER() OVER (ORDER BY a.Id) AS STT,b.Name as ProductsName,a.Amount FROM TransferStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id WHERE TransferStockMasterId = '" + Id + "'";
            dtMaster = Models.SQL.GetData(strMaster);
            dtDetail = Models.SQL.GetData(strDetail);

            decimal totalAmount = 0;
            foreach (DataRow dr in dtDetail.Rows)
            {
                totalAmount += decimal.Parse(dr["Amount"].ToString());
            }

            DataRow newRow = dtDetail.NewRow();
            newRow["STT"] = dtDetail.Rows.Count + 1;
            newRow["ProductsName"] = "Tổng cộng";
            newRow["Amount"] = totalAmount;
            dtDetail.Rows.Add(newRow);

            reportViewer1.LocalReport.ReportEmbeddedResource = "PMQuanLyHangTonKho.rptTransferStock.rdlc";
            ReportDataSource rds = new ReportDataSource("DataSet1", dtDetail);
            ReportParameter[] parameters = new ReportParameter[4];
            DateTime voucherDate = DateTime.Parse(dtMaster.Rows[0]["VoucherDate"].ToString());
            string date = $@"Ngày {voucherDate.Day} Tháng {voucherDate.Month} Năm {voucherDate.Year}";
            string dateNow = $@"Ngày {DateTime.Now.Day} Tháng {DateTime.Now.Month} Năm {DateTime.Now.Year}";
            parameters[0] = new ReportParameter("VoucherDate", date, true);
            parameters[1] = new ReportParameter("WarehouseFrom", dtMaster.Rows[0]["NameWarehouseFrom"].ToString(), true);
            parameters[2] = new ReportParameter("WarehouseTo", dtMaster.Rows[0]["NameWarehouseTo"].ToString(), true);
            parameters[3] = new ReportParameter("DatetimeNow", dateNow, true);
            reportViewer1.LocalReport.SetParameters(parameters);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            dtMaster.Dispose();
            dtDetail.Dispose();
        }

        private void frmRptTransferStock_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }
    }
}
