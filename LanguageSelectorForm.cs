using System;
using System.Drawing;
using System.Windows.Forms;

namespace Setup_RadioPlayer
{
    public partial class LanguageSelectorForm : Form
    {
        public LanguageSelectorForm()
        {
            this.Text = "Select Language / Оберіть мову / Scegli la lingua";
            this.Size = new Size(400, 220);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(45, 45, 48);

            Label lbl = new Label();
            lbl.Text = "Please select your language:\nБудь ласка, оберіть мову:\nSeleziona la tua lingua:";
            lbl.ForeColor = Color.White;
            lbl.Font = new Font("Segoe UI", 11);
            lbl.AutoSize = false;
            lbl.Size = new Size(360, 60);
            lbl.Location = new Point(20, 20);

            ComboBox cmb = new ComboBox();
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.Font = new Font("Segoe UI", 11);
            cmb.Size = new Size(300, 30);
            cmb.Location = new Point(50, 90);
            cmb.Items.Add("Українська");
            cmb.Items.Add("English");
            cmb.Items.Add("Italiano");
            cmb.SelectedIndex = 0;

            Button btnOK = new Button();
            btnOK.Text = "OK";
            btnOK.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnOK.Size = new Size(100, 35);
            btnOK.Location = new Point(150, 140);
            btnOK.Click += (s, e) =>
            {
                LanguageManager.CurrentLanguage = (LanguageManager.Language)cmb.SelectedIndex;
                this.DialogResult = DialogResult.OK;
                this.Close();
            };

            this.Controls.Add(lbl);
            this.Controls.Add(cmb);
            this.Controls.Add(btnOK);
        }
    }
}
