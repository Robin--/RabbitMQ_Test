using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using EasyNetQ;

namespace RabbitMQ_Test
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IBus _bus;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _bus = RabbitHutch.CreateBus("host=localhost");

            var mwVM = new MainWindowViewModel(_bus);
            var mw = new MainWindow() { DataContext = mwVM };
            mw.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _bus.Dispose();
        }
    }
}
