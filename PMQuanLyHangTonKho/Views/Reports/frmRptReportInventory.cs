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
    public partial class frmRptReportInventory: Form
    {
        public frmRptReportInventory(string dFrom,string dTo,DataTable dtDetail)
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            decimal totalAmount = 0;
            foreach (DataRow dr in dtDetail.Rows)
            {
                totalAmount += decimal.Parse(dr["Amount"].ToString());
            }
            //DataRow newRow = dtDetail.NewRow();
            //newRow["STT"] = dtDetail.Rows.Count + 1;
            //newRow["ProductsName"] = "Tổng cộng";
            //newRow["Amount"] = totalAmount;
            //dtDetail.Rows.Add(newRow);

            reportViewer1.LocalReport.ReportEmbeddedResource = "PMQuanLyHangTonKho.rptReportInventory.rdlc";
            ReportDataSource rds = new ReportDataSource("DataSet1", dtDetail);
            ReportParameter[] parameters = new ReportParameter[2];
            string date = $@"{dFrom} - {dTo}";
            string dateNow = $@"Ngày {DateTime.Now.Day} Tháng {DateTime.Now.Month} Năm {DateTime.Now.Year}";
            parameters[0] = new ReportParameter("VoucherDate", date, true);
            parameters[1] = new ReportParameter("DatetimeNow", dateNow, true);
            reportViewer1.LocalReport.SetParameters(parameters);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(rds);
            this.reportViewer1.RefreshReport();
            dtDetail.Dispose();
        }
    }
}
