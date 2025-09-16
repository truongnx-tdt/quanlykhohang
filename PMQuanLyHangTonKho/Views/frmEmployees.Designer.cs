namespace PMQuanLyHangTonKho.Views
{
    partial class frmEmployees
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEmployees));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuThem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSua = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXoa = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXuatExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLamMoi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTimKiem = new System.Windows.Forms.ToolStripMenuItem();
            this.dtgvMain = new System.Windows.Forms.DataGridView();
            this.binSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.binSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuThem,
            this.menuSua,
            this.menuXoa,
            this.menuXuatExcel,
            this.menuCopy,
            this.menuLamMoi,
            this.menuThoat,
            this.menuTimKiem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(800, 24);
            this.menuStrip.TabIndex = 431;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuThem
            // 
            this.menuThem.Image = ((System.Drawing.Image)(resources.GetObject("menuThem.Image")));
            this.menuThem.Name = "menuThem";
            this.menuThem.Size = new System.Drawing.Size(93, 20);
            this.menuThem.Text = "Thêm mới";
            // 
            // menuSua
            // 
            this.menuSua.Image = ((System.Drawing.Image)(resources.GetObject("menuSua.Image")));
            this.menuSua.Name = "menuSua";
            this.menuSua.Size = new System.Drawing.Size(58, 20);
            this.menuSua.Text = "Sửa";
            // 
            // menuXoa
            // 
            this.menuXoa.Image = ((System.Drawing.Image)(resources.GetObject("menuXoa.Image")));
            this.menuXoa.Name = "menuXoa";
            this.menuXoa.Size = new System.Drawing.Size(57, 20);
            this.menuXoa.Text = "Xóa";
            // 
            // menuXuatExcel
            // 
            this.menuXuatExcel.Image = ((System.Drawing.Image)(resources.GetObject("menuXuatExcel.Image")));
            this.menuXuatExcel.Name = "menuXuatExcel";
            this.menuXuatExcel.Size = new System.Drawing.Size(94, 20);
            this.menuXuatExcel.Text = "Xuất Excel";
            // 
            // menuLamMoi
            // 
            this.menuLamMoi.Image = ((System.Drawing.Image)(resources.GetObject("menuLamMoi.Image")));
            this.menuLamMoi.Name = "menuLamMoi";
            this.menuLamMoi.Size = new System.Drawing.Size(84, 20);
            this.menuLamMoi.Text = "Làm mới";
            // 
            // menuThoat
            // 
            this.menuThoat.Image = ((System.Drawing.Image)(resources.GetObject("menuThoat.Image")));
            this.menuThoat.Name = "menuThoat";
            this.menuThoat.Size = new System.Drawing.Size(68, 20);
            this.menuThoat.Text = "Thoát";
            // 
            // menuTimKiem
            // 
            this.menuTimKiem.Image = ((System.Drawing.Image)(resources.GetObject("menuTimKiem.Image")));
            this.menuTimKiem.Name = "menuTimKiem";
            this.menuTimKiem.Size = new System.Drawing.Size(88, 20);
            this.menuTimKiem.Text = "Tìm kiếm";
            // 
            // dtgvMain
            // 
            this.dtgvMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvMain.Location = new System.Drawing.Point(1, 30);
            this.dtgvMain.Name = "dtgvMain";
            this.dtgvMain.Size = new System.Drawing.Size(797, 420);
            this.dtgvMain.TabIndex = 430;
            // 
            // menuCopy
            // 
            this.menuCopy.Image = ((System.Drawing.Image)(resources.GetObject("menuCopy.Image")));
            this.menuCopy.Name = "menuCopy";
            this.menuCopy.Size = new System.Drawing.Size(63, 20);
            this.menuCopy.Text = "Copy";
            // 
            // frmEmployees
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.dtgvMain);
            this.Name = "frmEmployees";
            this.Text = "Danh mục nhân viên";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.binSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuThem;
        private System.Windows.Forms.ToolStripMenuItem menuSua;
        private System.Windows.Forms.ToolStripMenuItem menuXoa;
        private System.Windows.Forms.ToolStripMenuItem menuXuatExcel;
        private System.Windows.Forms.ToolStripMenuItem menuLamMoi;
        private System.Windows.Forms.ToolStripMenuItem menuThoat;
        private System.Windows.Forms.ToolStripMenuItem menuTimKiem;
        private System.Windows.Forms.DataGridView dtgvMain;
        private System.Windows.Forms.BindingSource binSource;
        private System.Windows.Forms.ToolStripMenuItem menuCopy;
    }
}