using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PMQuanLyHangTonKho.Lib
{
    public class CssButton
    {
        public static void StyleButtonSaveExit(Button buttonSave, Button buttonExit)
        {
            buttonSave.BackColor = System.Drawing.SystemColors.Control;
            buttonSave.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonSave.Image = Image.FromFile(DLLSystem.GetPathImage() + "mark.bmp");
            buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonSave.Size = new System.Drawing.Size(99, 35);
            buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonSave.UseVisualStyleBackColor = false;
            ////////////////////****////////////////////////////////////
            buttonExit.BackColor = System.Drawing.SystemColors.Control;
            buttonExit.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            buttonExit.Image = Image.FromFile(DLLSystem.GetPathImage() + "exit.bmp");
            buttonExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            buttonExit.Size = new System.Drawing.Size(72, 35);
            buttonExit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            buttonExit.UseVisualStyleBackColor = false;
            buttonExit.DialogResult = DialogResult.Cancel;
        }
        public static void StyleButton(Button button, string nameFile)
        {
            button.BackColor = System.Drawing.SystemColors.Control;
            button.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            button.Image = Image.FromFile(DLLSystem.GetPathImage() + nameFile);
            button.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            button.Size = new System.Drawing.Size(99, 30);
            button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            button.UseVisualStyleBackColor = false;
        }
    }
}
