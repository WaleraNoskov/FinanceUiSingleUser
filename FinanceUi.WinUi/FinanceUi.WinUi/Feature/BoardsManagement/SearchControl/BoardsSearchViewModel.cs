using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement.SearchControl;

public class BoardsSearchViewModel : DisposableObservableObject
{
	private readonly BoardsManagementModel _model;

	public BoardsSearchViewModel(BoardsManagementModel model)
	{
		_model = model;

		SearchCommand = new AsyncRelayCommand(OnSearchCommandExecuted, CanSearchCommandExecute);
	}

	public GetAllBoardsDto GetAllBoardsDto => _model.GetAllBoardsDto;

	public AsyncRelayCommand SearchCommand { get; private set; }
	private async Task OnSearchCommandExecuted()
	{
		await _model.RestoreAsync();
	}
	private bool CanSearchCommandExecute() => !_model.IsLoading;
}

