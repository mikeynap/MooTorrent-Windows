using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace MooTorrent
{
    class ShowController
    {
        public readonly ShowCollection MyShows;
        private DispatcherTimer fetchTimer;
        public ShowController() { }

        public ShowController(ShowCollection shows)
        {
            MyShows = shows;
        }

        public ShowController(string pickled)
        {
            MyShows = ShowCollection.Deserialize(pickled);
        }

        public void AutoFetch(int seconds)
        {
            if (fetchTimer != null)
            {
                fetchTimer.Stop();
            }
            fetchTimer = new DispatcherTimer();
            fetchTimer.Tick += (object s, EventArgs e) => FetchShows();
            fetchTimer.Interval = new TimeSpan(0, 0, seconds);
            fetchTimer.Start();
        }

        public void StopAutoFetch()
        {
            fetchTimer.Stop();
            fetchTimer = null;
        }


        public bool FetchShows()
        {
            Console.WriteLine("Fetching Shows Idiot");
            var shows = EZTV.GetShows();
            if (shows == null)
            {
                return false;
            }
            foreach (Show s in shows)
            {
                if (MyShows.ShouldDownload(s))
                {
                    MyShows.UpdateShow(s);
                    Process.Start(s.Link);
                }
                else
                {
                    Console.WriteLine("Skipping " + s.ToString());
                }
            }
            return true;
        }
    }
}
