using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace PMQuanLyHangTonKho.Lib
{
    public class Lookup
    {
        public static void SearchLookupSingle(string strQuery, string[] columns, int[] width, string InputId, out string OutIdChoose, out string OutNameChoose, string KeySearch = null)
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
            CssDatagridview.LoadData(null, dtgvMain, strQuery, columns, width, true, false, KeySearch);
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

        public static void SearchLookupMulti(
    string strQuery,
    string[] columns,
    int[] width,
    out DataTable outSelected,
    string KeySearch = null)
        {
            outSelected = null;

            // ==== UI ====
            Form frm = new Form();
            frm.Size = new Size(700, 480);
            DLLSystem.Init(frm);

            Label lblId = new Label { Text = "Lọc theo mã", Font = new Font("Tahoma", 9), Location = new Point(3, 12) };
            TextBox txtId = new TextBox { Location = new Point(105, 10), Width = 120 };

            Label lblName = new Label { Text = "Lọc theo tên", Font = new Font("Tahoma", 9), Location = new Point(240, 12) };
            TextBox txtName = new TextBox { Location = new Point(330, 10), Width = 180 };

            Button btnSearch = new Button();
            CssButton.StyleButton(btnSearch, "find.bmp");
            btnSearch.Size = new Size(90, 30);
            btnSearch.Location = new Point(540, 5);
            btnSearch.Text = "Tìm kiếm";

            DataGridView dtgvMain = new DataGridView
            {
                Location = new Point(0, 40),
                Size = new Size(684, 360),
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = true,
                EditMode = DataGridViewEditMode.EditOnEnter,
                AllowUserToAddRows = false,
                AutoGenerateColumns = true,      // Quan trọng
                ColumnHeadersVisible = true
            };

            // Nạp dữ liệu lần đầu
            CssDatagridview.LoadData(null, dtgvMain, strQuery, columns, width, true, false, KeySearch);

            // Sau MỌI lần bind, set lại header/width & readonly
            dtgvMain.DataBindingComplete += (s, e) =>
            {
                int colCount = dtgvMain.Columns.Count;

                if (columns != null)
                    for (int i = 0; i < Math.Min(columns.Length, colCount); i++)
                        dtgvMain.Columns[i].HeaderText = columns[i];

                if (width != null)
                    for (int i = 0; i < Math.Min(width.Length, colCount); i++)
                        dtgvMain.Columns[i].Width = width[i];

                // Chỉ cho sửa cột 0 (xtag)
                for (int i = 0; i < colCount; i++)
                    dtgvMain.Columns[i].ReadOnly = (i != 0);

                // Đảm bảo cột 0 là checkbox nếu LoadData trả về kiểu bool
                if (colCount > 0 && !(dtgvMain.Columns[0] is DataGridViewCheckBoxColumn))
                {
                    // Nếu type là bool, DataGridView sẽ tự render checkbox khi AutoGenerateColumns = true.
                    // Không cần thay column thủ công để tránh vỡ binding.
                }
            };

            // Commit tick ngay khi có thay đổi ở ô checkbox
            dtgvMain.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dtgvMain.IsCurrentCellDirty && dtgvMain.CurrentCell is DataGridViewCheckBoxCell)
                    dtgvMain.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };

            // Nút tìm kiếm: sau khi refresh dữ liệu, header/width sẽ được set lại bởi DataBindingComplete
            btnSearch.Click += (s, e) => BtnSearch_Click(dtgvMain, strQuery, columns, width, txtId, txtName);

            // Nút chọn: chốt edit trước khi đóng
            Button btnChoose = new Button();
            CssButton.StyleButton(btnChoose, "run.png");
            btnChoose.Size = new Size(130, 30);
            btnChoose.Location = new Point(frm.ClientSize.Width - btnChoose.Width - 5, frm.ClientSize.Height - btnChoose.Height - 8);
            btnChoose.Text = "Chọn các dòng";
            btnChoose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnChoose.Click += (s, e) =>
            {
                dtgvMain.EndEdit();
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };

            frm.Controls.AddRange(new Control[] { lblId, txtId, lblName, txtName, btnSearch, dtgvMain, btnChoose });
            if (frm.ShowDialog() != DialogResult.OK) { outSelected = null; return; }

            // ===== LẤY KẾT QUẢ ĐƯỢC TICK =====
            // Lấy bảng nguồn
            DataTable srcTable = null;
            if (dtgvMain.DataSource is DataTable t) srcTable = t;
            else if (dtgvMain.DataSource is BindingSource bs)
            {
                if (bs.DataSource is DataTable tb) srcTable = tb;
                else if (bs.List is DataView dv) srcTable = dv.Table;
            }
            else if (dtgvMain.DataSource is DataView dv) srcTable = dv.Table;

            // Fallback nếu vẫn null
            if (srcTable == null)
            {
                srcTable = new DataTable();
                for (int c = 0; c < dtgvMain.Columns.Count; c++)
                {
                    string name = string.IsNullOrWhiteSpace(dtgvMain.Columns[c].DataPropertyName)
                                    ? dtgvMain.Columns[c].Name
                                    : dtgvMain.Columns[c].DataPropertyName;
                    if (!srcTable.Columns.Contains(name))
                        srcTable.Columns.Add(name, typeof(object));
                }
            }

            // Clone schema & bỏ xtag
            DataTable result = srcTable.Clone();
            if (result.Columns.Contains("xtag"))
                result.Columns.Remove("xtag");

            foreach (DataGridViewRow row in dtgvMain.Rows)
            {
                if (row.IsNewRow) continue;

                bool picked = false;
                var v0 = row.Cells[0]?.Value;
                if (v0 != null) bool.TryParse(v0.ToString(), out picked);
                if (!picked) continue;

                DataRow nr = result.NewRow();
                var drv = row.DataBoundItem as DataRowView;
                if (drv != null)
                {
                    foreach (DataColumn col in drv.Row.Table.Columns)
                    {
                        if (col.ColumnName.Equals("xtag", StringComparison.OrdinalIgnoreCase)) continue;
                        if (result.Columns.Contains(col.ColumnName))
                            nr[col.ColumnName] = drv.Row[col.ColumnName];
                    }
                }
                else
                {
                    for (int c = 1; c < row.Cells.Count; c++)
                    {
                        var col = dtgvMain.Columns[c];
                        string colName = string.IsNullOrWhiteSpace(col.DataPropertyName) ? col.Name : col.DataPropertyName;
                        if (!result.Columns.Contains(colName)) result.Columns.Add(colName, typeof(object));
                        nr[colName] = row.Cells[c].Value ?? DBNull.Value;
                    }
                }
                result.Rows.Add(nr);
            }

            outSelected = (result.Rows.Count > 0) ? result : null;
        }

        private static void BtnChoose_Click(Form frm)
        {
            frm.Close();
        }
        private static void BtnSearch_Click(DataGridView dgvMain, string strQuery, string[] columns, int[] width, TextBox txtId, TextBox txtName)
        {
            strQuery += " WHERE Id LIKE N'%" + txtId.Text + "%' AND Name LIKE N'%" + txtName.Text + "%'";
            CssDatagridview.LoadData(null, dgvMain, strQuery, columns, width, true, false);
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
