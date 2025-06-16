using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace App001_HelloWorld
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string labelHello;
        private string labelCounter;
        private string labelDatetime;

        public string LabelHello
        {
            get => labelHello;
            set { labelHello = value; OnPropertyChanged(); }
        }
        public string LabelCounter
        {
            get => labelCounter;
            set { labelCounter = value; OnPropertyChanged(); }
        }

        public string LabelDatetime
        {
            get => labelDatetime;
            set { labelDatetime = value; OnPropertyChanged(); }
        }

        public ICommand UpdateLabelHelloCommand { get; }
        public ICommand UpdateCountToTenCommand { get; }

        public MainViewModel()
        {
            LabelHello = "";
            LabelCounter = "";
            UpdateLabelHelloCommand = new RelayCommand(() => LabelHello = "Hello World !!");
            UpdateCountToTenCommand = new RelayCommand(CountToTen);
            ShowCurrentDatetime();
        }

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int intCounter = 0;
        private void CountToTen()
        {
            //取消現有計數，並建立新CancellationTokenSource
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            //計數器初始化
            intCounter = 0;
            LabelCounter = intCounter.ToString();

            //建立任務(顯示0~10)
            Task task = Task.Run(async () =>
            {
                try
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(300);
                        token.ThrowIfCancellationRequested();
                        intCounter = i;
                        LabelCounter = intCounter.ToString();
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was canceled.");
                }
            });
        }

        private async void ShowCurrentDatetime()
        {
            while (true)
            {
                await Task.Delay(10);
                ChangeDatetimeLabel();
            }
        }

        private void ChangeDatetimeLabel()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                LabelDatetime = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
            }));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
