using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public class ListEdit
    {
        public static void LoadFormSearchList(string[] columnsValue, string[] columnsText, Point point,out string KeySearch,Boolean isVoucher = false)
        {
            KeySearch = "";
            string KeySearch2 = "";
            Form frm = new Form();
            frm.Text = "Điều kiện lọc";
            frm.Size = (Size)point;
            DLLSystem.Init(frm);
            int k = 0;
            if (isVoucher)
            {
                k = 1;
                Label lblDFrom = new Label();
                lblDFrom.Location = new Point(10, 10);
                lblDFrom.Text = "Từ ngày/đến ngày";
                lblDFrom.AutoSize = true;
                lblDFrom.Font = new Font("Tahoma", 9);
                frm.Controls.Add(lblDFrom);

                DateTimePicker dtpDFrom = new DateTimePicker();
                dtpDFrom.Location = new Point(160, 10);
                dtpDFrom.Size = new Size(100, 20);
                dtpDFrom.Name = "dtpDFrom";
                dtpDFrom.Format = DateTimePickerFormat.Short;

                DateTimePicker dtpDTo = new DateTimePicker();
                dtpDTo.Location = new Point(270, 10);
                dtpDTo.Size = new Size(100, 20);
                dtpDTo.Name = "dtpDTo";
                dtpDTo.Format = DateTimePickerFormat.Short;

                frm.Controls.Add(dtpDFrom);
                frm.Controls.Add(dtpDTo);
            }
            for (int i = 0; i < columnsValue.Length; i++)
            {
                Label lbl = new Label();
                lbl.Location = new Point(10, 10 + ((k + i) * 30));
                lbl.Name = columnsValue[i];
                lbl.Text = columnsText[i];
                lbl.Font = new Font("Tahoma", 9);
                lbl.AutoSize = true;
                frm.Controls.Add(lbl);

                TextBox txtBox = new TextBox();
                if(isVoucher)
                {
                    txtBox.Location = new Point(160, 10 + ((k + i) * 30));
                    txtBox.Size = new Size(210, 20);
                }
                else
                {
                    txtBox.Location = new Point(120, 10 + ((k + i) * 30));
                    txtBox.Size = new Size(150, 20);
                }
                txtBox.Name = columnsValue[i];
                frm.Controls.Add(txtBox);
            }
            Button btnSearch = new Button();
            CssButton.StyleButton(btnSearch, "find.bmp");
            btnSearch.Location = new Point(frm.ClientSize.Width - btnSearch.Width - 14, frm.ClientSize.Height - btnSearch.Height - 8);
            btnSearch.Text = "Lọc dữ liệu";
            btnSearch.Click += (sender, e) => BtnSearch_Click(frm,out KeySearch2);
            frm.Controls.Add(btnSearch);
            frm.ShowDialog();
            KeySearch = KeySearch2;
        }
        private static void BtnSearch_Click(Form frm,out string KeySearch)
        {
            KeySearch = " WHERE 1=1 ";
            foreach (Control control in frm.Controls)
            {
                if (control is TextBox)
                {
                    KeySearch += " AND ";
                    KeySearch += "ISNULL("+ ((TextBox)control).Name + ",'')";
                    KeySearch += " LIKE N'%" + ((TextBox)control).Text + "%'";
                }
                else if(control is DateTimePicker)
                {
                    string a = control.Name;
                    if (control.Name == "dtpDFrom")
                    {
                        KeySearch += " AND VoucherDate BETWEEN '" + ((DateTimePicker)control).Value.ToString("yyyyMMdd") + "' ";
                    }
                    if(control.Name == "dtpDTo")
                    {
                        KeySearch += " AND N'" + ((DateTimePicker)control).Value.ToString("yyyyMMdd") + "' ";
                    }    
                }    
            }
            frm.Close();
        }
        public static void DeletePostData(string VoucherId)
        {
            string strPost = " DELETE PostData WHERE VoucherId = '" + VoucherId + "'";
            Models.SQL.RunQuery(strPost);
        }
        public static void SavePostData(string tableDetail,string tableMaster, string VoucherId,string VoucherCode,DateTime VoucherDate, string Absolute)
        {
            DeletePostData(VoucherId);
            string strPost = " INSERT INTO PostData(VoucherId,VoucherCode,VoucherDate,ProductsId,Amount,Price,TotalMoney)";
            strPost += " SELECT '" + VoucherId + "','"+VoucherCode+"','" + VoucherDate.ToString("yyyyMMdd") + "',ProductsId,"+ Absolute + "Amount,"+ Absolute + "Price,"+ Absolute + "TotalMoney FROM "+ tableDetail + " WHERE "+tableMaster+"Id = '" + VoucherId + "'";
            Models.SQL.RunQuery(strPost);
        }
        public static void SaveOrUpdateDetail(DataGridView dtgvMain, string table, string Key, string IdVoucherDetail,Boolean isPrice = false)
        {
            ListEdit.Delete(table, Key, IdVoucherDetail);
            string Price = "";
            string TotalMoney = "";
            string[] key = null;
            string[] value = null;
            for (int i = 0; i < dtgvMain.Rows.Count; i++)
            {
                string Id = Guid.NewGuid().ToString();
                string MasterId = IdVoucherDetail;
                string ProductsId = dtgvMain.Rows[i].Cells["ProductsId"].Value.ToString();
                string Amount = dtgvMain.Rows[i].Cells["Amount"].Value.ToString();
                string Note = dtgvMain.Rows[i].Cells["Note"].Value.ToString();
                if (!isPrice)
                {
                    Price = dtgvMain.Rows[i].Cells["Price"].Value.ToString().Replace(",","");
                    TotalMoney = dtgvMain.Rows[i].Cells["TotalMoney"].Value.ToString().Replace(",", "");
                    key = new[] { "Id", Key, "ProductsId", "Amount", "Price", "TotalMoney", "Note" };
                    value = new[] { Id, MasterId, ProductsId, Amount, Price, TotalMoney, Note };
                }    
                else
                {
                    key = new[] { "Id", Key, "ProductsId", "Amount", "Note" };
                    value = new[] { Id, MasterId, ProductsId, Amount, Note };
                }    
                Models.SQL.InsertTable(table, key, value);
            }   
        }
        public static void SaveOrUpdate(Form frm, Boolean isSave, out Boolean isLoad, Boolean isLog = false)
        {
            isLoad = false;
            string strQuery = "";
            string strCheck = "";
            List<string> lstKey = new List<string>();
            List<string> lstValue = new List<string>();
            foreach (Control control in frm.Controls)
            {
                if (control is TextBox && control.Tag != null && control.Tag.ToString().Contains("NB") && string.IsNullOrEmpty(control.Text))
                {
                    Alert.Error("Trường dữ liệu bắt buộc nhập");
                    control.Focus();
                    return;
                }
                else if (control is TextBox && !control.Tag.ToString().Contains("NS"))
                {
                    lstKey.Add(control.Name.Substring(3));
                    if (!control.Tag.ToString().Contains("NM"))
                    {
                        lstValue.Add("N'" + control.Text + "'");
                    }
                    else if (control.Tag.ToString().Contains("NM"))
                    {
                        if(string.IsNullOrEmpty(control.Text))
                        {
                            control.Text = "0";
                        }    
                        lstValue.Add("N'" + control.Text.Replace(",", "") + "'");
                    }
                }
                else if(control is CheckBox checkBox && !control.Tag.ToString().Contains("NS"))
                {
                    lstKey.Add(control.Name.Substring(3));
                    bool isChecked = checkBox.Checked;
                    if (isChecked)
                    {
                        lstValue.Add("'1'");
                    }
                    else
                    {
                        lstValue.Add("'0'");
                    }
                }
                else if (control is DateTimePicker dt)
                {
                    lstKey.Add(control.Name.Substring(3));
                    if (dt.Text ==" ")
                    {
                        lstValue.Add("NULL");
                    }
                    else
                    {
                        lstValue.Add("'" + DateTime.Parse(dt.Text).ToString("yyyyMMdd") + "'");
                    }
                }
            }
            if (isSave)
            {
                strCheck = "SELECT * FROM " + frm.Name.Substring(7) + " WHERE " + lstKey[lstKey.Count - 1] + " = " + lstValue[lstValue.Count - 1] + "";
                if (Models.SQL.FindExists(strCheck))
                {
                    Alert.Error("Dữ liệu đã tồn tại, hãy thêm bằng mã mới");
                    return;
                }
                string strLogKey = "";
                string strLogValues = "";
                if(isLog)
                {
                    strLogKey = ",DateCreate,UserCreate,DateUpdate,UserUpdate";
                    strLogValues = ",GETDATE(),N'"+frmLogin.UserName+"',GETDATE(),N'"+frmLogin.UserName + "'";
                }    
                strQuery = "INSERT INTO " + frm.Name.Substring(7) + "(" + string.Join(",", lstKey) + " "+ strLogKey + ") VALUES(" + string.Join(",", lstValue) + " "+ strLogValues + ")";
                Models.SQL.RunQuery(strQuery);
            }
            else
            {
                strQuery = "UPDATE " + frm.Name.Substring(7) + " SET ";
                for (int i = 0; i < lstKey.Count; i++)
                {
                    strQuery += lstKey[i].ToString() + " = " + lstValue[i];
                    if (i != lstKey.Count - 1)
                    {
                        strQuery += ",";
                    }
                    else
                    {
                        if(isLog)
                        {
                            strQuery += " ,DateUpdate = GETDATE(), UserUpdate = N'"+frmLogin.UserName+"' ";
                        }    
                        strQuery += " WHERE " + lstKey[i].ToString() + " = " + lstValue[i] + "";
                    }
                }
                Models.SQL.RunQuery(strQuery);
            }
            isLoad = true;
            frm.Close();
        }
        public static void Delete(string table, string Key, string Value)
        {
            string strQuery = "DELETE " + table + " WHERE " + Key + " = '" + Value + "'";
            Models.SQL.RunQuery(strQuery);
        }
    }
}
