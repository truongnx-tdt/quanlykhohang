namespace PMQuanLyHangTonKho.Views
{
    partial class frmEditTransferStockMaster
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
            this.lblWarehouseNameTo = new System.Windows.Forms.Label();
            this.btnChooseWarehouseIdTo = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtgvDetail = new System.Windows.Forms.DataGridView();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWarehouseNameFrom = new System.Windows.Forms.Label();
            this.btnChooseWarehouseIdFrom = new System.Windows.Forms.Button();
            this.txtWarehouseIdFrom = new System.Windows.Forms.TextBox();
            this.dtpVoucherDate = new System.Windows.Forms.DateTimePicker();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWarehouseIdTo = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // lblWarehouseNameTo
            // 
            this.lblWarehouseNameTo.AutoSize = true;
            this.lblWarehouseNameTo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouseNameTo.Location = new System.Drawing.Point(911, 43);
            this.lblWarehouseNameTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWarehouseNameTo.Name = "lblWarehouseNameTo";
            this.lblWarehouseNameTo.Size = new System.Drawing.Size(61, 18);
            this.lblWarehouseNameTo.TabIndex = 467;
            this.lblWarehouseNameTo.Text = "Tên ncc";
            // 
            // btnChooseWarehouseIdTo
            // 
            this.btnChooseWarehouseIdTo.Location = new System.Drawing.Point(877, 39);
            this.btnChooseWarehouseIdTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChooseWarehouseIdTo.Name = "btnChooseWarehouseIdTo";
            this.btnChooseWarehouseIdTo.Size = new System.Drawing.Size(32, 27);
            this.btnChooseWarehouseIdTo.TabIndex = 466;
            this.btnChooseWarehouseIdTo.Text = "...";
            this.btnChooseWarehouseIdTo.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(521, 46);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 18);
            this.label7.TabIndex = 464;
            this.label7.Text = "Chuyển sang kho";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.dtgvDetail);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 127);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(1167, 436);
            this.groupBox1.TabIndex = 463;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "F3 - Thêm dòng, F8 - Xóa dòng";
            // 
            // dtgvDetail
            // 
            this.dtgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtgvDetail.Location = new System.Drawing.Point(4, 23);
            this.dtgvDetail.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtgvDetail.Name = "dtgvDetail";
            this.dtgvDetail.RowHeadersWidth = 51;
            this.dtgvDetail.Size = new System.Drawing.Size(1159, 409);
            this.dtgvDetail.TabIndex = 437;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(964, 570);
            this.txtTotalAmount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(199, 22);
            this.txtTotalAmount.TabIndex = 462;
            this.txtTotalAmount.Tag = "txtNM";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(812, 574);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 18);
            this.label4.TabIndex = 461;
            this.label4.Text = "Tổng số lượng";
            // 
            // lblWarehouseNameFrom
            // 
            this.lblWarehouseNameFrom.AutoSize = true;
            this.lblWarehouseNameFrom.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarehouseNameFrom.Location = new System.Drawing.Point(911, 10);
            this.lblWarehouseNameFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWarehouseNameFrom.Name = "lblWarehouseNameFrom";
            this.lblWarehouseNameFrom.Size = new System.Drawing.Size(62, 18);
            this.lblWarehouseNameFrom.TabIndex = 458;
            this.lblWarehouseNameFrom.Text = "Tên kho";
            // 
            // btnChooseWarehouseIdFrom
            // 
            this.btnChooseWarehouseIdFrom.Location = new System.Drawing.Point(877, 6);
            this.btnChooseWarehouseIdFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnChooseWarehouseIdFrom.Name = "btnChooseWarehouseIdFrom";
            this.btnChooseWarehouseIdFrom.Size = new System.Drawing.Size(32, 27);
            this.btnChooseWarehouseIdFrom.TabIndex = 457;
            this.btnChooseWarehouseIdFrom.Text = "...";
            this.btnChooseWarehouseIdFrom.UseVisualStyleBackColor = true;
            // 
            // txtWarehouseIdFrom
            // 
            this.txtWarehouseIdFrom.Location = new System.Drawing.Point(675, 7);
            this.txtWarehouseIdFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtWarehouseIdFrom.Name = "txtWarehouseIdFrom";
            this.txtWarehouseIdFrom.ReadOnly = true;
            this.txtWarehouseIdFrom.Size = new System.Drawing.Size(199, 22);
            this.txtWarehouseIdFrom.TabIndex = 456;
            this.txtWarehouseIdFrom.Tag = "txtNB";
            // 
            // dtpVoucherDate
            // 
            this.dtpVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpVoucherDate.Location = new System.Drawing.Point(244, 43);
            this.dtpVoucherDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpVoucherDate.Name = "dtpVoucherDate";
            this.dtpVoucherDate.Size = new System.Drawing.Size(199, 22);
            this.dtpVoucherDate.TabIndex = 455;
            this.dtpVoucherDate.Tag = "dtp";
            // 
            // txtNote
            // 
            this.txtNote.Location = new System.Drawing.Point(244, 75);
            this.txtNote.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(912, 22);
            this.txtNote.TabIndex = 454;
            this.txtNote.Tag = "txt";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(21, 73);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 18);
            this.label8.TabIndex = 453;
            this.label8.Text = "Ghi chú";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(521, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 18);
            this.label6.TabIndex = 452;
            this.label6.Text = "Chuyển từ kho";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(1068, 645);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(96, 43);
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
            this.btnSave.Location = new System.Drawing.Point(933, 645);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(132, 43);
            this.btnSave.TabIndex = 450;
            this.btnSave.Text = "Lưu dữ liệu";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 43);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 18);
            this.label2.TabIndex = 449;
            this.label2.Text = "Ngày nhập";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(244, 7);
            this.txtId.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(199, 22);
            this.txtId.TabIndex = 448;
            this.txtId.Tag = "txtNB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 18);
            this.label1.TabIndex = 447;
            this.label1.Text = "Mã phiếu";
            // 
            // txtWarehouseIdTo
            // 
            this.txtWarehouseIdTo.Location = new System.Drawing.Point(675, 37);
            this.txtWarehouseIdTo.Margin = new System.Windows.Forms.Padding(4);
            this.txtWarehouseIdTo.Name = "txtWarehouseIdTo";
            this.txtWarehouseIdTo.ReadOnly = true;
            this.txtWarehouseIdTo.Size = new System.Drawing.Size(199, 22);
            this.txtWarehouseIdTo.TabIndex = 468;
            this.txtWarehouseIdTo.Tag = "txtNB";
            // 
            // frmEditTransferStockMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 695);
            this.Controls.Add(this.txtWarehouseIdTo);
            this.Controls.Add(this.lblWarehouseNameTo);
            this.Controls.Add(this.btnChooseWarehouseIdTo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblWarehouseNameFrom);
            this.Controls.Add(this.btnChooseWarehouseIdFrom);
            this.Controls.Add(this.txtWarehouseIdFrom);
            this.Controls.Add(this.dtpVoucherDate);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmEditTransferStockMaster";
            this.Text = "frmTransferStockMasterEdit";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWarehouseNameTo;
        private System.Windows.Forms.Button btnChooseWarehouseIdTo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dtgvDetail;
        private System.Windows.Forms.TextBox txtTotalAmount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtEmployeesId2;
        private System.Windows.Forms.Label lblWarehouseNameFrom;
        private System.Windows.Forms.Button btnChooseWarehouseIdFrom;
        private System.Windows.Forms.TextBox txtWarehouseIdFrom;
        private System.Windows.Forms.DateTimePicker dtpVoucherDate;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWarehouseIdTo;
    }
}