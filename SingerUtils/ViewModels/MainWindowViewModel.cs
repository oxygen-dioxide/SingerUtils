using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace SingerUtils.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        [Reactive] public string SingerPath { get; set; }
        
        public void SingersViewModel()
        {
            this.SingerPath = "";
        }
    }
}
