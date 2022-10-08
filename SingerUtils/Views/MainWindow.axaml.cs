using Avalonia.Controls;
using Avalonia.Interactivity;
using SingerUtils.ViewModels;
using System.IO;

namespace SingerUtils.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        async public void OnFindSingerPath(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFolderDialog();
            var path = await dialog.ShowAsync(this);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (Directory.Exists(path))
            {
                ((MainWindowViewModel)DataContext).SingerPath = path;
            }
        }
    }
}
