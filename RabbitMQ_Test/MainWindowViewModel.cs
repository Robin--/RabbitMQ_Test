using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasyNetQ;

namespace RabbitMQ_Test
{
    public class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private string _appId = Guid.NewGuid().ToString();
        private IBus _bus;
        public MainWindowViewModel(IBus bus)
        {
            _bus = bus;
            _bus.Subscribe<EasyMsg>(_appId, HandleStringMsg);
            SubcriptionLog = "Listening";
            SendMessage = new RelayCommand(SendNewMessage);
            WindowTitle = "App: " + _appId;
        }

        public ICommand SendMessage { get; set; }

        public string WindowTitle { get; set; }

        private string _SubscriptionLog;
        public string SubcriptionLog 
        {
            get { return _SubscriptionLog; }
            set
            {
                _SubscriptionLog = String.Format("{0}{1}{2}", _SubscriptionLog, value, Environment.NewLine);
                OnPropertyChanged("SubcriptionLog");
            }
        }

        private void SendNewMessage(object data)
        {
            using (var pubChannel = _bus.OpenPublishChannel())
            {
                pubChannel.Publish<EasyMsg>(new EasyMsg() { Data = String.Format("Msg from window {0}", _appId) });
            }
        }

        private void HandleStringMsg(EasyMsg msg)
        {
            SubcriptionLog = msg.Data;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string caller)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
        }

        public void Dispose()
        {
            if (_bus != null)
                _bus.Dispose();
        }
    }
}
