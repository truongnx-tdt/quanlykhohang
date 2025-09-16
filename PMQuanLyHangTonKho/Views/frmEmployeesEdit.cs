using PMQuanLyHangTonKho.Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Views
{
    public partial class frmEditEmployees : Form
    {
        public frmEditEmployees()
        {
            InitializeComponent();
            dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
            dtpDateOfBirth.CustomFormat = " ";
            dtpHireDate.Format = DateTimePickerFormat.Custom;
            dtpHireDate.CustomFormat = " ";
            DLLSystem.InitNotResize(this);
            CssButton.StyleButtonSaveExit(btnSave, btnExit);
            this.btnSave.Click += btnSave_Click;
        }
        public void SetValue(string Id,Boolean isCopy = true)
        {
            DataTable dt = new DataTable();
            dt = Models.SQL.GetData("SELECT * FROM Employees WHERE Id = '"+ Id + "'");
            if(dt != null)
            {
                if(string.IsNullOrEmpty(dt.Rows[0]["DateOfBirth"].ToString()))
                {
                    dtpDateOfBirth.CustomFormat = " ";
                }   
                else
                {
                    dtpDateOfBirth.Text = dt.Rows[0]["DateOfBirth"].ToString();
                }
                if(string.IsNullOrEmpty(dt.Rows[0]["HireDate"].ToString()))
                {
                    dtpHireDate.CustomFormat = " ";
                }
                else
                {
                    dtpHireDate.Text = dt.Rows[0]["HireDate"].ToString();
                }

                txtId.Text = dt.Rows[0]["Id"].ToString();
                txtName.Text = dt.Rows[0]["Name"].ToString();
                string gender  = dt.Rows[0]["Gender"].ToString();
                txtPhone.Text = dt.Rows[0]["Phone"].ToString();
                txtEmail.Text = dt.Rows[0]["Email"].ToString();
                txtPermanentAddress.Text = dt.Rows[0]["PermanentAddress"].ToString();
                txtTemporaryAddress.Text = dt.Rows[0]["TemporaryAddress"].ToString();
                txtPosition.Text = dt.Rows[0]["Position"].ToString();
                if (gender != "False")
                {
                    chkNam.Checked = true;
                    chkNu.Checked = false;
                }
                else
                {
                    chkNam.Checked = false;
                    chkNu.Checked = true;
                }
            }    
            if(isCopy)
            {
                txtId.ReadOnly = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!chkNam.Checked && !chkNu.Checked)
            {
                Alert.Error("Giới tính bắt buộc nhập");
                chkNu.Focus();
                return;
            }    
            if(DateTime.Parse(dtpDateOfBirth.Value.ToString("dd/MM/yyyy")) >= DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")))
            {
                Alert.Error("Ngày sinh phải nhỏ hơn ngày hiện tại");
                dtpDateOfBirth.Focus();
                return;
            }    
            if(chkNam.Checked)
            {
                chkGender.Checked = true;
            }    
            else
            {
                chkGender.Checked = false;
            }
            ListEdit.SaveOrUpdate(this, frmEmployees.isSave, out frmEmployees.isLoad,true);
        }
        private void dtDateOfBirth_ValueChanged(object sender, EventArgs e)
        {
            this.dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";
        }

        private void dtDateOfBirth_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dtpDateOfBirth.Text = "";
                dtpDateOfBirth.CustomFormat = " ";
            }
        }

        private void dtHireDate_ValueChanged(object sender, EventArgs e)
        {
            this.dtpHireDate.CustomFormat = "dd/MM/yyyy";
        }

        private void dtHireDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                dtpHireDate.Text = "";
                dtpHireDate.CustomFormat = " ";
            }
        }

        private void chkNam_CheckedChanged(object sender, EventArgs e)
        {
            if(chkNam.Checked) 
            {
                chkNu.Checked = false;
            }
            else
            {
                chkNu.Checked = true;
            }    
        }

        private void chkNu_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNu.Checked)
            {
                chkNam.Checked = false;
            }
            else
            {
                chkNam.Checked = true;
            }
        }
    }
}
