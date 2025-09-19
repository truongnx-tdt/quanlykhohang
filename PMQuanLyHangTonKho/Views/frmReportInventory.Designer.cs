namespace PMQuanLyHangTonKho.Views
{
    partial class frmReportInventory
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
            this.dtgvMain = new System.Windows.Forms.DataGridView();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnXuatExcel = new System.Windows.Forms.Button();
            this.btnIn = new System.Windows.Forms.Button();
            this.lblWarehouseName = new System.Windows.Forms.Label();
            this.btnChooseWarehouse = new System.Windows.Forms.Button();
            this.txtWarehouseId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // dtgvMain
            // 
            this.dtgvMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtgvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvMain.Location = new System.Drawing.Point(2, 130);
            this.dtgvMain.Name = "dtgvMain";
            this.dtgvMain.Size = new System.Drawing.Size(797, 419);
            this.dtgvMain.TabIndex = 433;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(141, 19);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(150, 20);
            this.dtpFrom.TabIndex = 436;
            this.dtpFrom.Tag = "dtp";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(109, 14);
            this.label6.TabIndex = 435;
            this.label6.Text = "Từ ngày/đến ngày";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(297, 19);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(150, 20);
            this.dtpTo.TabIndex = 437;
            this.dtpTo.Tag = "dtp";
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.Control;
            this.btnFind.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFind.Location = new System.Drawing.Point(575, 11);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(99, 35);
            this.btnFind.TabIndex = 438;
            this.btnFind.Text = "Lọc dữ liệu";
            this.btnFind.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFind.UseVisualStyleBackColor = false;
            // 
            // btnXuatExcel
            // 
            this.btnXuatExcel.BackColor = System.Drawing.SystemColors.Control;
            this.btnXuatExcel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXuatExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXuatExcel.Location = new System.Drawing.Point(680, 52);
            this.btnXuatExcel.Name = "btnXuatExcel";
            this.btnXuatExcel.Size = new System.Drawing.Size(99, 35);
            this.btnXuatExcel.TabIndex = 439;
            this.btnXuatExcel.Text = "Xuất Excel";
            this.btnXuatExcel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnXuatExcel.UseVisualStyleBackColor = false;
            // 
            // btnIn
            // 
            this.btnIn.BackColor = System.Drawing.SystemColors.Control;
            this.btnIn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnIn.Location = new System.Drawing.Point(680, 11);
            this.btnIn.Name = "btnIn";
            this.btnIn.Size = new System.Drawing.Size(99, 35);
            this.btnIn.TabIndex = 440;
            this.btnIn.Text = "In báo cáo";
            this.btnIn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnIn.UseVisualStyleBackColor = false;
            // 
            // lblWarehouseName
            // 
            this.lblWarehouseName.AutoSize = true;
            this.lblWarehouseName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouseName.Location = new System.Drawing.Point(317, 53);
            this.lblWarehouseName.Name = "lblWarehouseName";
            this.lblWarehouseName.Size = new System.Drawing.Size(53, 14);
            this.lblWarehouseName.TabIndex = 499;
            this.lblWarehouseName.Text = "Tên kho";
            // 
            // btnChooseWarehouse
            // 
            this.btnChooseWarehouse.Location = new System.Drawing.Point(292, 50);
            this.btnChooseWarehouse.Name = "btnChooseWarehouse";
            this.btnChooseWarehouse.Size = new System.Drawing.Size(24, 22);
            this.btnChooseWarehouse.TabIndex = 498;
            this.btnChooseWarehouse.Text = "...";
            this.btnChooseWarehouse.UseVisualStyleBackColor = true;
            // 
            // txtWarehouseId
            // 
            this.txtWarehouseId.Location = new System.Drawing.Point(141, 52);
            this.txtWarehouseId.Name = "txtWarehouseId";
            this.txtWarehouseId.ReadOnly = true;
            this.txtWarehouseId.Size = new System.Drawing.Size(150, 20);
            this.txtWarehouseId.TabIndex = 497;
            this.txtWarehouseId.Tag = "txtNB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(25, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 14);
            this.label3.TabIndex = 496;
            this.label3.Text = "Mã kho";
            // 
            // frmReportInventory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 551);
            this.Controls.Add(this.lblWarehouseName);
            this.Controls.Add(this.btnChooseWarehouse);
            this.Controls.Add(this.txtWarehouseId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnIn);
            this.Controls.Add(this.btnXuatExcel);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dtgvMain);
            this.Name = "frmReportInventory";
            this.Text = "Báo cáo tồn kho";
            ((System.ComponentModel.ISupportInitialize)(this.dtgvMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dtgvMain;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnXuatExcel;
        private System.Windows.Forms.Button btnIn;
        private System.Windows.Forms.Label lblWarehouseName;
        private System.Windows.Forms.Button btnChooseWarehouse;
        private System.Windows.Forms.TextBox txtWarehouseId;
        private System.Windows.Forms.Label label3;
    }
}