using System.Collections.Generic;
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
        
        public List<Comment> Comments { get; }

        public Photo(int ownerId, int photoId, string photoUrl, string inText,List<Comment> comments = null)
        {
            OwnerId = ownerId;
            PhotoId = photoId;
            Comments = comments;
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

        public class Comment
        {
            string firstName;
            string lastName;
            string message;

            public Comment(string fn,string ln,string m)
            {
                firstName = fn;
                lastName = ln;
                message = m;
            }

            public override string ToString()
            {
                return string.Format("{0} {1}: {2}", firstName, lastName, message);
            }
        }

        public override string ToString()
        {
            return "vk.com/photo-" + OwnerId + "_" + PhotoId;
        }
    }
}
