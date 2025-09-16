using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public class DLLSystem
    {
        public static string GetPathImage()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).FullName.Replace("bin", "") + "Content\\Images\\";
        }
        public static string GetPathTemp()
        {
            return Directory.GetParent(Directory.GetCurrentDirectory()).FullName.Replace("bin", "") + "Content\\Images\\Temp\\";
        }
        public static void InitNotResize(Form frm)
        {
            Init(frm);
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;
            frm.BackColor = Color.White;
        }
        public static void Init(Form frm)
        {
            frm.Icon = new Icon(GetPathImage() + "logo2.ico");
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.BackColor = Color.White;
        }
        public static string GenCode(string Voucher)
        {
            return Voucher + DateTime.Now.ToString("hhmmssddMMyyyy");
        }
        public static void ExportExcel(DataGridView dtgvMain)
        {
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sheet1"];
            worksheet = workbook.ActiveSheet;
            for (int i = 1; i < dtgvMain.Columns.Count + 1; i++)
            {
                worksheet.Cells[1, i] = dtgvMain.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < dtgvMain.Rows.Count; i++)
            {
                for (int j = 0; j < dtgvMain.Columns.Count; j++)
                {
                    if (dtgvMain.Rows[i].Cells[j].Value != null)
                    {
                        worksheet.Cells[i + 2, j + 1] = dtgvMain.Rows[i].Cells[j].Value.ToString();
                    }
                    else
                    {
                        worksheet.Cells[i + 2, j + 1] = "";
                    }
                }
            }
        }
        public static Boolean RoleForm(string form)
        {
            String strSQL = "SELECT See FROM MenuItem WHERE Form = '"+form+"'";
            DataTable dt = new DataTable();
            dt = Models.SQL.GetData(strSQL);
            if (!dt.Rows[0]["See"].ToString().Contains(frmLogin.UserId) && frmLogin.Admin == "False")
            {
                Alert.Error("Không có quyền truy cập");
                return false;
            }
            return true;
        }
        public static void RoleMenu(Form frm, MenuStrip menuStrip, ToolStripMenuItem menuThem, ToolStripMenuItem menuSua, ToolStripMenuItem menuXoa, ToolStripMenuItem menuXuatExcel, ToolStripMenuItem menuCopy = null)
        {
            if (frmLogin.Admin == "True")
            {
                return;
            }
            string strRole = $@"SELECT * FROM MenuItem WHERE form ='{frm.Name}'";
            DataTable dtRole = new DataTable();
            dtRole = Models.SQL.GetData(strRole);
            if (dtRole.Rows.Count == 1)
            {
                if (!string.IsNullOrEmpty(dtRole.Rows[0]["New"].ToString()) && !dtRole.Rows[0]["New"].ToString().Contains(frmLogin.UserId))
                {
                    menuThem.Visible = false;
                }
                if (!string.IsNullOrEmpty(dtRole.Rows[0]["Edit"].ToString()) &&  !dtRole.Rows[0]["Edit"].ToString().Contains(frmLogin.UserId))
                {
                    menuSua.Visible = false;
                }
                if (!string.IsNullOrEmpty(dtRole.Rows[0]["Del"].ToString()) &&  !dtRole.Rows[0]["Del"].ToString().Contains(frmLogin.UserId))
                {
                    menuXoa.Visible = false;
                }
                if (!string.IsNullOrEmpty(dtRole.Rows[0]["Excel"].ToString()) &&  !dtRole.Rows[0]["Excel"].ToString().Contains(frmLogin.UserId))
                {
                    menuXuatExcel.Visible = false;
                }
                if (!string.IsNullOrEmpty(dtRole.Rows[0]["Copy"].ToString()) && !dtRole.Rows[0]["Copy"].ToString().Contains(frmLogin.UserId))
                {
                    menuXuatExcel.Visible = false;
                }
            }
            else
            {
                menuStrip.Visible = false;
            }
        }
    }
}
