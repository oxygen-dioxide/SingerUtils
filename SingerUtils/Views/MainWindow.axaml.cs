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
using System.IO.Compression;

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
            var vm = (MainWindowViewModel)DataContext;
            var dialog = new OpenFolderDialog();
            dialog.Directory = vm.SingerPath;
            var path = await dialog.ShowAsync(this);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (Directory.Exists(path))
            {
                vm.SingerPath = path;
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

        async public void OnRunPack(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            //write voicebank.gitignore
            string IgnoreFileTypes = vm.IgnoreFileTypes;
            string gitignorePath = System.IO.Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            File.WriteAllText(gitignorePath, vm.IgnoreFileTypes);
            //ask where to save the .zip file
            var dialog = new SaveFileDialog();
            dialog.InitialFileName = System.IO.Path.GetFileName(vm.SingerPath)+".zip";
            dialog.Filters = new List<FileDialogFilter>
            {
                new FileDialogFilter
                {
                    Extensions=new List<string>{".zip" },
                    Name="Zip Archive (.zip)"
                }
            };
            var zipPath = await dialog.ShowAsync(this);
            if (string.IsNullOrEmpty(zipPath))
            {
                return;
            }
            //get all the files to pack
            var ignore = new Ignore.Ignore();
            ignore.Add(IgnoreFileTypes.Split("\n"));
            string singerPath = vm.SingerPath;
            List<string> fileList = Directory.EnumerateFiles(singerPath, "*.*", SearchOption.AllDirectories).ToList();
            List<string> packList = fileList.FindAll(x => !(ignore.IsIgnored(System.IO.Path.GetRelativePath(singerPath, x))));

            //pack
            int index = 0;
            int fileCount = packList.Count();
            vm.PackProgressMaximum = fileCount;
            using ZipArchive zipFile = new ZipArchive(File.Create(zipPath),ZipArchiveMode.Create);
            {
                await Task.Run(() =>
                {
                    foreach (var absFilePath in packList)
                    {
                        index++;
                        string reFilePath = System.IO.Path.GetRelativePath(singerPath, absFilePath);
                        vm.PackProgressText = string.Format(
                            "[{0}/{1}] {2}",
                            index,
                            fileCount,
                            reFilePath);
                        vm.PackProgressValue = index;
                        zipFile.CreateEntryFromFile(absFilePath,reFilePath);
                    }
                });
            }
            vm.PackProgressText = "Pack Succeed";
        }
    }
}
