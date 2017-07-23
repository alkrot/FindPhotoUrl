using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FindFotoUrl
{
	public class Progress : Form
	{
		private IContainer components = null;

		public Label label1;

		public Progress()
		{
			this.InitializeComponent();
		}

		private void Progress_Load(object sender, EventArgs e)
		{
			int x = Screen.PrimaryScreen.Bounds.Width - 310;
			int y = Screen.PrimaryScreen.Bounds.Height - 110;
			base.Location = new Point(x, y);
		}

		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.label1 = new Label();
			base.SuspendLayout();
			this.label1.Dock = DockStyle.Fill;
			this.label1.Location = new Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(279, 28);
			this.label1.TabIndex = 0;
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(279, 28);
			base.ControlBox = false;
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
			base.Name = "Progress";
			this.Text = "Идет сканирование";
			base.TopMost = true;
			base.Load += new EventHandler(this.Progress_Load);
			base.ResumeLayout(false);
		}
	}
}
