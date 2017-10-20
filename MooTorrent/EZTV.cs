using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Diagnostics;

namespace MooTorrent
{
    class EZTV
    {
        public static List<Show> GetShows()
        {
            var list = new List<Show>();
            System.Xml.XmlReader r;
            try
            {
                r = System.Xml.XmlReader.Create("https://eztv.ag/ezrss.xml");
            } catch
            {
                return null;
            }

            var x = XDocument.Load(r);
            r.Close();
            var items = x.Root.Elements("channel");
            if (items == null)
            {
                Console.WriteLine("No Data from RSS...");
                return list;
            }

            foreach (XElement item in items.Elements("item"))
            {
                var title = (String)item.Element("title");
                var tName = XName.Get("magnetURI", "http://xmlns.ezrss.it/0.1/");
                var link = (String)item.Element(tName);
                var date = (String)item.Element("pubDate");
                if (link == null)
                {
                    Console.WriteLine("No Magnet Link For" + title);
                    continue;
                }
                list.Add(new Show(title,"","",date,link));
            }
            return list;       
        }

    }
}
