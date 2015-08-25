namespace AgarIo.AdminPanel.ViewModels
{
    using System.Threading;
    using System.Threading.Tasks;
    using AgarIo.AdminPanel.Events;
    using AgarIo.Contract;
    using AgarIo.Contract.AdminCommands;

    using Caliburn.Micro;

    public class ConnectionViewModel : Screen, IHandleWithTask<ConnectedEvent>, IHandle<DisconnectedEvent>, IHandle<ConnectingEvent>
    {
        private readonly IEventAggregator _eventAggregator;

        private readonly IConnection _connection;

        private CancellationTokenSource _cancellationTokenSource;

        private string _userName;

        private string _password;

        private string _host;

        private int _port;

        private bool _isConnected;

        private bool _isConnecting;

        private bool _isEditingEnabled;

        public ConnectionViewModel(IEventAggregator eventAggregator, IConnection connection)
        {
            _eventAggregator = eventAggregator;
            _connection = connection;
            Host = "localhost";
            Port = 8000;
            UserName = string.Empty;
            Password = string.Empty;

            IsConnected = false;
            IsConnecting = false;
            IsEditingEnabled = true;

            eventAggregator.Subscribe(this);
        }

        public string UserName
        {
            get { return _userName; }

            set
            {
                if (value == _userName) return;
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
            }
        }

        public string Password
        {
            get { return _password; }

            set
            {
                if (value == _password) return;
                _password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public string Host
        {
            get { return _host; }

            set
            {
                if (value == _host) return;
                _host = value;
                NotifyOfPropertyChange(() => Host);
            }
        }

        public int Port
        {
            get { return _port; }

            set
            {
                if (value == _port) return;
                _port = value;
                NotifyOfPropertyChange(() => Port);
            }
        }

        public bool IsConnected
        {
            get { return _isConnected; }

            private set
            {
                if (value == _isConnected) return;
                _isConnected = value;
                NotifyOfPropertyChange(() => IsConnected);
            }
        }

        public bool IsConnecting
        {
            get { return _isConnecting; }

            private set
            {
                if (value == _isConnecting) return;
                _isConnecting = value;
                NotifyOfPropertyChange(() => IsConnecting);
            }
        }

        public bool IsEditingEnabled
        {
            get { return _isEditingEnabled; }

            set
            {
                if (value == _isEditingEnabled) return;
                _isEditingEnabled = value;
                NotifyOfPropertyChange(() => IsEditingEnabled);
            }
        }

        public async Task ConnectAsync()
        {
            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();
            await _connection.ConnectAsync(_host, _port, _cancellationTokenSource.Token);
        }

        public async Task DisconnectAsync()
        {
            _cancellationTokenSource.Cancel();
            await _connection.DisconnectAsync();
        }

        public void Handle(DisconnectedEvent message)
        {
            IsConnected = false;
            IsConnecting = false;
            IsEditingEnabled = true;
        }

        public void Handle(ConnectingEvent message)
        {
            IsConnected = false;
            IsConnecting = true;
            IsEditingEnabled = false;
        }

        public async Task Handle(ConnectedEvent message)
        {
            IsConnected = true;
            IsConnecting = false;
            IsEditingEnabled = false;

            var loginResponseDto = await _connection.DispatchLoginAsync(
                new LoginDto { IsAdmin = true, Login = UserName, Password = Password },
                _cancellationTokenSource.Token);

            if (loginResponseDto.ErrorCode != 0)
            {
                await DisconnectAsync();
            }
            else
            {
                await _eventAggregator.PublishOnUIThreadAsync(new LoggedInEvent());
            }

            await _connection.DispatchCommandAsync<AdminCommandResponseDto>(
                new DefineWorldAdminCommandDto { Size = 2000 },
                _cancellationTokenSource.Token);
        }
    }
}