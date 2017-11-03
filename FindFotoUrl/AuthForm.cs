using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FindFotoUrl
{
	public partial class AuthForm : Form
	{
		public AuthForm()
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
	}
}
