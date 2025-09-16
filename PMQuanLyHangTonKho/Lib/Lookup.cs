using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public class Lookup
    {
        public static void SearchLookupSingle(string strQuery, string[] columns, int[] width, string InputId, out string OutIdChoose, out string OutNameChoose,string KeySearch = null)
        {
            OutIdChoose = string.Empty;
            OutNameChoose = string.Empty;
            Form frm = new Form();
            frm.Size = new Size(550, 400);
            DLLSystem.Init(frm);
            Label lblId = new Label();
            lblId.Text = "Lọc theo mã";
            lblId.Font = new Font("Tahoma", 9);
            lblId.Location = new Point(3, 12);

            TextBox txtId = new TextBox();
            txtId.Location = new Point(105, 10);

            Label lblName = new Label();
            lblName.Text = "Lọc theo tên";
            lblName.Font = new Font("Tahoma", 9);
            lblName.Location = new Point(220, 12);

            TextBox txtName = new TextBox();
            txtName.Location = new Point(320, 10);

            DataGridView dtgvMain = new DataGridView();
            dtgvMain.Size = new Size(533, 276);
            dtgvMain.Location = new Point(0, 40);
            CssDatagridview.LoadData(null, dtgvMain, strQuery, columns, width, true, false,KeySearch);
            frm.Controls.Add(dtgvMain);
            Button btnSearch = new Button();
            CssButton.StyleButton(btnSearch, "find.bmp");
            btnSearch.Size = new Size(90, 30);
            btnSearch.Location = new Point(440, 5);
            btnSearch.Text = "Tìm kiếm";
            btnSearch.Click += (sender, e) => BtnSearch_Click(dtgvMain, strQuery, columns, width, txtId, txtName);
            frm.Controls.Add(lblId);
            frm.Controls.Add(lblName);
            frm.Controls.Add(txtId);
            frm.Controls.Add(txtName);
            frm.Controls.Add(btnSearch);
            for (int i = 0; i < columns.Length; i++)
            {
                dtgvMain.Columns[i].HeaderText = columns[i];
            }
            for (int i = 0; i < width.Length; i++)
            {
                dtgvMain.Columns[i].Width = width[i];
            }
            //Bỏ Readonly cột đầu tiên
            for (int i = 0; i < dtgvMain.Columns.Count; i++)
            {
                if (i != 0)
                {
                    dtgvMain.Columns[i].ReadOnly = true;
                }
            }
            //Check giá trị đã chọn
            if (!string.IsNullOrEmpty(InputId))
            {
                foreach (DataGridViewRow row in dtgvMain.Rows)
                {
                    if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() == InputId)
                    {
                        row.Cells[0].Value = true;
                    }
                }
            }
            dtgvMain.CellContentClick += (sender, e) => dtgvMain_CellContentClick(dtgvMain, sender, e);
            Button btnChoose = new Button();
            CssButton.StyleButton(btnChoose, "run.png");
            btnChoose.Size = new Size(110, 30);
            btnChoose.Location = new Point(frm.ClientSize.Width - btnChoose.Width - 5, frm.ClientSize.Height - btnChoose.Height - 8);
            btnChoose.Text = "Chọn dữ liệu";
            btnChoose.Click += (sender, e) => BtnChoose_Click(frm);
            frm.Controls.Add(btnChoose);
            frm.ShowDialog();
            Boolean isCheck = false;
            foreach (DataGridViewRow row in dtgvMain.Rows)
            {
                var a = row.Cells[0].Value.ToString();
                if (Convert.ToBoolean(row.Cells[0].Value.ToString()) == true)
                {
                    isCheck = true;
                    OutIdChoose = row.Cells[1].Value.ToString();
                    OutNameChoose = row.Cells[2].Value.ToString();
                    break;
                }
            }
            if (!isCheck)
            {
                OutIdChoose = "";
                OutNameChoose = "";
            }
        }

        private static void dtgvMain_CellContentClick(DataGridView dtgvMain, object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                bool isChecked = (bool)dtgvMain.Rows[e.RowIndex].Cells[0].Value;
                foreach (DataGridViewRow row in dtgvMain.Rows)
                {
                    row.Cells[0].Value = false;
                }
                if (isChecked)
                {
                    dtgvMain.Rows[e.RowIndex].Cells[0].Value = true;
                }
                dtgvMain.CurrentCell = dtgvMain.Rows[e.RowIndex].Cells[1];
            }
        }


        private static void BtnChoose_Click(Form frm)
        {
            frm.Close();
        }
        private static void BtnSearch_Click(DataGridView dgvMain, string strQuery, string[] columns, int[] width, TextBox txtId, TextBox txtName)
        {
            strQuery += " WHERE Id LIKE N'%" + txtId.Text + "%' AND Name LIKE N'%" + txtName.Text + "%'";
            CssDatagridview.LoadData(null, dgvMain, strQuery, columns, width, true,false);
            for (int i = 0; i < dgvMain.Columns.Count; i++)
            {
                if (i != 0)
                {
                    dgvMain.Columns[i].ReadOnly = true;
                }
            }
        }
    }
}
