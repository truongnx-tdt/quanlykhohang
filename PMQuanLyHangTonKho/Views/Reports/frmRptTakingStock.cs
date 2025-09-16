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
    public partial class frmRptTakingStock: Form
    {
        public frmRptTakingStock(string Id)
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            DataTable dtMaster = new DataTable();
            DataTable dtDetail = new DataTable();
            string strMaster = "SELECT VoucherDate,b.Name as NameWarehouse, c.Name as NameEmployees1,d.Name as NameEmployees2,e.Name as NameEmployees3 FROM TakingStockMaster a INNER JOIN Warehouse b ON a.WarehouseId = b.Id INNER JOIN Employees c ON a.EmployeesId1 = c.Id INNER JOIN Employees d ON a.EmployeesId2 = d.Id INNER JOIN Employees e ON a.EmployeesId3 = e.Id WHERE a.Id = '" + Id + "'";
            string strDetail = "SELECT ROW_NUMBER() OVER (ORDER BY a.Id) AS STT,b.Name as ProductsName,a.Amount FROM TakingStockDetail a INNER JOIN Products b ON a.ProductsId = b.Id WHERE TakingStockMasterId = '" + Id + "'";
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

            reportViewer1.LocalReport.ReportEmbeddedResource = "PMQuanLyHangTonKho.rptTakingStock.rdlc";
            ReportDataSource rds = new ReportDataSource("DataSet1", dtDetail);
            ReportParameter[] parameters = new ReportParameter[6];
            DateTime voucherDate = DateTime.Parse(dtMaster.Rows[0]["VoucherDate"].ToString());
            string date = $@"Ngày {voucherDate.Day} Tháng {voucherDate.Month} Năm {voucherDate.Year}";
            string dateNow = $@"Ngày {DateTime.Now.Day} Tháng {DateTime.Now.Month} Năm {DateTime.Now.Year}";
            parameters[0] = new ReportParameter("VoucherDate", date, true);
            parameters[1] = new ReportParameter("Warehouse", dtMaster.Rows[0]["NameWarehouse"].ToString(), true);
            parameters[2] = new ReportParameter("Employees1", dtMaster.Rows[0]["NameEmployees1"].ToString(), true);
            parameters[3] = new ReportParameter("Employees2", dtMaster.Rows[0]["NameEmployees2"].ToString(), true);
            parameters[4] = new ReportParameter("Employees3", dtMaster.Rows[0]["NameEmployees3"].ToString(), true);
            parameters[5] = new ReportParameter("DatetimeNow", dateNow, true);
            reportViewer1.LocalReport.SetParameters(parameters);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            dtMaster.Dispose();
            dtDetail.Dispose();
        }
    }
}
