using System.IO.Pipes;
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

namespace App002_DataBindingTest
{
    public partial class MainWindow : Window
    {
        Person person = new Person { Name = "Salman", Age = 26 };

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = person;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string message = person.Name + " is " + person.Age;
            MessageBox.Show(message);
        }

        public class Person
        {
            private string nameValue;
            public string Name
            {
                get { return nameValue; }
                set { nameValue = value; }
            }

            private double ageValue;

            public double Age
            {
                get { return ageValue; }

                set
                {
                    if (value != ageValue)
                    {
                        ageValue = value;
                    }
                }
            }
        }
    }
}