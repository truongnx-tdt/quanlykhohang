namespace PMQuanLyHangTonKho.Views
{
    partial class frmEditExportStockMaster
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtgvDetail = new System.Windows.Forms.DataGridView();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTotalMoney = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.btnChooseCustomer = new System.Windows.Forms.Button();
            this.txtCustomerId = new System.Windows.Forms.TextBox();
            this.dtpVoucherDate = new System.Windows.Forms.DateTimePicker();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLayTuHoaDonBan = new System.Windows.Forms.Button();
            this.lblWarehouseName = new System.Windows.Forms.Label();
            this.btnChooseWarehouse = new System.Windows.Forms.Button();
            this.txtWarehouseId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtgvDetail);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(3, 103);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 354);
            this.groupBox1.TabIndex = 463;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "F3 - Thêm dòng, F8 - Xóa dòng";
            // 
            // dtgvDetail
            // 
            this.dtgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvDetail.Location = new System.Drawing.Point(3, 18);
            this.dtgvDetail.Name = "dtgvDetail";
            this.dtgvDetail.Size = new System.Drawing.Size(869, 333);
            this.dtgvDetail.TabIndex = 437;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(723, 463);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(150, 20);
            this.txtTotalAmount.TabIndex = 462;
            this.txtTotalAmount.Tag = "txtNM";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(609, 466);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 14);
            this.label4.TabIndex = 461;
            this.label4.Text = "Tổng số lượng";
            // 
            // txtTotalMoney
            // 
            this.txtTotalMoney.Location = new System.Drawing.Point(434, 463);
            this.txtTotalMoney.Name = "txtTotalMoney";
            this.txtTotalMoney.ReadOnly = true;
            this.txtTotalMoney.Size = new System.Drawing.Size(150, 20);
            this.txtTotalMoney.TabIndex = 460;
            this.txtTotalMoney.Tag = "txtNM";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(344, 467);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 14);
            this.label3.TabIndex = 459;
            this.label3.Text = "Tổng tiền";
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(683, 8);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(46, 14);
            this.lblCustomerName.TabIndex = 458;
            this.lblCustomerName.Text = "Tên kh";
            // 
            // btnChooseCustomer
            // 
            this.btnChooseCustomer.Location = new System.Drawing.Point(658, 5);
            this.btnChooseCustomer.Name = "btnChooseCustomer";
            this.btnChooseCustomer.Size = new System.Drawing.Size(24, 22);
            this.btnChooseCustomer.TabIndex = 457;
            this.btnChooseCustomer.Text = "...";
            this.btnChooseCustomer.UseVisualStyleBackColor = true;
            // 
            // txtCustomerId
            // 
            this.txtCustomerId.Location = new System.Drawing.Point(506, 6);
            this.txtCustomerId.Name = "txtCustomerId";
            this.txtCustomerId.ReadOnly = true;
            this.txtCustomerId.Size = new System.Drawing.Size(150, 20);
            this.txtCustomerId.TabIndex = 456;
            this.txtCustomerId.Tag = "txtNB";
            // 
            // dtpVoucherDate
            // 
            this.dtpVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVoucherDate.Location = new System.Drawing.Point(183, 35);
            this.dtpVoucherDate.Name = "dtpVoucherDate";
            this.dtpVoucherDate.Size = new System.Drawing.Size(150, 20);
            this.dtpVoucherDate.TabIndex = 455;
            this.dtpVoucherDate.Tag = "dtp";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(183, 61);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(685, 20);
            this.txtNote.TabIndex = 454;
            this.txtNote.Tag = "txt";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(16, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 14);
            this.label8.TabIndex = 453;
            this.label8.Text = "Ghi chú";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(391, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 14);
            this.label6.TabIndex = 452;
            this.label6.Text = "Khách hàng";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(801, 524);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 35);
            this.btnExit.TabIndex = 451;
            this.btnExit.Text = "Thoát";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(700, 524);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 35);
            this.btnSave.TabIndex = 450;
            this.btnSave.Text = "Lưu dữ liệu";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 14);
            this.label2.TabIndex = 449;
            this.label2.Text = "Ngày xuất";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(183, 6);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(150, 20);
            this.txtId.TabIndex = 448;
            this.txtId.Tag = "txtNB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 14);
            this.label1.TabIndex = 447;
            this.label1.Text = "Mã phiếu xuất";
            // 
            // btnLayTuHoaDonBan
            // 
            this.btnLayTuHoaDonBan.BackColor = System.Drawing.SystemColors.Control;
            this.btnLayTuHoaDonBan.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLayTuHoaDonBan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLayTuHoaDonBan.Location = new System.Drawing.Point(557, 524);
            this.btnLayTuHoaDonBan.Name = "btnLayTuHoaDonBan";
            this.btnLayTuHoaDonBan.Size = new System.Drawing.Size(125, 35);
            this.btnLayTuHoaDonBan.TabIndex = 464;
            this.btnLayTuHoaDonBan.Text = "Lấy dữ liệu từ HDB";
            this.btnLayTuHoaDonBan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLayTuHoaDonBan.UseVisualStyleBackColor = false;
            // 
            // lblWarehouseName
            // 
            this.lblWarehouseName.AutoSize = true;
            this.lblWarehouseName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouseName.Location = new System.Drawing.Point(683, 34);
            this.lblWarehouseName.Name = "lblWarehouseName";
            this.lblWarehouseName.Size = new System.Drawing.Size(53, 14);
            this.lblWarehouseName.TabIndex = 468;
            this.lblWarehouseName.Text = "Tên kho";
            // 
            // btnChooseWarehouse
            // 
            this.btnChooseWarehouse.Location = new System.Drawing.Point(658, 31);
            this.btnChooseWarehouse.Name = "btnChooseWarehouse";
            this.btnChooseWarehouse.Size = new System.Drawing.Size(24, 22);
            this.btnChooseWarehouse.TabIndex = 467;
            this.btnChooseWarehouse.Text = "...";
            this.btnChooseWarehouse.UseVisualStyleBackColor = true;
            // 
            // txtWarehouseId
            // 
            this.txtWarehouseId.Location = new System.Drawing.Point(506, 32);
            this.txtWarehouseId.Name = "txtWarehouseId";
            this.txtWarehouseId.ReadOnly = true;
            this.txtWarehouseId.Size = new System.Drawing.Size(150, 20);
            this.txtWarehouseId.TabIndex = 466;
            this.txtWarehouseId.Tag = "txtNB";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(391, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 14);
            this.label5.TabIndex = 465;
            this.label5.Text = "Kho xuất";
            // 
            // frmEditExportStockMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 565);
            this.Controls.Add(this.lblWarehouseName);
            this.Controls.Add(this.btnChooseWarehouse);
            this.Controls.Add(this.txtWarehouseId);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnLayTuHoaDonBan);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTotalMoney);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.btnChooseCustomer);
            this.Controls.Add(this.txtCustomerId);
            this.Controls.Add(this.dtpVoucherDate);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.Name = "frmEditExportStockMaster";
            this.Text = "frmExportStockMasterEdit";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtgvDetail;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTotalMoney;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Button btnChooseCustomer;
        private System.Windows.Forms.TextBox txtCustomerId;
        private System.Windows.Forms.DateTimePicker dtpVoucherDate;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLayTuHoaDonBan;
        private System.Windows.Forms.Label lblWarehouseName;
        private System.Windows.Forms.Button btnChooseWarehouse;
        private System.Windows.Forms.TextBox txtWarehouseId;
        private System.Windows.Forms.Label label5;
    }
}