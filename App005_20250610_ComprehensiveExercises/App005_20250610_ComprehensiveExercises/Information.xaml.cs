using System.Collections.Generic;
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

namespace App005_20250610_ComprehensiveExercises
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //Save panel info to dictionary
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("FirstName", txtFName.Text);
            dict.Add("LastName", txtLName.Text);
            dict.Add("StreetName", txtStreet.Text);
            dict.Add("ZipCode", txtZip.Text);
            dict.Add("City", cbbCity.Text);

            //Show input result to messagebox
            string fullContent = "";
            foreach (string str in dict.Keys) { fullContent += str + ": " + dict[str] + "\n"; }
            MessageBox.Show(fullContent, "Input Information", MessageBoxButton.OKCancel, MessageBoxImage.Information);
        }
    }
}