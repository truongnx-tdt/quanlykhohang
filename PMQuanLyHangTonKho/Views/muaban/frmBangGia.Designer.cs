namespace PMQuanLyHangTonKho.Views.muaban
{
    partial class frmBangGia
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBangGia));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuThem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSua = new System.Windows.Forms.ToolStripMenuItem();
            this.menuXoa = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLamMoi = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTimKiem = new System.Windows.Forms.ToolStripMenuItem();
            this.dtgvMaster = new System.Windows.Forms.DataGridView();
            this.dtgvDetail = new System.Windows.Forms.DataGridView();
            this.menuXuatExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMaster)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).BeginInit();
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
            this.menuLamMoi,
            this.menuThoat,
            this.menuTimKiem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1063, 24);
            this.menuStrip.TabIndex = 438;
            this.menuStrip.Text = "menuStrip1";
            // 
            // menuThem
            // 
            this.menuThem.Image = ((System.Drawing.Image)(resources.GetObject("menuThem.Image")));
            this.menuThem.Name = "menuThem";
            this.menuThem.Size = new System.Drawing.Size(93, 20);
            this.menuThem.Text = "Thêm mới";
            this.menuThem.Click += new System.EventHandler(this.menuThem_Click);
            // 
            // menuSua
            // 
            this.menuSua.Image = ((System.Drawing.Image)(resources.GetObject("menuSua.Image")));
            this.menuSua.Name = "menuSua";
            this.menuSua.Size = new System.Drawing.Size(58, 20);
            this.menuSua.Text = "Sửa";
            this.menuSua.Click += new System.EventHandler(this.menuSua_Click);
            // 
            // menuXoa
            // 
            this.menuXoa.Image = ((System.Drawing.Image)(resources.GetObject("menuXoa.Image")));
            this.menuXoa.Name = "menuXoa";
            this.menuXoa.Size = new System.Drawing.Size(57, 20);
            this.menuXoa.Text = "Xóa";
            this.menuXoa.Click += new System.EventHandler(this.menuXoa_Click);
            // 
            // menuLamMoi
            // 
            this.menuLamMoi.Image = ((System.Drawing.Image)(resources.GetObject("menuLamMoi.Image")));
            this.menuLamMoi.Name = "menuLamMoi";
            this.menuLamMoi.Size = new System.Drawing.Size(84, 20);
            this.menuLamMoi.Text = "Làm mới";
            this.menuLamMoi.Click += new System.EventHandler(this.menuLamMoi_Click);
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
            this.menuTimKiem.Click += new System.EventHandler(this.menuTimKiem_Click);
            // 
            // dtgvMaster
            // 
            this.dtgvMaster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgvMaster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvMaster.Location = new System.Drawing.Point(0, 27);
            this.dtgvMaster.Name = "dtgvMaster";
            this.dtgvMaster.Size = new System.Drawing.Size(1063, 306);
            this.dtgvMaster.TabIndex = 439;
            // 
            // dtgvDetail
            // 
            this.dtgvDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvDetail.Location = new System.Drawing.Point(0, 339);
            this.dtgvDetail.Name = "dtgvDetail";
            this.dtgvDetail.Size = new System.Drawing.Size(1063, 326);
            this.dtgvDetail.TabIndex = 440;
            // 
            // menuXuatExcel
            // 
            this.menuXuatExcel.Image = ((System.Drawing.Image)(resources.GetObject("menuXuatExcel.Image")));
            this.menuXuatExcel.Name = "menuXuatExcel";
            this.menuXuatExcel.Size = new System.Drawing.Size(94, 20);
            this.menuXuatExcel.Text = "Xuất Excel";
            // 
            // frmBangGia
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 665);
            this.Controls.Add(this.dtgvDetail);
            this.Controls.Add(this.dtgvMaster);
            this.Controls.Add(this.menuStrip);
            this.Name = "frmBangGia";
            this.Text = "Bảng giá";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMaster)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuThem;
        private System.Windows.Forms.ToolStripMenuItem menuSua;
        private System.Windows.Forms.ToolStripMenuItem menuXoa;
        private System.Windows.Forms.ToolStripMenuItem menuLamMoi;
        private System.Windows.Forms.ToolStripMenuItem menuThoat;
        private System.Windows.Forms.ToolStripMenuItem menuTimKiem;
        private System.Windows.Forms.DataGridView dtgvMaster;
        private System.Windows.Forms.DataGridView dtgvDetail;
        private System.Windows.Forms.BindingSource binSource;
        private System.Windows.Forms.ToolStripMenuItem menuXuatExcel;
    }
}