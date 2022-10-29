using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using FluentAvalonia.UI.Controls;
using SingerUtils.Core;
using SingerUtils.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ignore;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Compression;
using System;

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

        async public void OnPreviewCleanup(object sender, RoutedEventArgs e)
        {
            //TODO
            var vm = (MainWindowViewModel)DataContext;
            List<string> deleteList = vm.GetFilesToCleanup();
            //Show Preview
            var previewContentDialog = new ContentDialog()
            {
                Title = "These files will be deleted",
                Content = string.Join("\n", deleteList.Select(x => System.IO.Path.GetRelativePath(vm.SingerPath, x))),
                PrimaryButtonText = "Cleanup",
                CloseButtonText = "Cancel"
            };
            var result = await previewContentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                vm.Cleanup(deleteList);
            }
        }

        async public void OnRunCleanup(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            List<string> deleteList = vm.GetFilesToCleanup();
            vm.Cleanup(deleteList);
        }

        async public void OnPreviewPack(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            var packList = vm.GetFilesToPack();
            //Show Preview
            var previewContentDialog = new ContentDialog()
            {
                Title = "These files will be packed",
                Content = string.Join("\n",packList.Select(x=>System.IO.Path.GetRelativePath(vm.SingerPath,x))),
                PrimaryButtonText = "Pack",
                CloseButtonText = "Cancel"
            };
            var result = await previewContentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                //ask where to save the .zip file
                var dialog = new SaveFileDialog();
                dialog.InitialFileName = System.IO.Path.GetFileName(vm.SingerPath) + ".zip";
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
                //Pack
                vm.Pack(zipPath, packList);
            }
        }

        async public void OnRunPack(object sender, RoutedEventArgs e)
        {
            var vm = (MainWindowViewModel)DataContext;
            //ask where to save the .zip file
            var dialog = new SaveFileDialog();
            dialog.InitialFileName = System.IO.Path.GetFileName(vm.SingerPath) + ".zip";
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
            //Pack
            var packList = vm.GetFilesToPack();
            vm.Pack(zipPath,packList);
        }

        public void OnVisitGithub(object sender, RoutedEventArgs e)
        {
            try
            {
                OS.OpenWeb("https://github.com/oxygen-dioxide/SingerUtils");
            }
            catch
            {

            }
        }
    }
}
