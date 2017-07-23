using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FindFotoUrl
{
	public class Autch : Form
	{
		private readonly IContainer components = null;

		private WebBrowser _webBrowser1;

		public Autch()
		{
			InitializeComponent();
		}

		private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			string text = _webBrowser1.Url.ToString();
			bool flag = text.IndexOf('#') >= 0;
			if (flag)
			{
				string[] array = text.Split('#')[1].Split('&');
				User.Default.Id = int.Parse(array[2].Split('=')[1]);
				User.Default.Token = array[0].Split('=')[1];
				Close();
			}
		}

		private void Autch_Load(object sender, EventArgs e)
		{
			_webBrowser1.Navigate("https://oauth.vk.com/authorize?client_id=3809552&scope=photos,groups&redirect_uri=https://oauth.vk.com/blank.html&display=mobile&v=5.34&response_type=token");
		}

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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Autch));
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
			Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			Name = "Autch";
			ShowIcon = false;
			StartPosition = FormStartPosition.CenterScreen;
			Load += Autch_Load;
			ResumeLayout(false);
		}
	}
}
