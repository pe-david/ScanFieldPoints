using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using EventStore.ClientAPI;
using ReactiveDomain.Bus;
using ReactiveDomain.Domain;
using ReactiveDomain.EventStore;
using ReactiveUI;
using ScanFieldPoints;
using Splat;

namespace ScanFieldPoints
{
    public class Bootstrap
    {
        private static string _assemblyName;
        private static EventStoreLoader _es;
        private static GetEventStoreRepository _esRepository;
        private static IEventStoreConnection _esConnection;

        public Bootstrap()
        {
            var fullName = Assembly.GetExecutingAssembly().FullName;
            _assemblyName = fullName.Split(',')[0];
        }

        private static IGeneralBus Bus { get; set; }

        public static void Load()
        {
            Debug.WriteLine($"{_assemblyName} has been loaded.");
        }

        public static void Configure(IGeneralBus bus)
        {
            //_es = new EventStoreLoader();
            //_es.SetupEventStore(new DirectoryInfo(@"C:\Users\rosed18169\source\EventStore-OSS-Win-v3.9.4"));
            //_esConnection = _es.Connection;
            //_esRepository = new GetEventStoreRepository(_es.Connection);

            Locator.CurrentMutable.RegisterConstant(_esRepository, typeof(IRepository));
            RegisterViews();
        }

        private static void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new ScanField(), typeof(IViewFor<ScanFieldVM>));
            Locator.CurrentMutable.Register(() => new ScanPoint(), typeof(IViewFor<PointVM>));
        }

        public void Run(StartupEventArgs args)
        {
            Bus = new CommandBus(
                "Main Bus",
                false,
                TimeSpan.FromMinutes(1), // TODO: Eliminate these timeouts and add the necessary timeouts to commands on a per-command basis.
                TimeSpan.FromMinutes(1));

            Configure(Bus);

            //var mainWindow = new MainWindow()
            //{
            //    ViewModel = new MainWindowViewModel(_bus, _accountRm, NewAccountId)
            //};

            //mainWindow.Show();
        }
    }
}
