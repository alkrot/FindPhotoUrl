using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FindFotoUrl
{
    partial class MainForm
    {
        /// <summary>
        /// Делегат для добавления из другого потока в список ссылки
        /// </summary>
        /// <param name="text">ссылка</param>
        private delegate void AddList(string text);

        private static Vkapi Vkapi = new Vkapi(); // инцилизация вк-апи

        public static List<Photo> Photos = new List<Photo>(); //хранит список фотографий

        public static int Offset; //смещение

        private static int allcount; //всего фото

        private static int count; //количество полученых

        public static FPhotoForm Ph; //для просмотра фотографии по ссылки

        //public static MainForm Main = new MainForm();

        public static Thread Th; //Новый поток

        private Progress progress = new Progress(); //Прогресс бар

        public static bool Openform; //открыта ли форма для просмотра фотографии

        /*----------------------------*/
        private IContainer components;

        private TextBox _textBox1;

        private Label _label1;

        private Button button1;

        private Label _label2;

        private Label _label3;

        private Label label4;

        private Button button2;

        private ContextMenuStrip _contextMenuStrip1;

        private ToolStripMenuItem _toolStripMenuItem1;

        private ToolStripMenuItem _открытьToolStripMenuItem;

        private ListBox listBox1;

        private System.Windows.Forms.Timer timer1;

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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            _textBox1 = new TextBox();
            _label1 = new Label();
            button1 = new Button();
            _label2 = new Label();
            _label3 = new Label();
            label4 = new Label();
            button2 = new Button();
            listBox1 = new ListBox();
            _contextMenuStrip1 = new ContextMenuStrip(components);
            _toolStripMenuItem1 = new ToolStripMenuItem();
            _открытьToolStripMenuItem = new ToolStripMenuItem();
            timer1 = new System.Windows.Forms.Timer(components);
            _contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            _textBox1.Location = new Point(120, 9);
            _textBox1.Name = "_textBox1";
            _textBox1.Size = new Size(252, 20);
            _textBox1.TabIndex = 0;
            _textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label1
            // 
            _label1.AutoSize = true;
            _label1.Location = new Point(12, 9);
            _label1.Name = "_label1";
            _label1.Size = new Size(102, 13);
            _label1.TabIndex = 1;
            _label1.Text = "Ссылка на альбом";
            // 
            // button1
            // 
            button1.Location = new Point(297, 35);
            button1.Name = "_button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Парсить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            _label2.AutoSize = true;
            _label2.Location = new Point(12, 68);
            _label2.Name = "_label2";
            _label2.Size = new Size(130, 13);
            _label2.TabIndex = 4;
            _label2.Text = "Фотографии с сылками";
            // 
            // label3
            // 
            _label3.AutoSize = true;
            _label3.Location = new Point(12, 283);
            _label3.Name = "_label3";
            _label3.Size = new Size(42, 13);
            _label3.TabIndex = 5;
            _label3.Text = "Ваш id:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(54, 283);
            label4.Name = "_label4";
            label4.Size = new Size(0, 13);
            label4.TabIndex = 6;
            // 
            // button2
            // 
            button2.Location = new Point(257, 279);
            button2.Name = "_button2";
            button2.Size = new Size(115, 20);
            button2.TabIndex = 7;
            button2.Text = "Найти ссылку";
            button2.UseVisualStyleBackColor = true;
            button2.Visible = false;
            button2.Click += button2_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(15, 84);
            listBox1.Name = "_listBox1";
            listBox1.Size = new Size(357, 186);
            listBox1.TabIndex = 8;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            listBox1.SelectedValueChanged += listBox1_SelectedValueChanged;
            listBox1.KeyPress += listBox1_KeyPress;
            listBox1.MouseDown += listBox1_MouseDown;
            // 
            // contextMenuStrip1
            // 
            _contextMenuStrip1.Items.AddRange(new ToolStripItem[] {
            _toolStripMenuItem1,
            _открытьToolStripMenuItem});
            _contextMenuStrip1.Name = "_contextMenuStrip1";
            _contextMenuStrip1.Size = new Size(140, 48);
            // 
            // toolStripMenuItem1
            // 
            _toolStripMenuItem1.Name = "_toolStripMenuItem1";
            _toolStripMenuItem1.Size = new Size(139, 22);
            _toolStripMenuItem1.Text = "Копировать";
            _toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // открытьToolStripMenuItem
            // 
            _открытьToolStripMenuItem.Name = "_открытьToolStripMenuItem";
            _открытьToolStripMenuItem.Size = new Size(139, 22);
            _открытьToolStripMenuItem.Text = "Открыть";
            _открытьToolStripMenuItem.Click += открытьToolStripMenuItem_Click;
            // 
            // timer1
            // 
            timer1.Interval = 10;
            timer1.Tick += timer1_Tick;
            // 
            // Index
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 304);
            Controls.Add(listBox1);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(_label3);
            Controls.Add(_label2);
            Controls.Add(button1);
            Controls.Add(_label1);
            Controls.Add(_textBox1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Index";
            Text = "Поиск фото с сылками";
            TopMost = true;
            FormClosed += Main_FormClosed;
            Load += MainForm_Load;
            _contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

        }
    }
}
