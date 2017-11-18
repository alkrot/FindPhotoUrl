using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FindFotoUrl
{
    public partial class AuthForm
    {
        private WebBrowser _webBrowser1;

        private readonly IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            bool flag = disposing && components != null;
            if (flag)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AuthForm));
            _webBrowser1 = new WebBrowser();
            SuspendLayout();
            _webBrowser1.Dock = DockStyle.Fill;
            _webBrowser1.Location = new Point(0, 0);
            _webBrowser1.MinimumSize = new Size(20, 20);
            _webBrowser1.Name = "_webBrowser1";
            _webBrowser1.Size = new Size(600, 316);
            _webBrowser1.TabIndex = 0;
            _webBrowser1.DocumentCompleted += webBrowser1_DocumentCompleted;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 316);
            Controls.Add(_webBrowser1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Autch";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Load += Autch_Load;
            ResumeLayout(false);
        }
    }
}
