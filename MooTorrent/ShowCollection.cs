using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MooTorrent
{
    class ShowCollection
    {
        readonly public ConcurrentDictionary<string, Show> Shows = new ConcurrentDictionary<string, Show>();

        public void RegisterShow(Show s)
        {
            Shows[s.Name] = s;
        }

        public void RemoveShow(Show s)
        {
            Show garb;
            Shows.TryRemove(s.Name, out garb);
        }
        public void RemoveShow(string s)
        {
            Show garb;
            Shows.TryRemove(s, out garb);
        }

        public bool IsTracking(Show s)
        {
            return Shows.ContainsKey(s.Name);
        }

        public bool IsNew(Show s)
        {
            return IsTracking(s) && Shows[s.Name] < s;
        }

        public bool ShouldDownload(Show s)
        {
            return s.Link != null && IsNew(s) && s.ContainsModifier(s.Modifiers);
        }

        public bool UpdateShow(Show s)
        {
            if (!IsNew(s)) { 
                return false;
            }
            Shows[s.Name] = s;
            return true;
        }
        public string Serialize()
        {
            string pickled = "";
            foreach (var kv in Shows)
            {
                pickled += kv.Value.Serialize() + '\n';
            }
            return pickled.TrimEnd(null);
        }
        public static ShowCollection Deserialize(string shows)
        {
            var sc = new ShowCollection();
            if (shows.Length <= 1)
            {
                return sc;
            }
            foreach (var show in shows.Split('\n'))
            {
                sc.RegisterShow(Show.Deserialize(show));
            }
            return sc;
        }
    }
}
