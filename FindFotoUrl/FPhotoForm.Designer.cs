using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FindFotoUrl
{
    partial class FPhotoForm
    {
        private IContainer components = null;

        public PictureBox pictureBox1;

        public Label ltext;

        public LinkLabel url;

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
            this.pictureBox1 = new PictureBox();
            this.ltext = new Label();
            this.url = new LinkLabel();
            ((ISupportInitialize)this.pictureBox1).BeginInit();
            base.SuspendLayout();
            this.pictureBox1.BackColor = SystemColors.ActiveCaptionText;
            this.pictureBox1.Dock = DockStyle.Fill;
            this.pictureBox1.Location = new Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Size(533, 356);
            this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.ltext.BackColor = Color.White;
            this.ltext.Cursor = Cursors.IBeam;
            this.ltext.Dock = DockStyle.Bottom;
            this.ltext.Location = new Point(0, 277);
            this.ltext.Name = "ltext";
            this.ltext.Size = new Size(533, 79);
            this.ltext.TabIndex = 1;
            this.url.Dock = DockStyle.Bottom;
            this.url.Location = new Point(0, 258);
            this.url.Name = "url";
            this.url.Size = new Size(533, 19);
            this.url.TabIndex = 2;
            this.url.LinkClicked += new LinkLabelLinkClickedEventHandler(this.url_LinkClicked);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            base.ClientSize = new Size(533, 356);
            base.Controls.Add(this.url);
            base.Controls.Add(this.ltext);
            base.Controls.Add(this.pictureBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "FPhoto";
            this.RightToLeftLayout = true;
            base.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Фотография";
            base.TopMost = true;
            base.FormClosed += new FormClosedEventHandler(this.FPhoto_FormClosed);
            ((ISupportInitialize)this.pictureBox1).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }
    }
}
