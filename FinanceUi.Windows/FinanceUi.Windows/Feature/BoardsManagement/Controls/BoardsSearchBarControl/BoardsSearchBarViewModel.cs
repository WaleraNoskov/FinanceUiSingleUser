using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Windows.Feature.Boards;
using FinanceUi.Windows.Feature.Shared;

namespace FinanceUi.Windows.Feature.BoardsManagement.Controls.BoardsSearchBar;

internal class BoardsSearchBarViewModel : ObservableObject
{
	private readonly BoardManagementModel _model;

	public BoardsSearchBarViewModel(BoardManagementModel model)
	{
		_model = model;
		RestoreCommand = new AsyncRelayCommand(OnRestoreCommandExecuted, CanRestoreCommandExecuted);
	}

	public GetAllBoardsDto GetAllBoardsDto => _model.GetAllBoardsDto;
	public bool IsLoading => _model.IsLoading;

	public AsyncRelayCommand RestoreCommand { get; set; }
	private async Task OnRestoreCommandExecuted()
	{
		await _model.RestoreAsync();
	}
	private bool CanRestoreCommandExecuted() => !_model.IsLoading;
}

