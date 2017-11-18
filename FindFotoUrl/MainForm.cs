using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FindFotoUrl
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Метод добавление в список ссылки
        /// </summary>
        /// <param name="text"></param>
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
                listBox1.Items.Add(text);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AuthForm autch = new AuthForm();
            autch.ShowDialog();
            label4.Text = User.Default.Id.ToString();
            Vkapi.Get("stats.trackVisitor", "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (_textBox1.Text.Length == 0) return;
                timer1.Enabled = true;
                progress.Show();
                Th = new Thread(Start);
                Th.Start();
                button1.Enabled = false;
                WindowState = FormWindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Запуск получения фото
        /// </summary>
        public void Start()
        {
            Invoke((MethodInvoker)(() =>
            {
                Hide();
            }));
            string text = _textBox1.Text;
            if (text.IndexOf("album", StringComparison.Ordinal) < 0)
            {
                MessageBox.Show("Неправельная ссылка на альбом");
                FormShow();
                Th.Abort();
            }
            text = text.Substring(text.IndexOf("album", StringComparison.Ordinal)).Replace("album", "");
            string[] str = text.Split('_');
            Parsing(str);
            Print(Photos, listBox1);
            FormShow();
        }

        private void FormShow()
        {
            Invoke((MethodInvoker)(() => { Show(); }));
        }

        /// <summary>
        /// Парсинг альбома
        /// </summary>
        /// <param name="str">Ссылка на алььбом мыссив где [0] - id владельца, [1] - id альбома</param>
        public static void Parsing(string[] str)
        {
            JObject jObject = JObject.Parse(Vkapi.Get("photos.get", string.Concat("owner_id=", str[0], "&album_id=", str[1], "&count=1")));
            if (jObject["error"] != null)
            {
                switch (jObject["error"]["error_code"].Value<int>())
                {
                    case 100:
                        MessageBox.Show("Один из пармметров не верен");
                        break;
                    case 200:
                        MessageBox.Show("нет доступа к альбому");
                        break;
                }
                Th.Abort();
            }

            count = jObject["response"]["count"].Value<int>();
            double num = Math.Ceiling(count / 1000.0); //вычесляем количество повторов вызова так макс фотополучение 1000 то делим коли на 1000 и округляем в большую сторону, чтобы выполнилось хотя бы один раз
            int num2 = 0;
            while (num2 < num)
            {
                GetPhoto(str);
                Offset += 1000;
                num2++;
            }
        }

        /// <summary>
        /// Получение фотографий из альбома
        /// </summary>
        /// <param name="str">Ссылка на альбом массив</param>
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
                if (text.IndexOf("vk.com", StringComparison.Ordinal) >= 0)
                {
                    ListAdd(jObject["response"]["items"][i], text);
                }
                else
                {
                    if (int.Parse(jObject["response"]["items"][i]["comments"]["count"].ToString()) > 0)
                    {
                        List<Photo.Comment> comments;
                        text = PhotosGetComments(jObject["response"]["items"][i]["owner_id"].ToString(), jObject["response"]["items"][i]["id"].ToString(), out comments);
                        if (text.Length > 0)
                        {
                            ListAdd(jObject["response"]["items"][i], text, comments);
                        }
                    }
                }
                allcount++;
            }
        }

        /// <summary>
        /// Добавлем фото в список
        /// </summary>
        /// <param name="obj">json</param>
        /// <param name="result">ссылка в описание или комменте</param>
        public static void ListAdd(object obj, string result, List<Photo.Comment> comments = null)
        {
            JObject jObject = JObject.Parse(obj.ToString());
            Photo item = new Photo(jObject["owner_id"].Value<int>(), jObject["id"].Value<int>(), jObject["photo_604"].ToString(), result, comments);
            if (Photos.IndexOf(item) < 0)
            {
                Photos.Add(item);
            }
        }

        /// <summary>
        /// Просмотр комментарий у фото
        /// </summary>
        /// <param name="ownerId">id владельца</param>
        /// <param name="photoId">id фото</param>
        /// <returns>Комментарий с ссылкой</returns>
        public static string PhotosGetComments(string ownerId, string photoId, out List<Photo.Comment> comments)
        {
            Thread.Sleep(500);
            string param = string.Concat(new string[]
            {
                "owner_id=",
                ownerId,
                "&photo_id=",
                photoId,
                "&count=100",
                "&extended=1",
            });

            JObject jObject = JObject.Parse(Vkapi.Get("photos.getComments", param));
            int num = jObject["response"]["count"].Value<int>();
            string lastComment = string.Empty;
            var profile = jObject["response"]["profiles"];
            var group = jObject["response"]["groups"];
            comments = new List<Photo.Comment>();
            for (int i = 0; i < num; i++)
            {
                var comm = jObject["response"]["items"][i];
                string text = comm["text"].ToString();
                var user = profile.SelectToken("$.[?(@.id == " + comm["from_id"] + ")]") ?? group.SelectToken("$.[?(@.id == " + Math.Abs(comm["from_id"].Value<int>()) + ")]");
                var comment = (user["first_name"] != null) ? new Photo.Comment(user["first_name"].ToString(), user["last_name"].ToString(), text)
                    : new Photo.Comment(user["name"].ToString(), "", text);
                if (text.IndexOf("vk.com", StringComparison.Ordinal) >= 0)
                {
                    
                    lastComment = text;
                }
                comments.Add(comment);
            }
            return lastComment;
        }

        /// <summary>
        /// Вывод списка в контрол listbox
        /// </summary>
        /// <param name="photos">Список фото</param>
        /// <param name="text">listbox</param>
        public void Print(List<Photo> photos, ListBox text)
        {
            foreach (Photo current in photos)
            {
                var photoLink = "vk.com/photo" + current.OwnerId + "_" + current.PhotoId;
                if (text.Items.IndexOf(photoLink) < 0)
                {
                    ListText(photoLink + "\r");
                }
            }
        }

        /// <summary>
        /// Открытие фото в браузере
        /// </summary>
        public void OpenPhoto()
        {
            if (listBox1.Items.Count > 0 && listBox1.SelectedIndex >= 0)
                Process.Start("http://" + listBox1.Items[listBox1.SelectedIndex]);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
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
            FindUrlForm findUrl = new FindUrlForm();
            findUrl.ShowDialog();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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
            Clipboard.SetText(listBox1.Items[listBox1.SelectedIndex].ToString());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Photos.Count > 0 && listBox1.SelectedIndex >= 0)
            {
                if (!Openform)
                {
                    Openform = true;
                    Ph = new FPhotoForm();
                    Ph.Show();
                }
                int selectedIndex = listBox1.SelectedIndex;
                Ph.pictureBox1.ImageLocation = Photos[selectedIndex].PhotoUrl;
                Ph.ltext.Text = (Photos[selectedIndex].Comments != null) ? String.Join("\r\n", Photos[selectedIndex].Comments) : Photos[selectedIndex].Text;
                Ph.Left = Left + Width + 10;
                Ph.url.Text = Photos[selectedIndex].Url;
            }
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                OpenPhoto();
            }
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            button2.Visible = (listBox1.Items.Count > 0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count > 0 && count == allcount || Th.ThreadState == System.Threading.ThreadState.Aborted || Th.ThreadState == System.Threading.ThreadState.Stopped)
            {
                progress.Hide();
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                    Visible = true;
                }
                timer1.Enabled = false;
                Th.Abort();
            }
            progress.label1.Text = allcount + "[" + count + "]";
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            progress.Hide();
            Photos.Clear();
            listBox1.Items.Clear();
            button1.Enabled = true;
            button2.Visible = false;
            Offset = 0;
            allcount = 0;
            count = 0;
        }
    }
}
