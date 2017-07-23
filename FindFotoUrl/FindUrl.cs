using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FindFotoUrl
{
	public class FindUrl : Form
	{
		private IContainer components = null;

		private TextBox textBox1;

		private Button button1;

		private TextBox textBox2;

		public FindUrl()
		{
			this.InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string text = this.textBox1.Text;
			string pattern = "https?://";
			text = Regex.Replace(text, pattern, "");
			string text2 = "";
			this.button1.Enabled = false;
			for (int i = 0; i < Index.Photos.Count; i++)
			{
				bool flag = Index.Photos[i].Url == text;
				if (flag)
				{
					text2 = string.Concat(new object[]
					{
						"vk.com/photo",
						Index.Photos[i].OwnerId,
						"_",
						Index.Photos[i].PhotoId
					});
					break;
				}
			}
			this.textBox2.Text = text2;
			this.button1.Enabled = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FindUrl));
			this.textBox1 = new TextBox();
			this.button1 = new Button();
			this.textBox2 = new TextBox();
			base.SuspendLayout();
			this.textBox1.Location = new Point(12, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(186, 20);
			this.textBox1.TabIndex = 0;
			this.button1.Location = new Point(204, 10);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 23);
			this.button1.TabIndex = 1;
			this.button1.Text = "Поиск";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new EventHandler(this.button1_Click);
			this.textBox2.Location = new Point(12, 38);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new Size(267, 20);
			this.textBox2.TabIndex = 2;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(310, 75);
			base.Controls.Add(this.textBox2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.textBox1);
			base.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "FindUrl";
			this.Text = "Поиск нужной ссылки";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
