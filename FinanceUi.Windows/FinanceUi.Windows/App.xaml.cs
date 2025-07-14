using FinanceUi.Application;
using FinanceUi.Infrastructure;
using FinanceUi.Windows.Feature.BoardsManagement.Pages.BoardsManagementPage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FinanceUi.Windows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Microsoft.UI.Xaml.Application
    {
        private Window? _window;
        private IHost _host;
        private IServiceProvider _services => _host.Services;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();

            var builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configBuilder) =>
                {
                    configBuilder
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    IConfiguration config = context.Configuration;

                    services.AddSingleton(config);
                    services.AddAppSqlite(config);

                    services.AddInfrastructureServices();
                    services.AddApplicationServices();
                    services.AddUiServices();

                    services.AddSingleton<MainWindow>();
                });

            IHost host = builder.Build();
            //host.Run();

            _host = host;

        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var page = _services.GetRequiredService<BoardsManagementPage>();
            page.DataContext = _services.GetRequiredService<BoardsManagementViewModel>(); 

            _window = _host.Services.GetRequiredService<MainWindow>();
            _window.Content = page;
            _window.Activate();
        }
    }
}
