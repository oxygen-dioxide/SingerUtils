using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using FluentAvalonia.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SingerUtils.Core;

namespace SingerUtils.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive] public string SingerPath { get; set; }
        [Reactive] public string IgnoreFileTypes { get; set; }
        [Reactive] public string CleanUpProgressText { get; set; }
        [Reactive] public int CleanUpProgressMaximum { get; set; }
        [Reactive] public int CleanUpProgressValue { get; set; }
        [Reactive] public string PackProgressText { get; set; }
        [Reactive] public int PackProgressMaximum { get; set; }
        [Reactive] public int PackProgressValue { get; set; }
        public string AppVersion => $"SingerUtils v{System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version}";

        public MainWindowViewModel()
        {
            this.SingerPath = "";
            this.IgnoreFileTypes = "";
            this.CleanUpProgressText = " ";
            this.CleanUpProgressMaximum = 1;
            this.CleanUpProgressValue = 0;
            this.PackProgressText = " ";
            this.PackProgressMaximum = 1;
            this.PackProgressValue = 0;

            //Load voicebank.gitignore file
            string gitignorePath = Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            if (File.Exists(gitignorePath))
            {
                this.IgnoreFileTypes = File.ReadAllText(gitignorePath);
            }

            var args = Environment.GetCommandLineArgs();
            if (args.Count() > 1 && File.Exists(args[1]))
            {
                if (File.Exists(args[1]))
                {
                    //Load SingerPath from utauplugin temp file
                    foreach (var line in File.ReadLines(args[1]))
                    {
                        if (line.StartsWith("VoiceDir="))
                        {
                            this.SingerPath = line.Split("=")[1];
                            break;
                        }
                    }
                }
                else if (Directory.Exists(args[1]))
                {
                    this.SingerPath = args[1];
                }
            }
        }

        public List<string> GetFilesToCleanup()
        {
            var vm = this;
            //get all the files to be deleted
            var ignore = new Ignore.Ignore();
            ignore.Add(IgnoreFileTypes.Split("\n"));
            string singerPath = vm.SingerPath;
            List<string> fileList = Directory.EnumerateFiles(singerPath, "*.*", SearchOption.AllDirectories).ToList();
            List<string> deleteList = fileList.FindAll(x => ignore.IsIgnored(System.IO.Path.GetRelativePath(singerPath, x)));
            return deleteList;
        }

        async public void Cleanup(List<string> deleteList)
        {
            var vm = this;
            string singerPath = vm.SingerPath;
            //write voicebank.gitignore
            string IgnoreFileTypes = vm.IgnoreFileTypes;
            string gitignorePath = System.IO.Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            File.WriteAllText(gitignorePath, vm.IgnoreFileTypes);

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

        public List<string> GetFilesToPack()
        {
            var vm = this;
            //get all the files to pack
            var ignore = new Ignore.Ignore();
            ignore.Add(IgnoreFileTypes.Split("\n"));
            string singerPath = vm.SingerPath;
            List<string> fileList = Directory.EnumerateFiles(singerPath, "*.*", SearchOption.AllDirectories).ToList();
            List<string> packList = fileList.FindAll(x => !(ignore.IsIgnored(System.IO.Path.GetRelativePath(singerPath, x))));
            return packList;
        }

        async public void Pack(string zipPath,List<string> packList)
        {
            var vm = this;
            string singerPath = vm.SingerPath;
            //write voicebank.gitignore
            string IgnoreFileTypes = vm.IgnoreFileTypes;
            string gitignorePath = System.IO.Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            File.WriteAllText(gitignorePath, vm.IgnoreFileTypes);

            //pack
            int index = 0;
            int fileCount = packList.Count();
            vm.PackProgressMaximum = fileCount;
            using ZipArchive zipFile = new ZipArchive(File.Create(zipPath), ZipArchiveMode.Create);
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
                        zipFile.CreateEntryFromFile(absFilePath, reFilePath);
                    }
                });
            }
            vm.PackProgressText = "Pack Succeed";

        }
    }
}
