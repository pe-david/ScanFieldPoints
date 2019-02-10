using System.Reflection;
using System.Windows;

namespace ScanFieldPoints
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Bootstrap _bootstrap;
        public string AssemblyName { get; set; }

        public App()
        {
            var fullName = Assembly.GetExecutingAssembly().FullName;
            //Log.Info($"{fullName} Loaded.");
            AssemblyName = fullName.Split(',')[0];
            _bootstrap = new Bootstrap();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _bootstrap.Run(e);
        }
    }
}
