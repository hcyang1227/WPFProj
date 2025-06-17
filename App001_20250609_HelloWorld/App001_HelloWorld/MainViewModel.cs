using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace App001_HelloWorld
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string labelHello = "";
        private string labelCounter = "";
        private string labelDatetime = "";

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
            UpdateLabelHelloCommand = new RelayCommand(() => LabelHello = "Hello World !!");
            UpdateCountToTenCommand = new RelayCommand(CountToTen);
            Task task = Task.Run(ShowCurrentDatetime);
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

            //Task.Run()接收的是沒有參數的委派（delegate），但我的方法有參數
            Task task = Task.Run(() => CountToTenTask(token));
        }

        private async Task CountToTenTask(CancellationToken token)
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
        }

        private async Task ShowCurrentDatetime()
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


        public event PropertyChangedEventHandler? PropertyChanged; 
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
