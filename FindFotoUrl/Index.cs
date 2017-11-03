using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FindFotoUrl
{
	public class Index : Form
	{
		private delegate void AddList(string text);

		

		private static readonly Vkapi Vkapi = new Vkapi();

		public static List<Photo> Photos = new List<Photo>();

		public static int Offset;

		private static int _allcount;

		private static int _count;

		public static FPhoto Ph;

		public static Index Main = new Index();

		public static Thread Th;

		private readonly Progress _progress = new Progress();

		public static bool Openform;

		private IContainer components;

		private TextBox _textBox1;

		private Label _label1;

		private Button _button1;

		private Label _label2;

		private Label _label3;

		private Label _label4;

		private Button _button2;

		private ContextMenuStrip _contextMenuStrip1;

		private ToolStripMenuItem _toolStripMenuItem1;

		private ToolStripMenuItem _открытьToolStripMenuItem;

		private ListBox _listBox1;

		private System.Windows.Forms.Timer _timer1;

		public Index()
		{
            InitializeComponent();
		}

		private void ListText(string text)
		{
			bool invokeRequired = InvokeRequired;
			if (invokeRequired)
			{
                BeginInvoke(new AddList(ListText), new object[]
				{
					text
				});
			}
			else
			{
                _listBox1.Items.Add(text);
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			AuthForm autch = new AuthForm();
			autch.ShowDialog();
            _label4.Text = User.Default.Id.ToString();
            Vkapi.Get("stats.trackVisitor", "");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
                if (_textBox1.Text.Length == 0) return;
				_timer1.Enabled = true;
				_progress.Show();
				Th = new Thread(Start);
				Th.Start();
                _button1.Enabled = false;
                WindowState = FormWindowState.Minimized;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void Start()
		{
			string text = _textBox1.Text;
            if (text.IndexOf("album", StringComparison.Ordinal) < 0)
            {
                MessageBox.Show("Непраильная ссылка на альбом");
                Th.Abort();
            }
			text = text.Substring(text.IndexOf("album", StringComparison.Ordinal)).Replace("album", "");
			string[] str = text.Split('_');
            Parsing(str);
			Print(Photos, _listBox1);
		}

		public static void Parsing(string[] str)
		{
			JObject jObject = JObject.Parse(Vkapi.Get("photos.get", string.Concat("owner_id=", str[0], "&album_id=", str[1], "&count=1")));
            if(jObject["error"] != null && jObject["error"]["error_code"].Value<int>() == 200)
            {
                MessageBox.Show("нет доступа к альбому");
                Th.Abort();
            }
            _count = jObject["response"]["count"].Value<int>();
			double num = Math.Ceiling(_count / 1000.0);
			int num2 = 0;
			while (num2 < num)
			{
                GetPhoto(str);
                Offset += 1000;
				num2++;
			}
		}

		public static void GetPhoto(string[] str)
		{
			JObject jObject = JObject.Parse(Vkapi.Get("photos.get", string.Concat(new object[]
			{
				"owner_id=",
				str[0],
				"&album_id=",
				str[1],
				"&extended=1&offset=",
                Offset
            })));
			JArray jArray = JArray.Parse(jObject["response"]["items"].ToString());
			for (int i = 0; i < jArray.Count; i++)
			{
				string text = jObject["response"]["items"][i]["text"].ToString();
				bool flag = text.IndexOf("vk.com", StringComparison.Ordinal) >= 0;
				if (flag)
				{
                    ListAdd(jObject["response"]["items"][i], text);
				}
				else
				{
					bool flag2 = int.Parse(jObject["response"]["items"][i]["comments"]["count"].ToString()) > 0;
					if (flag2)
					{
						text = PhotosGetComments(jObject["response"]["items"][i]["owner_id"].ToString(), jObject["response"]["items"][i]["id"].ToString());
						bool flag3 = text.Length > 0;
						if (flag3)
						{
                            ListAdd(jObject["response"]["items"][i], text);
						}
					}
				}
                _allcount++;
			}
		}

		public static void ListAdd(object obj, string result)
		{
			JObject jObject = JObject.Parse(obj.ToString());
			Photo item = new Photo(jObject["owner_id"].Value<int>(), jObject["id"].Value<int>(), jObject["photo_604"].ToString(), result);
			bool flag = Photos.IndexOf(item) < 0;
			if (flag)
			{
                Photos.Add(item);
			}
		}

		public static string PhotosGetComments(string ownerId, string photoId)
		{
			Thread.Sleep(500);
			JObject jObject = JObject.Parse(Vkapi.Get("photos.getComments", string.Concat(new string[]
			{
				"owner_id=",
				ownerId,
				"&photo_id=",
				photoId,
				"&count=100"
			})));
			int num = jObject["response"]["count"].Value<int>();
			string result;
			for (int i = 0; i < num; i++)
			{
				string text = jObject["response"]["items"][i]["text"].ToString();
				bool flag = text.IndexOf("vk.com", StringComparison.Ordinal) >= 0;
				if (flag)
				{
					result = text;
					return result;
				}
			}
			result = "";
			return result;
		}

		public void Print(List<Photo> photos, ListBox text)
		{
			foreach (Photo current in photos)
			{
				bool flag = text.Items.IndexOf(string.Concat(new object[]
				{
					"vk.com/photo",
					current.OwnerId,
					"_",
					current.PhotoId
				})) < 0;
				if (flag)
				{
                    ListText(string.Concat(new object[]
					{
						"vk.com/photo",
						current.OwnerId,
						"_",
						current.PhotoId,
						"\n"
					}));
				}
			}
		}

		public void OpenPhoto()
		{
			Process.Start("http://" + _listBox1.Items[_listBox1.SelectedIndex]);
		}

		private void Index_FormClosed(object sender, FormClosedEventArgs e)
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
			string[] files = Directory.GetFiles(folderPath);
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string path = array[i];
				try
				{
					File.Delete(path);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

	    private void button2_Click(object sender, EventArgs e)
		{
			FindUrl findUrl = new FindUrl();
			findUrl.ShowDialog();
		}

		private void listBox1_MouseDown(object sender, MouseEventArgs e)
		{
			bool flag = e.Button == MouseButtons.Right;
			if (flag)
			{
                _contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
			}
		}

		private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
		{
            OpenPhoto();
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(_listBox1.Items[_listBox1.SelectedIndex].ToString());
		}

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool flag = Photos.Count > 0;
			if (flag)
			{
				bool flag2 = !Openform;
				if (flag2)
				{
                    Openform = true;
                    Ph = new FPhoto();
                    Ph.Show();
				}
				int selectedIndex = _listBox1.SelectedIndex;
                Ph.pictureBox1.ImageLocation = Photos[selectedIndex].PhotoUrl;
                Ph.ltext.Text = Photos[selectedIndex].Text;
                Ph.Left = Left + Width + 10;
                Ph.url.Text = Photos[selectedIndex].Url;
			}
		}

		private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			bool flag = e.KeyChar == '\r';
			if (flag)
			{
                OpenPhoto();
			}
		}

		private void listBox1_SelectedValueChanged(object sender, EventArgs e)
		{
            _button2.Visible = (_listBox1.Items.Count > 0);
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			bool flag = _count > 0 && _count == _allcount;
			if (flag || Th.ThreadState == System.Threading.ThreadState.Aborted || Th.ThreadState == System.Threading.ThreadState.Stopped)
			{
                _progress.Hide();
				bool flag2 = WindowState == FormWindowState.Minimized;
				if (flag2)
				{
                    WindowState = FormWindowState.Normal;
				}
                _timer1.Enabled = false;
                Th.Abort();
			}
            _progress.label1.Text = string.Concat(new object[]
			{
				_allcount,
				"[",
				_count,
				"]"
			});
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
            _progress.Hide();
            Photos.Clear();
            _listBox1.Items.Clear();
            _button1.Enabled = true;
            _button2.Visible = false;
            Offset = 0;
            _allcount = 0;
            _count = 0;
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
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Index));
            _textBox1 = new TextBox();
            _label1 = new Label();
            _button1 = new Button();
            _label2 = new Label();
            _label3 = new Label();
            _label4 = new Label();
            _button2 = new Button();
            _listBox1 = new ListBox();
            _contextMenuStrip1 = new ContextMenuStrip(components);
            _toolStripMenuItem1 = new ToolStripMenuItem();
            _открытьToolStripMenuItem = new ToolStripMenuItem();
            _timer1 = new System.Windows.Forms.Timer(components);
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
            _button1.Location = new Point(297, 35);
            _button1.Name = "_button1";
            _button1.Size = new Size(75, 23);
            _button1.TabIndex = 2;
            _button1.Text = "Парсить";
            _button1.UseVisualStyleBackColor = true;
            _button1.Click += button1_Click;
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
            _label4.AutoSize = true;
            _label4.Location = new Point(54, 283);
            _label4.Name = "_label4";
            _label4.Size = new Size(0, 13);
            _label4.TabIndex = 6;
            // 
            // button2
            // 
            _button2.Location = new Point(257, 279);
            _button2.Name = "_button2";
            _button2.Size = new Size(115, 20);
            _button2.TabIndex = 7;
            _button2.Text = "Найти ссылку";
            _button2.UseVisualStyleBackColor = true;
            _button2.Visible = false;
            _button2.Click += button2_Click;
            // 
            // listBox1
            // 
            _listBox1.FormattingEnabled = true;
            _listBox1.Location = new Point(15, 84);
            _listBox1.Name = "_listBox1";
            _listBox1.Size = new Size(357, 186);
            _listBox1.TabIndex = 8;
            _listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            _listBox1.SelectedValueChanged += listBox1_SelectedValueChanged;
            _listBox1.KeyPress += listBox1_KeyPress;
            _listBox1.MouseDown += listBox1_MouseDown;
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
            _timer1.Interval = 10;
            _timer1.Tick += timer1_Tick;
            // 
            // Index
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(384, 304);
            Controls.Add(_listBox1);
            Controls.Add(_button2);
            Controls.Add(_label4);
            Controls.Add(_label3);
            Controls.Add(_label2);
            Controls.Add(_button1);
            Controls.Add(_label1);
            Controls.Add(_textBox1);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = ((Icon)(resources.GetObject("$this.Icon")));
            MaximizeBox = false;
            Name = "Index";
            Text = "Поиск фото с сылками";
            TopMost = true;
            FormClosed += Index_FormClosed;
            Load += Form1_Load;
            _contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();

		}
	}
}
