using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using Microsoft.Reporting.WinForms;
using PMQuanLyHangTonKho.Lib;
using QRCoder;

namespace PMQuanLyHangTonKho
{
    public partial class frmQRCodeProduct: Form
    {
        public frmQRCodeProduct(string IdProduct)
        {
            InitializeComponent();
            DLLSystem.InitNotResize(this);
            SetDataQRCode(IdProduct);
        }

        private void frmQRCodeProduct_Load(object sender, EventArgs e)
        {

        }
        private void SetDataQRCode(string IdProduct)
        {
            string strQuery = "SELECT * FROM Products WHERE Id = '"+ IdProduct + "'";
            DataTable dt = new DataTable();
            dt = Models.SQL.GetData(strQuery);
            if(dt != null)
            {
                string status = "";
                if(dt.Rows[0]["Status"].ToString() == "1")
                {
                    status = "Đang sử dụng";
                }    
                else
                {
                    status = "Ngừng sử dụng";
                }    
                string textToEncode = "Mã sản phẩm: " + dt.Rows[0]["Id"].ToString() +"\n" +
                      "Tên sản phẩm: "+ dt.Rows[0]["Name"].ToString() + "\n" +
                      "Mã nhóm: "+ dt.Rows[0]["ProductCategoryId"].ToString() + "\n" +
                      "Đơn vị tính: "+ dt.Rows[0]["Unit"].ToString() + "\n" +
                      "Số lượng tồn tối thiểu: "+ dt.Rows[0]["MinimumStock"].ToString() + "\n" +
                      "Số lượng tồn tối đa: "+ dt.Rows[0]["MaximumStock"].ToString() + "\n" +
                      "Mô tả: "+ dt.Rows[0]["Description"].ToString() + "\n" +
                      "Hạn sử dụng: "+ DateTime.Parse(dt.Rows[0]["ExpirationDate"].ToString()).ToString("dd/MM/yyyy") + "\n" +
                      "Trạng thái sử dụng: "+ status + "";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(textToEncode, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                string qrCodeFilePath = "D:\\z_"+Guid.NewGuid().ToString()+".png";
                qrCodeImage.Save(qrCodeFilePath, ImageFormat.Png);
                reportViewer1.LocalReport.ReportEmbeddedResource = "PMQuanLyHangTonKho.rptQRCodeProduct.rdlc";
                ReportParameter pImage = new ReportParameter("pImage", new Uri(qrCodeFilePath).AbsoluteUri);
                reportViewer1.LocalReport.EnableExternalImages = true;
                reportViewer1.LocalReport.SetParameters(new ReportParameter[] {pImage});
                this.reportViewer1.RefreshReport();

            }
        }
    }
}
