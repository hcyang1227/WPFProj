using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace App001_HelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [Initial]

        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int intCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            ShowCurrentDatetime();
        }

        #endregion
        #region [UI Elements]
        private void btnHello_Click(object sender, RoutedEventArgs e)
        {
            lblHello.Content = "Hello World !!";
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            //取消現有計數，並建立新CancellationTokenSource
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            //計數器初始化
            intCounter = 0;
            ChangeCountLabel();

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
                        ChangeCountLabel();
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Task was canceled.");
                }
            });
        }
        private void ChangeCountLabel()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => { lblCount.Content = intCounter.ToString(); }));
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
            Application.Current.Dispatcher.Invoke(new Action(() => { lblDate.Content = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"); }));
        }

        #endregion
    }
}