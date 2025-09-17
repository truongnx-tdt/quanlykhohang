using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public class CssDatagridview
    {
        public static void StyleDatagridview(DataGridView dgvMain, Boolean isReadOnly = true)
        {
            dgvMain.AllowUserToAddRows = false;
            dgvMain.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMain.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvMain.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvMain.ColumnHeadersHeight = 30;
            dgvMain.ReadOnly = isReadOnly;
            dgvMain.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
            dgvMain.EnableHeadersVisualStyles = false;
            dgvMain.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvMain.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 9, FontStyle.Bold);
            dgvMain.BackgroundColor = Color.White;
            dgvMain.BorderStyle = BorderStyle.Fixed3D;
        }
        public static void LoadData(BindingSource binSource, DataGridView dgvMain, string strQuery, string[] columns, int[] width, Boolean isColumns = false, Boolean isReadOnly = true, string KeySearch = null)
        {
            DataTable dt = new DataTable();
            dt = Models.SQL.GetData(KeySearch != null ? strQuery + KeySearch : strQuery);
            if (binSource != null)
            {
                binSource.DataSource = dt;
                dgvMain.DataSource = binSource;
            }
            else
            {
                dgvMain.DataSource = dt;
            }
            if (!isColumns)
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    dgvMain.Columns[i].HeaderText = columns[i];
                }
                for (int i = 0; i < width.Length; i++)
                {
                    dgvMain.Columns[i].Width = width[i];
                }
                if (dgvMain.Columns.Contains("Type"))
                    dgvMain.Columns["Type"].Visible = false;
                if (dgvMain.Columns.Contains("IsActive"))
                    dgvMain.Columns["IsActive"].Visible = false;
            }
            StyleDatagridview(dgvMain, isReadOnly);
            dt.Dispose();
        }
        public static void LoadDataDetailVoucher(DataGridView dgvMain, string strQuery, string[] columns, int[] width, out DataTable dt)
        {
            dt = new DataTable();
            dt = Models.SQL.GetData(strQuery);
            dgvMain.DataSource = dt;
            for (int i = 0; i < columns.Length; i++)
            {
                dgvMain.Columns[i].HeaderText = columns[i];
            }
            for (int i = 0; i < width.Length; i++)
            {
                dgvMain.Columns[i].Width = width[i];
            }
            StyleDatagridview(dgvMain, false);
            dt.Dispose();
        }
    }
}
