using CommunityToolkit.Mvvm.Input;
using FinanceUi.Application.Services;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;
using FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;
using FinanceUi.WinUi.Feature.BoardsManagement.Paginator;
using FinanceUi.WinUi.Feature.BoardsManagement.SearchControl;
using FinanceUi.WinUi.Framework;
using FinanceUi.WinUi.State;
using System.Threading.Tasks;

namespace FinanceUi.WinUi.Feature.BoardsManagement;

public class BoardsManagementPageViewModel : DisposableObservableObject
{
	private readonly BoardsManagementModel _model;

	public BoardsManagementPageViewModel(BoardsManagementModel model, CurrentBoardStateService currentBoardStateService)
	{
		_model = model;
		_model.PropertyChanged += _model_PropertyChanged; ;

		_boardSearchViewModel = new BoardsSearchViewModel(model);
		_boardsListViewModel = new BoardsListViewModel(model, currentBoardStateService);
		_boardsPaginatorViewModel = new BoardsPaginatorViewModel(model);

		RefreshCommand = new AsyncRelayCommand(OnRefreshAsyncCommandExecute, CanRefreshAsyncCommandExecute);
	}

	private readonly BoardsSearchViewModel _boardSearchViewModel;
	public BoardsSearchViewModel BoardsSearchViewModel => _boardSearchViewModel;

	private readonly BoardsListViewModel _boardsListViewModel;
	public BoardsListViewModel BoardsListViewModel => _boardsListViewModel;

	private readonly BoardsPaginatorViewModel _boardsPaginatorViewModel;
	public BoardsPaginatorViewModel BoardsPaginatorViewModel => _boardsPaginatorViewModel;

	public AddOrEditBoardFormViewModel GetAddFormViewModel => new AddOrEditBoardFormViewModel(_model);

	public AddOrEditBoardFormViewModel AddBoardViewModel => new AddOrEditBoardFormViewModel(_model);

	public IAsyncRelayCommand RefreshCommand { get; set; }
	private async Task OnRefreshAsyncCommandExecute()
	{
		await _model.RestoreAsync();
	}
	private bool CanRefreshAsyncCommandExecute() => !_model.IsLoading;

	private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		OnPropertyChanged(e.PropertyName);
	}
}