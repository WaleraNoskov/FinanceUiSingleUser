using CommunityToolkit.Mvvm.Input;
using FinanceUi.Application.Services;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;
using FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;
using FinanceUi.WinUi.Feature.BoardsManagement.SearchControl;
using FinanceUi.WinUi.Feature.Shared;
using System.Threading.Tasks;

namespace FinanceUi.WinUi.Feature.BoardsManagement;

public class BoardsManagementPageViewModel : DisposableObservableObject
{
    private readonly BoardsManagementModel _model;

    public BoardsManagementPageViewModel(BoardsManagementModel model)
    {
        _model = model;
        _boardSearchViewModel = new BoardsSearchViewModel(model);
        _boardsListViewModel = new BoardsListViewModel(model);
    }

    private readonly BoardsSearchViewModel _boardSearchViewModel;
    public BoardsSearchViewModel BoardsSearchViewModel => _boardSearchViewModel;

    private readonly BoardsListViewModel _boardsListViewModel;
    public BoardsListViewModel BoardsListViewModel => _boardsListViewModel;

    public AddOrEditBoardFormViewModel GetAddFormViewModel => new AddOrEditBoardFormViewModel(_model);

    public AddOrEditBoardFormViewModel AddBoardViewModel => new AddOrEditBoardFormViewModel(_model);
}