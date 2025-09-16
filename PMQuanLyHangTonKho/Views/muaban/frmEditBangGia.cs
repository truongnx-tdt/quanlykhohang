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
            "SELECT a.ProductsId, b.Name AS ProductsName, " +
            "CONVERT(VARCHAR, a.FromQty) AS FromQty, " +
            "CONVERT(VARCHAR, a.ToQty) AS ToQty, " +
            "CONVERT(VARCHAR, a.UnitPrice) AS UnitPrice, " +
            "a.Note " +
            "FROM PriceListDetail a INNER JOIN Products b ON a.ProductsId = b.Id";

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
            txtType.DataBindings.Add("Text", binSource, "Type");          // 'P' or 'S'
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
                if (string.IsNullOrWhiteSpace(txtType.Text)) txtType.Text = "S"; // mặc định bảng giá bán
            }
            else if (frmBangGia.isCopy)
            {
                txtId.Text = DLLSystem.GenCode("BG");
            }

            // Nạp detail
            string key = " WHERE PriceListId = '" + txtId.Text + "'";
            Lib.CssDatagridview.LoadDataDetailVoucher(dtgvDetail, strQueryDetail + key, columnsNameDetail, widthDetail, out dtDetail);

            // Khóa cột tên hàng - tự fill theo ProductsId
            dtgvDetail.Columns["ProductsName"].ReadOnly = true;

            // Nếu bạn dùng combobox cho Type, có thể set sẵn list P/S (ở Designer)
            // Nếu dùng TextBox thì giữ như hiện tại (txtType).
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
            if (txtType.Text != "P" && txtType.Text != "S")
            {
                Alert.Error("Loại bảng giá phải là 'P' (Mua) hoặc 'S' (Bán).");
                txtType.Focus();
                return;
            }

            // Làm sạch detail: bỏ dòng trống hoặc sai mã hàng
            for (int i = dtgvDetail.Rows.Count - 1; i >= 0; i--)
            {
                var r = dtgvDetail.Rows[i];
                var prod = r.Cells["ProductsId"].Value;
                var from = r.Cells["FromQty"].Value;
                var price = r.Cells["UnitPrice"].Value;

                bool emptyRow = (prod == null || string.IsNullOrWhiteSpace(prod.ToString()))
                                && (from == null || string.IsNullOrWhiteSpace(from.ToString()))
                                && (price == null || string.IsNullOrWhiteSpace(price.ToString()));
                if (emptyRow)
                {
                    dtgvDetail.Rows.RemoveAt(i);
                    continue;
                }

                // Xóa dòng thiếu mã hoặc không tồn tại trong Products
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

            // Check có chi tiết chưa (có thể cho phép rỗng nếu bạn muốn tạo trước)
            if (dtgvDetail.Rows.Count == 0)
            {
                if (!Alert.QuestionYesNo("Bảng giá chưa có dòng. Bạn vẫn muốn lưu?")) return;
            }

            // Lưu header (dùng SQL trực tiếp cho chắc chắn)
            if (frmBangGia.isSave || frmBangGia.isCopy)
            {
                // INSERT
                string sqlIns =
                    @"INSERT INTO PriceListMaster(Id, Name, Type, StartDate, EndDate, IsActive, Notes, DateCreate, UserCreate)
                      VALUES(@Id, @Name, @Type, @Start, @End, @Active, @Notes, GETDATE(), @User)";
                Models.SQL.RunQuery(sqlIns, new object[,]
                {
                    {"@Id", txtId.Text.Trim()},
                    {"@Name", txtName.Text.Trim()},
                    {"@Type", txtType.Text.Trim()},
                    {"@Start", dtpStartDate.Checked ? (object)dtpStartDate.Value.Date : DBNull.Value},
                    {"@End", dtpEndDate.Checked ? (object)dtpEndDate.Value.Date : DBNull.Value},
                    {"@Active", chkActive.Checked ? 1 : 0},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", DLLSystem.UserName}
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
                    {"@Type", txtType.Text.Trim()},
                    {"@Start", dtpStartDate.Checked ? (object)dtpStartDate.Value.Date : DBNull.Value},
                    {"@End", dtpEndDate.Checked ? (object)dtpEndDate.Value.Date : DBNull.Value},
                    {"@Active", chkActive.Checked ? 1 : 0},
                    {"@Notes", txtNote.Text.Trim()},
                    {"@User", DLLSystem.UserName}
                });

                // Xóa detail cũ để ghi lại (đơn giản, đúng với SaveOrUpdateDetail style)
                Models.SQL.RunQuery("DELETE FROM PriceListDetail WHERE PriceListId=@Id", new object[,] { { "@Id", txtId.Text.Trim() } });
            }

            // Lưu detail: tận dụng helper giống phiếu nhập/xuất
            // Grid phải có các cột đúng tên trường DB: ProductsId, FromQty, ToQty, UnitPrice, Note
            ListEdit.SaveOrUpdateDetail(dtgvDetail, "PriceListDetail", "PriceListId", txtId.Text.Trim());

            // Thông báo cho list reload
            frmBangGia.isLoad = true;
            Alert.Success("Đã lưu bảng giá.");
            this.Close();
        }

        // ====== GRID EVENTS ======
        private void dtgvDetail_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var col = dtgvDetail.Columns[e.ColumnIndex].Name;

            if (col == "ProductsId")
            {
                string input = (dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value ?? "").ToString();
                string KeySearch = " WHERE Id LIKE N'%" + input + "%' OR Name LIKE N'%" + input + "%'";

                string outId, outName;
                Lookup.SearchLookupSingle(strQueryProduct, columnsProductText, widthProduct, input, out outId, out outName, KeySearch);

                dtgvDetail.Rows[e.RowIndex].Cells["ProductsId"].Value = outId;
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
