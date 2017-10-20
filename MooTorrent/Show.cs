using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MooTorrent
{
    class Show
    {
        public Show()
        {

        }

        public Show(string title, string episode, string modifiers, DateTime pubdate, string link): this(title)
        {
            this.Episode = episode;
            this.Modifiers = modifiers;
            this.Date = pubdate;
            this.Link = link;
        }

        public Show(string title)
        {
            this.Modifiers = "";
            this.Date = DateTime.Now;
            this.Link = "";
            Parse(title);
        }

        public Show(string title, string modifiers) : this(title)
        {
            this.Modifiers = Modifiers;
        }

        public Show(string title, string episode, string modifiers): this(title)
        {
            this.Episode = episode;
            this.Modifiers = modifiers;
        }

        public Show(string title, string episode, string modifiers, string pubdate, string link)
        {
            DateTime date;
            if (DateTime.TryParse(pubdate, out date) == false)
            {
                date = DateTime.Now;
            }
            this.Episode = episode;
            this.Modifiers = modifiers;
            this.Date = date;
            this.Link = link;
            Parse(title);
        }

        public string Name { get; private set; }
        public string Episode { get; private set; }
        public string Modifiers { get; private set; }
        public DateTime Date { get; private set; }
        public string Link { get; private set; }
        private static Regex regex = new Regex("([A-Za-z0-9. -]+) (S[0-9]+E[0-9]+|20[0-9][0-9] [0-9][0-9] [0-9][0-9]) (.*)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private void Parse(string title) 
        {
            // Find matches.
            MatchCollection matches = regex.Matches(title);
            if (matches.Count < 1 || matches[0].Groups.Count < 2) {
                this.Name = title;
                return;
            }

            GroupCollection groups = matches[0].Groups;
            this.Name = groups[1].Value;
            this.Episode = groups[2].Value;
            if (groups.Count > 3)
            {
                this.Modifiers = groups[3].Value;
            }
        }

        public bool ContainsModifier(string modifier)
        {
            return this.Modifiers != null && this.Modifiers.Contains(modifier);
        }

        public override string ToString()
        {
            return base.ToString() + ": " + this.Name + " " + this.Episode + " " + this.Modifiers + " " +  this.Link +  " " + this.Date;
        }

        public static bool operator ==(Show s1, Show s2)
        {
            return s1.Name == s2.Name && s1.Episode == s2.Episode && s1.Modifiers == s2.Modifiers;
        }

        public static bool operator !=(Show s1, Show s2) => !(s1 == s2);

        public static bool operator <(Show s1, Show s2)
        {
            if (s1.Episode == null && s2.Episode != null) {
                return true;
            } else if (s1.Episode == null || s2.Episode == null)
            {
                return false;
            }
            if (s1.Episode.StartsWith("S") && s2.Episode.StartsWith("S")) {
                return String.Compare(s1.Episode, s2.Episode) < 0;
            }
            DateTime date1, date2;
            if (!DateTime.TryParse(s1.Episode, out date1) || !DateTime.TryParse(s2.Episode, out date2))
            {
                return s1.Date.CompareTo(s2.Date) < 0;
            }
            return date1.CompareTo(date2) < 0;
        }

        public static bool operator >(Show s1, Show s2)
        {
            return !(s1 < s2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Show))
            {
                return false;
            }
            return this == (Show)obj;
        }

        public override int GetHashCode()
        {
            return (int)Show.CalculateHash(this.ToString());
        }
        private static UInt64 CalculateHash(string read)
        {
            UInt64 hashedValue = 3074457345618258791ul;
            for (int i = 0; i < read.Length; i++)
            {
                hashedValue += read[i];
                hashedValue *= 3074457345618258799ul;
            }
            return hashedValue;
        }

        public string Serialize()
        {
            return Name + '\t' + Episode + '\t' + Modifiers + '\t' + Date.ToString() + '\t' + Link;
        }

        public static Show Deserialize(string show)
        {
            Show s = new Show();
            var chunks = show.Split('\t');
            if (chunks.Length > 4)
            {
                s.Link = chunks[4];
            }
            if (chunks.Length > 3)
            {
                DateTime date;
                DateTime.TryParse(chunks[3], out date);
                s.Date = date;
            }
            if (chunks.Length > 2)
            {
                s.Modifiers = chunks[2];
            }
            if (chunks.Length > 1)
            {
                s.Episode = chunks[1];
            }
            if (chunks.Length > 0)
            {
                s.Name = chunks[0];
            }
            return s;
        }




    }
}
