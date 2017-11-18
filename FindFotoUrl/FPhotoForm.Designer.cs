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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.url = new System.Windows.Forms.LinkLabel();
            this.ltext = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(533, 356);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // url
            // 
            this.url.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.url.Location = new System.Drawing.Point(0, 337);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(533, 19);
            this.url.TabIndex = 2;
            this.url.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.url_LinkClicked);
            // 
            // ltext
            // 
            this.ltext.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ltext.Location = new System.Drawing.Point(0, 241);
            this.ltext.Name = "ltext";
            this.ltext.ReadOnly = true;
            this.ltext.Size = new System.Drawing.Size(533, 96);
            this.ltext.TabIndex = 3;
            this.ltext.Text = "";
            // 
            // FPhotoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(533, 356);
            this.Controls.Add(this.ltext);
            this.Controls.Add(this.url);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FPhotoForm";
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Фотография";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FPhoto_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        public RichTextBox ltext;
    }
}
