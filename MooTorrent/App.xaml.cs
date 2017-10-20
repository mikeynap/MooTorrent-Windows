using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using System.Threading;

namespace MooTorrent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon notifyIcon;
        private ShowController showCtrl;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            showCtrl = new ShowController(MooTorrent.Properties.Settings.Default.Shows);
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                showCtrl.FetchShows();
            }).Start();

            showCtrl.AutoFetch(100);
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
        }

        protected override void OnExit(ExitEventArgs e)
        {
            MooTorrent.Properties.Settings.Default.Shows = showCtrl.MyShows.Serialize();
            MooTorrent.Properties.Settings.Default.Save();
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}

