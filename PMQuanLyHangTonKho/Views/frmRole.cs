using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmRole : Form
    {
        public frmRole(string id)
        {
            InitializeComponent();
            string strQuery = "SELECT Id,Name,CAST(CASE WHEN see LIKE '%" + id + ",%' THEN 1 ELSE 0 END AS BIT) AS See ";
            strQuery += ",CAST(CASE WHEN New LIKE '%" + id + ",%' THEN 1 ELSE 0 END AS BIT) AS New";
            strQuery += ",CAST(CASE WHEN Edit LIKE '%" + id + ",%' THEN 1 ELSE 0 END AS BIT) AS Edit";
            strQuery += ",CAST(CASE WHEN Del LIKE '%" + id + ",%' THEN 1 ELSE 0 END AS BIT) AS Del";
            strQuery += ",CAST(CASE WHEN Excel LIKE '%" + id + ",%' THEN 1 ELSE 0 END AS BIT) AS Excel";
            strQuery += ",CAST(CASE WHEN Copy LIKE '%" + id + ",%' THEN 1 ELSE 0 END AS BIT) AS Copy";
            strQuery += " FROM MenuItem ORDER BY Id";
            string[] columnsName = new string[] { "Mã", "Tên menu", "Xem", "Thêm", "Sửa", "Xóa", "Xuất Excel","Copy" };
            int[] width = new int[] { 30, 250, 100, 100, 100, 100, 100,100 };
            DLLSystem.Init(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            CssDatagridview.LoadData(null, dtgvMain, strQuery, columnsName, width, false, false);
            for (int i = 0; i < dtgvMain.Columns.Count; i++)
            {
                if (i == 0 || i == 1)
                {
                    dtgvMain.Columns[i].ReadOnly = true;
                }
            }
            dtgvMain.KeyDown += DtgvMain_KeyDown;
            btnSave.Click += (sender, e) => BtnSave_Click(dtgvMain, id);
        }

        private void BtnSave_Click(DataGridView dtgvMain, string Id)
        {
            string strUpdate = "";
            for (int i = 0; i < dtgvMain.Rows.Count; i++)
            {
                Boolean See = Convert.ToBoolean(dtgvMain.Rows[i].Cells["See"].Value);
                Boolean New = Convert.ToBoolean(dtgvMain.Rows[i].Cells["New"].Value);
                Boolean Edit = Convert.ToBoolean(dtgvMain.Rows[i].Cells["Edit"].Value);
                Boolean Del = Convert.ToBoolean(dtgvMain.Rows[i].Cells["Del"].Value);
                Boolean Excel = Convert.ToBoolean(dtgvMain.Rows[i].Cells["Excel"].Value);
                Boolean Copy = Convert.ToBoolean(dtgvMain.Rows[i].Cells["Copy"].Value);
                if (See)
                {
                    strUpdate += $@" UPDATE MenuItem SET See = ISNULL(See,'') + '{Id},' WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (!See)
                {
                    strUpdate += $@" UPDATE MenuItem SET See = REPLACE(ISNULL(See,''),'{Id},','') WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (New)
                {
                    strUpdate += $@" UPDATE MenuItem SET New = ISNULL(New,'') + '{Id},' WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (!New)
                {
                    strUpdate += $@" UPDATE MenuItem SET New = REPLACE(ISNULL(New,''),'{Id},','') WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (Edit)
                {
                    strUpdate += $@" UPDATE MenuItem SET Edit = ISNULL(Edit,'') + '{Id},' WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (!Edit)
                {
                    strUpdate += $@" UPDATE MenuItem SET Edit = REPLACE(ISNULL(Edit,''),'{Id},','') WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (Del)
                {
                    strUpdate += $@" UPDATE MenuItem SET Del = ISNULL(Del,'') + '{Id},' WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (!Del)
                {
                    strUpdate += $@" UPDATE MenuItem SET Del = REPLACE(ISNULL(Del,''),'{Id},','') WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (Excel)
                {
                    strUpdate += $@" UPDATE MenuItem SET Excel = ISNULL(Excel,'') + '{Id},' WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (!Excel)
                {
                    strUpdate += $@" UPDATE MenuItem SET Excel = REPLACE(ISNULL(Excel,''),'{Id},','') WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (Copy)
                {
                    strUpdate += $@" UPDATE MenuItem SET Copy = ISNULL(Copy,'') + '{Id},' WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
                if (!Copy)
                {
                    strUpdate += $@" UPDATE MenuItem SET Copy = REPLACE(ISNULL(Copy,''),'{Id},','') WHERE Id = {dtgvMain.Rows[i].Cells["Id"].Value}";
                }
            }
            Models.SQL.RunQuery(strUpdate);
            this.Close();
        }

        private void DtgvMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.B)
            {
                int rowIndex = dtgvMain.CurrentCell.RowIndex;

                foreach (DataGridViewCell cell in dtgvMain.Rows[rowIndex].Cells)
                {
                    if (cell is DataGridViewCheckBoxCell)
                    {
                        cell.Value = true; 
                    }
                }
            }
            if (e.Control && e.KeyCode == Keys.U)
            {
                int rowIndex = dtgvMain.CurrentCell.RowIndex;

                foreach (DataGridViewCell cell in dtgvMain.Rows[rowIndex].Cells)
                {
                    if (cell is DataGridViewCheckBoxCell)
                    {
                        cell.Value = false;
                    }
                }
            }
        }
    }
}
