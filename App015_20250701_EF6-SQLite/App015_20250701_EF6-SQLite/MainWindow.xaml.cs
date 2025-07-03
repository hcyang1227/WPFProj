using App015_20250701_EF6_SQLite.ViewModels;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace App015_20250701_EF6_SQLite
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ellipse = sender as Ellipse;
            var defect = ellipse?.DataContext as DefectModel;
            var vm = DataContext as MainViewModel;
            if (defect != null && vm != null)
            {
                // 這裡假設你有一個查詢方法取得圖片路徑
                var imagePath = vm.GetImagePathForDefect(defect);
                vm.SelectedImagePath = File.Exists(imagePath) ? imagePath : null;
            }
        }
    }
}
