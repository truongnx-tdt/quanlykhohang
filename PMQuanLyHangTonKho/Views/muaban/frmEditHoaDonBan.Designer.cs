namespace PMQuanLyHangTonKho.Views.muaban
{
    partial class frmEditHoaDonBan
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
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTotalQuantity = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtgvDetail = new System.Windows.Forms.DataGridView();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblWarehouseName = new System.Windows.Forms.Label();
            this.btnChooseWarehouse = new System.Windows.Forms.Button();
            this.txtWarehouseId = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpVoucherDate = new System.Windows.Forms.DateTimePicker();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.btnChooseCustomer = new System.Windows.Forms.Button();
            this.txtCustomerId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPriceListName = new System.Windows.Forms.Label();
            this.btnChoosePriceList = new System.Windows.Forms.Button();
            this.txtPriceListId = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkRetail = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.RichTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Enabled = false;
            this.txtTotalAmount.Location = new System.Drawing.Point(913, 715);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.Size = new System.Drawing.Size(150, 20);
            this.txtTotalAmount.TabIndex = 508;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(858, 722);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 507;
            this.label7.Text = "Tổng tiền";
            // 
            // txtTotalQuantity
            // 
            this.txtTotalQuantity.Enabled = false;
            this.txtTotalQuantity.Location = new System.Drawing.Point(681, 715);
            this.txtTotalQuantity.Name = "txtTotalQuantity";
            this.txtTotalQuantity.Size = new System.Drawing.Size(150, 20);
            this.txtTotalQuantity.TabIndex = 506;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(626, 722);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 505;
            this.label5.Text = "Tổng SL";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtgvDetail);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(10, 147);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1056, 530);
            this.groupBox1.TabIndex = 504;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "F3 - Thêm dòng, F8 - Xóa dòng";
            // 
            // dtgvDetail
            // 
            this.dtgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvDetail.Location = new System.Drawing.Point(3, 18);
            this.dtgvDetail.Name = "dtgvDetail";
            this.dtgvDetail.Size = new System.Drawing.Size(1050, 509);
            this.dtgvDetail.TabIndex = 437;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(990, 756);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 35);
            this.btnExit.TabIndex = 503;
            this.btnExit.Text = "Thoát";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(889, 756);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(99, 35);
            this.btnSave.TabIndex = 502;
            this.btnSave.Text = "Lưu dữ liệu";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(101, 12);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(150, 20);
            this.txtId.TabIndex = 510;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 509;
            this.label1.Text = "Mã";
            // 
            // lblWarehouseName
            // 
            this.lblWarehouseName.AutoSize = true;
            this.lblWarehouseName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouseName.Location = new System.Drawing.Point(880, 14);
            this.lblWarehouseName.Name = "lblWarehouseName";
            this.lblWarehouseName.Size = new System.Drawing.Size(53, 14);
            this.lblWarehouseName.TabIndex = 514;
            this.lblWarehouseName.Text = "Tên kho";
            // 
            // btnChooseWarehouse
            // 
            this.btnChooseWarehouse.Location = new System.Drawing.Point(855, 11);
            this.btnChooseWarehouse.Name = "btnChooseWarehouse";
            this.btnChooseWarehouse.Size = new System.Drawing.Size(24, 22);
            this.btnChooseWarehouse.TabIndex = 513;
            this.btnChooseWarehouse.Text = "...";
            this.btnChooseWarehouse.UseVisualStyleBackColor = true;
            // 
            // txtWarehouseId
            // 
            this.txtWarehouseId.Location = new System.Drawing.Point(703, 12);
            this.txtWarehouseId.Name = "txtWarehouseId";
            this.txtWarehouseId.ReadOnly = true;
            this.txtWarehouseId.Size = new System.Drawing.Size(150, 20);
            this.txtWarehouseId.TabIndex = 512;
            this.txtWarehouseId.Tag = "txtNB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(588, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(28, 14);
            this.label6.TabIndex = 511;
            this.label6.Text = "Kho";
            // 
            // dtpVoucherDate
            // 
            this.dtpVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVoucherDate.Location = new System.Drawing.Point(101, 39);
            this.dtpVoucherDate.Name = "dtpVoucherDate";
            this.dtpVoucherDate.Size = new System.Drawing.Size(150, 20);
            this.dtpVoucherDate.TabIndex = 519;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(880, 44);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(48, 14);
            this.lblCustomerName.TabIndex = 523;
            this.lblCustomerName.Text = "Tên KH";
            // 
            // btnChooseCustomer
            // 
            this.btnChooseCustomer.Location = new System.Drawing.Point(855, 41);
            this.btnChooseCustomer.Name = "btnChooseCustomer";
            this.btnChooseCustomer.Size = new System.Drawing.Size(24, 22);
            this.btnChooseCustomer.TabIndex = 522;
            this.btnChooseCustomer.Text = "...";
            this.btnChooseCustomer.UseVisualStyleBackColor = true;
            // 
            // txtCustomerId
            // 
            this.txtCustomerId.Location = new System.Drawing.Point(703, 42);
            this.txtCustomerId.Name = "txtCustomerId";
            this.txtCustomerId.ReadOnly = true;
            this.txtCustomerId.Size = new System.Drawing.Size(150, 20);
            this.txtCustomerId.TabIndex = 521;
            this.txtCustomerId.Tag = "txtNB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(588, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 14);
            this.label4.TabIndex = 520;
            this.label4.Text = "Khách hàng";
            // 
            // lblPriceListName
            // 
            this.lblPriceListName.AutoSize = true;
            this.lblPriceListName.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPriceListName.Location = new System.Drawing.Point(880, 70);
            this.lblPriceListName.Name = "lblPriceListName";
            this.lblPriceListName.Size = new System.Drawing.Size(48, 14);
            this.lblPriceListName.TabIndex = 527;
            this.lblPriceListName.Text = "Tên BG";
            // 
            // btnChoosePriceList
            // 
            this.btnChoosePriceList.Location = new System.Drawing.Point(855, 67);
            this.btnChoosePriceList.Name = "btnChoosePriceList";
            this.btnChoosePriceList.Size = new System.Drawing.Size(24, 22);
            this.btnChoosePriceList.TabIndex = 526;
            this.btnChoosePriceList.Text = "...";
            this.btnChoosePriceList.UseVisualStyleBackColor = true;
            // 
            // txtPriceListId
            // 
            this.txtPriceListId.Location = new System.Drawing.Point(703, 68);
            this.txtPriceListId.Name = "txtPriceListId";
            this.txtPriceListId.ReadOnly = true;
            this.txtPriceListId.Size = new System.Drawing.Size(150, 20);
            this.txtPriceListId.TabIndex = 525;
            this.txtPriceListId.Tag = "txtNB";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(588, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 14);
            this.label8.TabIndex = 524;
            this.label8.Text = "Bảng giá";
            // 
            // chkRetail
            // 
            this.chkRetail.AutoSize = true;
            this.chkRetail.Location = new System.Drawing.Point(101, 70);
            this.chkRetail.Name = "chkRetail";
            this.chkRetail.Size = new System.Drawing.Size(56, 17);
            this.chkRetail.TabIndex = 528;
            this.chkRetail.Text = "Bán lẻ";
            this.chkRetail.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 529;
            this.label2.Text = "Ngày chứng từ";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(329, 12);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(253, 77);
            this.txtNote.TabIndex = 531;
            this.txtNote.Text = "";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(274, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 532;
            this.label9.Text = "Ghi chú";
            // 
            // frmEditHoaDonBan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 805);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkRetail);
            this.Controls.Add(this.lblPriceListName);
            this.Controls.Add(this.btnChoosePriceList);
            this.Controls.Add(this.txtPriceListId);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.btnChooseCustomer);
            this.Controls.Add(this.txtCustomerId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpVoucherDate);
            this.Controls.Add(this.lblWarehouseName);
            this.Controls.Add(this.btnChooseWarehouse);
            this.Controls.Add(this.txtWarehouseId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtTotalQuantity);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Name = "frmEditHoaDonBan";
            this.Text = "frmEditHoaDonBan";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtTotalQuantity;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtgvDetail;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblWarehouseName;
        private System.Windows.Forms.Button btnChooseWarehouse;
        private System.Windows.Forms.TextBox txtWarehouseId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpVoucherDate;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.Button btnChooseCustomer;
        private System.Windows.Forms.TextBox txtCustomerId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPriceListName;
        private System.Windows.Forms.Button btnChoosePriceList;
        private System.Windows.Forms.TextBox txtPriceListId;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkRetail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtNote;
        private System.Windows.Forms.Label label9;
    }
}