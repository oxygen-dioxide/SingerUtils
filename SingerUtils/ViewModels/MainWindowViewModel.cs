using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            if (args.Count() >= 1 && File.Exists(args[1]))
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
    }
}
