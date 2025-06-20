using System.Windows;

namespace App014_20250620_Task_vs_Thread
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int interval = 50;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!Int32.TryParse(txtRepeat.Text, out int taskNum))
                return;

            //將UI元件設為不可互動
            btnStart.IsEnabled = false;
            txtRepeat.IsEnabled = false;
            txtResult.Clear();

            //開始多層次Task
            await Task.Delay(interval);
            UpdateOutput($"[Main before] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(interval);
            await Task.Run(() => TaskMulti(taskNum));
            await Task.Delay(interval);
            UpdateOutput($"[Main after] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(interval);

            //將UI元件恢復可互動
            btnStart.IsEnabled = true;
            txtRepeat.IsEnabled = true;
        }

        private async Task TaskMulti(int taskNum)
        {
            await Task.Delay(interval);
            UpdateOutput($"[Task{taskNum} before] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(interval);
            //用遞迴寫法依序執行 TaskN -> ... -> Task3 -> Task2 -> Task1
            if (taskNum > 1)
                await Task.Run(() => TaskMulti(taskNum - 1));
            await Task.Delay(interval);
            UpdateOutput($"[Task{taskNum} after] Thread ID: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(interval);
        }

        private void UpdateOutput(string message)
        {
            Dispatcher.Invoke(() =>
            {
                txtResult.AppendText(message + Environment.NewLine);
                txtResult.ScrollToEnd();
            });
        }

    }
}