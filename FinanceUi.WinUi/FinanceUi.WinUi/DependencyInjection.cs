using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.WinUi.Feature.BoardsManagement;
using FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;
using FinanceUi.WinUi.Feature.BoardsManagement.SearchControl;
using FinanceUi.WinUi.State;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceUi.WinUi;

public static class DependencyInjection
{
	public static IServiceCollection AddWinUiServices(this IServiceCollection services)
	{
		services.AddSingleton<MainWindow>();

		//State
		services
			.AddSingleton<CurrentBoardStateService>();

		//Boards management
		services
			.AddScoped<BoardsManagementModel>()
			.AddScoped<BoardsManagementPageViewModel>()
			.AddScoped<BoardsManagementPageView>();

		services
			.AddScoped<BoardsSearchViewModel>()
			.AddScoped<BoardsSearchControl>();

		services
			.AddScoped<BoardsListViewModel>()
			.AddScoped<BoardsListControl>();

		return services;
	}
}

