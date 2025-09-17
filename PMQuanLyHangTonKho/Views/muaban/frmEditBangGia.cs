using PMQuanLyHangTonKho.Lib;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PMQuanLyHangTonKho.Views.muaban
{
    public partial class frmEditBangGia : Form
    {
        string strQueryDetail =
            "SELECT a.ProductId, b.Name AS ProductsName, " +
            "CONVERT(VARCHAR, a.FromQty) AS FromQty, " +
            "CONVERT(VARCHAR, a.ToQty) AS ToQty, " +
            "CONVERT(VARCHAR, a.UnitPrice) AS UnitPrice, " +
            "a.Notes " +
            "FROM PriceListDetail a INNER JOIN Products b ON a.ProductId = b.Id";

        string[] columnsNameDetail = new string[]
        { "Mã sản phẩm", "Tên sản phẩm", "Từ SL", "Đến SL", "Đơn giá", "Ghi chú" };

        int[] widthDetail = new int[] { 120, 250, 100, 100, 120, 250 };

        DataTable dtDetail = new DataTable();

        // ====== LOOKUP DANH MỤC ======
        string strQueryProduct =
            "SELECT CAST(0 AS BIT) AS xtag, Id, Name, ProductCategoryId, Unit, MinimumStock, MaximumStock, Description, ExpirationDate, Status FROM Products";
        string[] columnsProductText = new string[]
        { "Chọn", "Mã hàng hóa", "Tên hàng hóa", "Mã nhóm", "Đvt", "Tồn tối thiểu", "Tồn tối đa", "Mô tả", "Hạn sử dụng", "Trạng thái" };
        int[] widthProduct = new int[] { 60, 120, 250, 120, 100, 120, 120, 220, 120, 140 };

        public frmEditBangGia()
        {
            InitializeComponent();
            this.KeyPreview = true;
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);

            // Events
            this.Load += frm_Load;
            this.btnSave.Click += btnSave_Click;
            this.btnExit.Click += (s, e) => this.Close();

            this.dtgvDetail.CellEndEdit += dtgvDetail_CellEndEdit;
            this.dtgvDetail.CellValidating += dtgvDetail_CellValidating;
            this.dtgvDetail.KeyDown += dtgvDetail_KeyDown;
        }

        public void SetValue(BindingSource binSource)
        {
            txtId.DataBindings.Add("Text", binSource, "Id");
            txtName.DataBindings.Add("Text", binSource, "Name");
            cboType.DataBindings.Clear();
            cboType.DataBindings.Add("SelectedValue", binSource, "Type",
                                     true, DataSourceUpdateMode.Never);
            dtpStartDate.DataBindings.Add("Text", binSource, "StartDate");
            dtpEndDate.DataBindings.Add("Text", binSource, "EndDate");
            chkActive.DataBindings.Add("Checked", binSource, "IsActive");
            txtNote.DataBindings.Add("Text", binSource, "Notes");
        }

        // ====== LOAD FORM ======
        private void frm_Load(object sender, EventArgs e)
        {
            if (frmBangGia.isSave && !frmBangGia.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("BG");
                chkActive.Checked = true;
                if (cboType.SelectedValue == null || string.IsNullOrWhiteSpace(cboType.SelectedValue.ToString()))
                    cboType.SelectedValue = "S";
            }
            else if (frmBangGia.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("BG");
            }

            string key = " WHERE PriceListId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + key, columnsNameDetail, widthDetail, out dtDetail);

            dtgvDetail.Columns["ProductsName"].ReadOnly = true;

            var dtType = new DataTable();
            dtType.Columns.Add("Value", typeof(string));
            dtType.Columns.Add("Text", typeof(string));
            dtType.Rows.Add("P", "Mua");
            dtType.Rows.Add("S", "Bán");

            cboType.DisplayMember = "Text";
            cboType.ValueMember = "Value";
            cboType.DataSource = dtType;
            cboType.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // ====== LƯU ======
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate header
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                Alert.Error("Chưa nhập Tên bảng giá.");
                txtName.Focus();
                return;
            }

            var typeVal = (cboType.SelectedValue ?? "").ToString();
            if (typeVal != "P" && typeVal != "S")
            {
                Alert.Error("Loại bảng giá phải là 'P' (Mua) hoặc 'S' (Bán).");
                cboType.Focus();
                return;
            }

            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                var r = dtgvDetail.Rows[i];
                var prod = r.Cells["ProductId"].Value;
                var from = r.Cells["FromQty"].Value;
                var price = r.Cells["UnitPrice"].Value;

                bool isProdEmpty = prod == null || string.IsNullOrWhiteSpace(prod.ToString());
                bool isFromEmpty = from == null || from == DBNull.Value || from.ToString() == "" || Convert.ToDecimal(from) == 0;
                bool isPriceEmpty = price == null || price == DBNull.Value || price.ToString() == "" || Convert.ToDecimal(price) == 0;

                bool emptyRow = isProdEmpty || isFromEmpty || isPriceEmpty;

                if (emptyRow)
                {
                    dtgvDetail.Rows.RemoveAt(i);
                    continue;
                }

                if (prod == null || string.IsNullOrWhiteSpace(prod.ToString()))
                {
                    dtgvDetail.Rows.RemoveAt(i);
                    continue;
                }
                bool exists = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id = '" + prod.ToString() + "'");
                if (!exists)
                {
                    dtgvDetail.Rows.RemoveAt(i);
                    continue;
                }
            }

            if (dtgvDetail.Rows.Count == 0)
            {
                Alert.Question("Bảng giá chưa có dòng. Bạn vẫn muốn lưu?", out bool isOk);
                if (!isOk) return;
            }

            if (frmBangGia.isSave || frmBangGia.isCopy)
            {
                // INSERT
                string sqlIns =
                    @"INSERT INTO PriceListMaster(Id, Name, Type, StartDate, EndDate, IsActive, Notes, DateCreate, UserCreate)
                      VALUES(@Id, @Name, @Type, @Start, @End, @Active, @Notes, GETDATE(), @User)";
                Models.SQL.RunQuery(sqlIns, new object[,]
                {
                    {"@Id",     txtId.Text.Trim()},
                    {"@Name",   txtName.Text.Trim()},
                    {"@Type",   (cboType.SelectedValue ?? "S").ToString()},
                    {"@Start",  dtpStartDate.Checked ? (object)dtpStartDate.Value.Date : DBNull.Value},
                    {"@End",    dtpEndDate.Checked   ? (object)dtpEndDate.Value.Date   : DBNull.Value},
                    {"@Active", chkActive.Checked ? 1 : 0},
                    {"@Notes",  txtNote.Text.Trim()},
                    {"@User",   frmLogin.UserName}
                });
            }
            else
            {
                // UPDATE
                string sqlUpd =
                    @"UPDATE PriceListMaster
                      SET Name=@Name, Type=@Type, StartDate=@Start, EndDate=@End,
                          IsActive=@Active, Notes=@Notes, DateUpdate=GETDATE(), UserUpdate=@User
                      WHERE Id=@Id";
                Models.SQL.RunQuery(sqlUpd, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Name", txtName.Text.Trim()},
                    {"@Type",   (cboType.SelectedValue ?? "S").ToString()},
                    {"@Start", dtpStartDate.Checked ? (object)dtpStartDate.Value.Date : DBNull.Value},
                    {"@End", dtpEndDate.Checked ? (object)dtpEndDate.Value.Date : DBNull.Value},
                    {"@Active", chkActive.Checked ? 1 : 0},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", frmLogin.UserName}
                });

                Models.SQL.RunQuery("DELETE FROM PriceListDetail WHERE PriceListId=@Id", new object[,] { { "@Id", txtId.Text.Trim() } });
            }
            // INSERT DETAIL
            foreach (DataGridViewRow r in dtgvDetail.Rows)
            {
                var prod = r.Cells["ProductId"].Value;
                var from = r.Cells["FromQty"].Value;
                var price = r.Cells["UnitPrice"].Value;
                if (prod == null || string.IsNullOrWhiteSpace(prod.ToString())) continue;
                bool exists = Models.SQL.FindExists("SELECT Id FROM Products WHERE Id = '" + prod.ToString() + "'");
                if (!exists) continue;
                decimal fromVal = 0, toVal = 0, priceVal = 0;
                try { fromVal = Convert.ToDecimal((from ?? "0").ToString().Replace(",", "").Trim()); } catch { }
                try { toVal = Convert.ToDecimal((r.Cells["ToQty"].Value ?? "0").ToString().Replace(",", "").Trim()); } catch { }
                try { priceVal = Convert.ToDecimal((price ?? "0").ToString().Replace(",", "").Trim()); } catch { }
                string sqlInsD =
                    @"INSERT INTO PriceListDetail(Id, PriceListId, ProductId, FromQty, ToQty, UnitPrice, Notes)
                      VALUES(@Id, @PriceListId, @ProductId, @FromQty, @ToQty, @UnitPrice, @Notes)";
                Models.SQL.RunQuery(sqlInsD, new object[,]
                {
                    {"@Id",           Guid.NewGuid().ToString()},
                    {"@PriceListId", txtId.Text.Trim()},
                    {"@ProductId",   prod.ToString().Trim()},
                    {"@FromQty",     fromVal},
                    {"@ToQty",       toVal},
                    {"@UnitPrice",   priceVal},
                    {"@Notes",       (r.Cells["Notes"].Value ?? "").ToString().Trim()}
                });
            }

            frmBangGia.isLoad = true;
            Alert.Infor("Đã lưu bảng giá.");
            this.Close();
        }

        // ====== GRID EVENTS ======
        private void dtgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var col = dtgvDetail.Columns[e.ColumnIndex].Name;

            if (col == "ProductId")
            {
                string input = (dtgvDetail.Rows[e.RowIndex].Cells["ProductId"].Value ?? "").ToString();
                string KeySearch = " WHERE Id LIKE N'%" + input + "%' OR Name LIKE N'%" + input + "%'";

                string outId, outName;
                Lookup.SearchLookupSingle(strQueryProduct, columnsProductText, widthProduct, input, out outId, out outName, KeySearch);

                dtgvDetail.Rows[e.RowIndex].Cells["ProductId"].Value = outId;
                dtgvDetail.Rows[e.RowIndex].Cells["ProductsName"].Value = outName;
            }
            else if (col == "FromQty" || col == "ToQty" || col == "UnitPrice")
            {
                // Chuẩn hóa số: bỏ dấu phẩy trước khi parse, sau đó format N0
                string raw = (dtgvDetail.Rows[e.RowIndex].Cells[col].Value ?? "0").ToString().Replace(",", "").Trim();
                if (string.IsNullOrEmpty(raw)) raw = "0";
                try
                {
                    decimal val = Convert.ToDecimal(raw);
                    string formatted = string.Format(CultureInfo.InvariantCulture, "{0:N0}", val);
                    dtgvDetail.Rows[e.RowIndex].Cells[col].Value = formatted;
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Lỗi định dạng số: " + ex.Message);
                    dtgvDetail.Rows[e.RowIndex].Cells[col].Value = "0";
                }
            }
        }

        private void dtgvDetail_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = dtgvDetail.Columns[e.ColumnIndex].Name;
            if (col == "FromQty" || col == "ToQty" || col == "UnitPrice")
            {
                string inputValue = e.FormattedValue.ToString().Trim();
                if (!string.IsNullOrEmpty(inputValue) && !System.Text.RegularExpressions.Regex.IsMatch(inputValue.Replace(",", ""), @"^\d+(\.\d+)?$"))
                {
                    e.Cancel = true;
                }
            }
        }

        private void dtgvDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                dtDetail.Rows.Add(dtDetail.NewRow());
            }
            if (e.KeyCode == Keys.F8)
            {
                DeleteCurrentDetailRow();
            }
        }

        private void DeleteCurrentDetailRow()
        {
            if (dtgvDetail.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgvDetail.SelectedRows[0];
                dtDetail.Rows.RemoveAt(selectedRow.Index);
            }
            else
            {
                Alert.Error("Vui lòng chọn một dòng để xóa!");
            }
        }
    }
}
