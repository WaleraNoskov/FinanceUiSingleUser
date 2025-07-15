using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FinanceUi.Application;
using FinanceUi.Core.Repositories;
using FinanceUi.Infrastructure;
using FinanceUi.WinUi.Feature.BoardsManagement;
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
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FinanceUi.WinUi
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	public partial class App : Microsoft.UI.Xaml.Application
	{
		private Window? _window;
		private IHost _host;
		private IServiceProvider _serviceProvider => _host.Services;

		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			InitializeComponent();

			HostApplicationBuilder builder = Host.CreateApplicationBuilder();

			var config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false)
				.Build();
			builder.Configuration.AddConfiguration(config);

			var rawConnectionString = config["ConnectionStrings:Default"];
			var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			var resolvedConnectionString = rawConnectionString.Replace("%LOCALAPPDATA%", localAppData);

			builder.Services.AddSqlite<AppDbContext>(resolvedConnectionString, opt => { });
			builder.Services.AddInfrastructureServices();
			builder.Services.AddApplicationServices();
			builder.Services.AddWinUiServices();

			IHost host = builder.Build();
			_host = host;
		}

		/// <summary>
		/// Invoked when the application is launched.
		/// </summary>
		/// <param name="args">Details about the launch request and process.</param>
		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			var page = _host.Services.GetRequiredService<BoardsManagementPageView>();
			page.DataContext = _host.Services.GetRequiredService<BoardsManagementPageViewModel>();

			_window = _host.Services.GetRequiredService<MainWindow>();
			_window.Content = page;
			_window.Activate();
		}
	}
}
