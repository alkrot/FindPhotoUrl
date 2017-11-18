using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FindFotoUrl
{
	public partial class FPhotoForm : Form
	{
		public FPhotoForm()
		{
            InitializeComponent();
		}

		private void FPhoto_FormClosed(object sender, FormClosedEventArgs e)
		{
			MainForm.Openform = false;
		}

		private void url_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("https://" + url.Text);
		}
	}
}
