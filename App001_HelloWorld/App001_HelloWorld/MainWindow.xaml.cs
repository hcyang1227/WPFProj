using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace App001_HelloWorld
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region [Initial]

        DispatcherTimer timer1 = new DispatcherTimer();
        DispatcherTimer timer2 = new DispatcherTimer();
        int timerCounter = 0;

        public MainWindow()
        {
            InitializeComponent();
            timer1.Tick += dispatcherTimer_Tick1;
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 350);
            timer2.Tick += dispatcherTimer_Tick2;
            timer2.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer2.Start();
        }

        #endregion
        #region [UI Elements]
        private void btnHello_Click(object sender, RoutedEventArgs e)
        {
            lblHello.Content = "Hello World !!";
        }

        private void btnCount_Click(object sender, RoutedEventArgs e)
        {
            timerCounter = 0;
            lblCount.Content = timerCounter.ToString();
            timer1.Start();
        }

        private void dispatcherTimer_Tick1(object sender, EventArgs e)
        {
            if (timerCounter < 10)
            {
                timerCounter += 1;
                lblCount.Content = timerCounter.ToString();
            }
        }

        private void dispatcherTimer_Tick2(object sender, EventArgs e)
        {
            lblDate.Content = "Current Datetime:\n" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        #endregion
    }
}