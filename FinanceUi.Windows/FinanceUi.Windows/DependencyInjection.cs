using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Windows.Feature.Boards;
using FinanceUi.Windows.Feature.BoardsManagement.Controls.BoardsSearchBar;
using FinanceUi.Windows.Feature.BoardsManagement.Pages.BoardsManagementPage;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceUi.Windows;

internal static class DependencyInjection
{
	public static IServiceCollection AddUiServices(this IServiceCollection services)
	{
		services
			.AddScoped<BoardManagementModel>()
			.AddScoped<BoardsManagementViewModel>()
			.AddScoped<BoardsManagementPage>()
			.AddScoped<BoardsSearchBarViewModel>()
			.AddScoped<BoardsSearchBar>();

		return services;
	}
}

