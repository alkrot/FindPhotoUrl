using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FindFotoUrl
{
    public partial class FindUrlForm : Form
    {
        public FindUrlForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            string pattern = "https?://";
            text = Regex.Replace(text, pattern, "");
            button1.Enabled = false;

            var photos = MainForm.Photos.Where(a => a.Url == text).Distinct().ToList();
            foreach(var photo in photos)
                textBox2.Text += "vk.com/photo" + photo.OwnerId + "_" + photo.PhotoId +";";
            textBox2.Text = textBox2.Text.Substring(0, textBox2.Text.Length - 1);
            Text = "Найдено: " + photos.Count + " фото с указанной ссылкой";
            button1.Enabled = true;
        }
    }
}
