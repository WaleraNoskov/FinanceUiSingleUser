using FinanceUi.Application.Services;
using FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;
using FinanceUi.WinUi.Feature.BoardsManagement.SearchControl;
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement;

public class BoardsManagementPageViewModel : DisposableObservableObject
{
	private readonly BoardsManagementModel _model;

	private readonly BoardsSearchViewModel _boardSearchViewModel;
	public BoardsSearchViewModel BoardsSearchViewModel => _boardSearchViewModel;

	private readonly BoardsListViewModel _boardsListViewModel;
	public BoardsListViewModel BoardsListViewModel => _boardsListViewModel;

	public BoardsManagementPageViewModel(BoardsManagementModel model, BoardsSearchViewModel searchViewModel, BoardsListViewModel listViewModel)
	{
		_model = model;
		_boardSearchViewModel = searchViewModel;
		_boardsListViewModel = listViewModel;
	}
}