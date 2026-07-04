using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ExifFileRenamer
{
    /// <summary>
    /// About dialog: product name, version, author and a clickable e-mail link.
    /// </summary>
    internal class AboutForm : Form
    {
        private const string AUTHOR_NAME = "Alexey Konovalenko";
        private const string AUTHOR_EMAIL = "aldev@ukr.net";

        public AboutForm()
        {
            Text = "About " + Application.ProductName;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            ClientSize = new Size(340, 192);

            var laProduct = new Label
            {
                Text = Application.ProductName,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 18)
            };

            var laVersion = new Label
            {
                Text = "Version " + Application.ProductVersion,
                AutoSize = true,
                Location = new Point(22, 48)
            };

            var laAuthor = new Label
            {
                Text = "Author: " + AUTHOR_NAME,
                AutoSize = true,
                Location = new Point(22, 76)
            };

            var laEmailCaption = new Label
            {
                Text = "E-mail:",
                AutoSize = true,
                Location = new Point(22, 98)
            };

            var lnkEmail = new LinkLabel
            {
                Text = AUTHOR_EMAIL,
                AutoSize = true,
                Location = new Point(64, 98)
            };
            lnkEmail.LinkClicked += LnkEmail_LinkClicked;

            var laCopyright = new Label
            {
                Text = GetCopyright(),
                ForeColor = SystemColors.GrayText,
                AutoSize = true,
                Location = new Point(22, 126)
            };

            var buOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new Size(88, 28),
                Location = new Point(ClientSize.Width - 88 - 16, ClientSize.Height - 28 - 12),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            Controls.Add(laProduct);
            Controls.Add(laVersion);
            Controls.Add(laAuthor);
            Controls.Add(laEmailCaption);
            Controls.Add(lnkEmail);
            Controls.Add(laCopyright);
            Controls.Add(buOk);
            AcceptButton = buOk;
            CancelButton = buOk;
        }

        private static string GetCopyright()
        {
            var attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
            return attribute != null ? attribute.Copyright : string.Empty;
        }

        private void LnkEmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ((LinkLabel)sender).LinkVisited = true;
                Process.Start("mailto:" + AUTHOR_EMAIL);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Could not open the mail client:\n" + ex.Message,
                    "About", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
