using System.Text.RegularExpressions;

namespace FindFotoUrl
{
    public class Photo
    {
        public int OwnerId { get; }

        public int PhotoId { get; }

        public string Text { get; private set; }

        public string PhotoUrl { get; }

        public string Url { get; private set; }

        public Photo(int ownerId, int photoId, string photoUrl, string inText)
        {
            OwnerId = ownerId;
            PhotoId = photoId;
            PhotoUrl = photoUrl;
            AddText(inText);
        }

        private void AddText(string input)
        {
            string pattern = "(vk.com\\/[0-9|a-z|_|\\.]+)\\s*";
            string pattern2 = "[\\s*(https?:\\/\\/)?(m.)?vk.com\\/[0-9|a-z|_|\\.]+";
            Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Url = match.Groups[0].ToString().TrimStart(new char[0]).TrimEnd(new char[0]);
            Text = Regex.Replace(input, pattern2, " ");
        }
    }
}
