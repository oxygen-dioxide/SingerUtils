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

        public MainWindowViewModel()
        {
            this.SingerPath = "";
            this.IgnoreFileTypes = "";
            this.CleanUpProgressText = " ";
            this.CleanUpProgressMaximum = 1;
            this.CleanUpProgressValue = 0;

            //Load voicebank.gitignore file
            string gitignorePath = Path.Combine(PathManager.Inst.RootPath, "voicebank.gitignore");
            if (File.Exists(gitignorePath))
            {
                this.IgnoreFileTypes = File.ReadAllText(gitignorePath);
            }

            //Load SingerPath from utauplugin temp file
            var args = Environment.GetCommandLineArgs();
            if (args.Count() >= 1 && File.Exists(args[1]))
            {
                foreach(var line in File.ReadLines(args[1]))
                {
                    if (line.StartsWith("VoiceDir="))
                    {
                        this.SingerPath = line.Split("=")[1];
                        break;
                    }
                }
            }
        }
    }
}
