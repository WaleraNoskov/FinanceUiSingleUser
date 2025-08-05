using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;
using FinanceUi.WinUi.Feature.BoardsManagement.DeleteBoardDialog;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;

public sealed partial class BoardsListControl : UserControl
{
	private BoardsListViewModel _viewModel;

	public BoardsListControl()
	{
		InitializeComponent();
	}

	private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
	{
		_viewModel = (args.NewValue as BoardsListViewModel)!;
	}

	private async void EditButton_Click(object sender, RoutedEventArgs e)
	{
		var guidIsParsed = Guid.TryParse((sender as MenuFlyoutItem)!.Tag.ToString(), out var guid);
		if (!guidIsParsed)
			return;

		var dto = _viewModel.Boards.First(b => b.Id == guid);
		var dataContext = _viewModel.GetEditBoardViewModel;
		dataContext.CurrentBoardDto = new BriefBoardDto
		{
			Id = dto.Id,
			Title = dto.Title
		};
		dataContext.IsEditMode = true;

		var dialog = new AddOrEditBoardDialogView()
		{
			XamlRoot = this.XamlRoot,
			DataContext = dataContext
		};

		var result = await dialog.ShowAsync();
		if (result == ContentDialogResult.Primary && _viewModel.RefreshCommand.CanExecute(null))
			await _viewModel.RefreshCommand.ExecuteAsync(null);
	}

	private async void DeleteButton_Click(object sender, RoutedEventArgs e)
	{
		var guidIsParsed = Guid.TryParse((sender as MenuFlyoutItem)!.Tag.ToString(), out var guid);
		if (!guidIsParsed)
			return;

		var dto = _viewModel.Boards.First(b => b.Id == guid);
		var dataContext = _viewModel.GetDeleteBoardDialogViewModel;
		dataContext.BoardDto = new BriefBoardDto
		{
			Id = dto.Id,
			Title = dto.Title
		};

		var dialog = new DeleteBoardDialogView()
		{
			XamlRoot = this.XamlRoot,
			DataContext = dataContext
		};


        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary && _viewModel.RefreshCommand.CanExecute(null))
            await _viewModel.RefreshCommand.ExecuteAsync(null);
    }
}

