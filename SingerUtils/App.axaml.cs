using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;
using SingerUtils.ViewModels;
using SingerUtils.Views;
using Serilog;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace SingerUtils
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            Log.Information("Initializing application.");
            AvaloniaXamlLoader.Load(this);
            InitializeCulture();
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
        public void InitializeCulture()
        {
            Log.Information("Initializing culture.");
            var language = CultureInfo.InstalledUICulture.Name;
            if (!string.IsNullOrEmpty(Core.Util.Preferences.Default.Language))
            {
                language = Core.Util.Preferences.Default.Language;
            }
            SetLanguage(language);

            // Force using InvariantCulture to prevent issues caused by culture dependent string conversion, especially for floating point numbers.
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Log.Information("Initialized culture.");
        }

        public static void SetLanguage(string language)
        {
            var dictionaryList = Current.Resources.MergedDictionaries
                .Select(res => (ResourceInclude)res)
                .ToList();
            var resDictName = string.Format(@"Strings.{0}.axaml", language);
            var resDict = dictionaryList
                .FirstOrDefault(d => d.Source!.OriginalString.Contains(resDictName));
            if (resDict == null)
            {
                resDict = dictionaryList.FirstOrDefault(d => d.Source!.OriginalString.Contains("Strings.axaml"));
            }
            if (resDict != null)
            {
                Current.Resources.MergedDictionaries.Remove(resDict);
                Current.Resources.MergedDictionaries.Add(resDict);
            }
        }
    }
}
