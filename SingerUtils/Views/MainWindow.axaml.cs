using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using SingerUtils.Core;
using SingerUtils.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ignore;
using System.Threading.Tasks;
using System.Threading;

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

        async public void OnRunCleanup(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            //write voicebank.gitignore
            string IgnoreFileTypes = vm.IgnoreFileTypes;
            string gitignorePath = System.IO.Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            File.WriteAllText(gitignorePath, vm.IgnoreFileTypes);

            //get all the files to be deleted
            var ignore = new Ignore.Ignore();
            ignore.Add(IgnoreFileTypes.Split("\n"));
            string singerPath = vm.SingerPath;
            List<string> fileList = Directory.EnumerateFiles(singerPath, "*.*", SearchOption.AllDirectories).ToList();
            List<string> deleteList = fileList.FindAll(x => ignore.IsIgnored(System.IO.Path.GetRelativePath(singerPath, x)));
            
            //delete files
            int index = 0;
            int fileCount = deleteList.Count();
            vm.CleanUpProgressMaximum = fileCount;
            await Task.Run(() =>
            {
                foreach (string file in deleteList)
                {
                    index++;
                    vm.CleanUpProgressText = string.Format(
                        "[{0}/{1}] {2}",
                        index,
                        fileCount,
                        System.IO.Path.GetRelativePath(singerPath, file));
                    vm.CleanUpProgressValue = index;
                    File.Delete(file);
                }
            });
            vm.CleanUpProgressText = "Cleanup Succeed";
        }

        public void OnRunPack(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            //write voicebank.gitignore
            string IgnoreFileTypes = vm.IgnoreFileTypes;
            string gitignorePath = System.IO.Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            File.WriteAllText(gitignorePath, vm.IgnoreFileTypes);
        }
    }
}
