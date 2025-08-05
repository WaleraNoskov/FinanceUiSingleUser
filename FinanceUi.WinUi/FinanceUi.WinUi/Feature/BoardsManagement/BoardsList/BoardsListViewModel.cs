using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;
using FinanceUi.WinUi.Feature.BoardsManagement.DeleteBoardDialog;
using FinanceUi.WinUi.Framework;
using FinanceUi.WinUi.State;

namespace FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;

public class BoardsListViewModel : DisposableObservableObject
{
    private readonly BoardsManagementModel _model;
    private readonly CurrentBoardStateService _currentBoardStateService;

    public BoardsListViewModel(BoardsManagementModel model, CurrentBoardStateService currentBoardStateService)
    {
        _model = model;
        _model.PropertyChanged += _model_PropertyChanged;

        _currentBoardStateService = currentBoardStateService;
        _currentBoardStateService.PropertyChanged += _currentBoardStateService_PropertyChanged;

        RefreshCommand = new AsyncRelayCommand(OnRefreshCommandExecute, CanRehreshCommandExecute);
        SetCurrentBoardIdCommand = new RelayCommand<Guid>(OnSetCurrentBoardIdCommandExecuted, CanSetCurrentBoardIdCommandExecute);
    }

    public ReadOnlyObservableCollection<BoardDto> Boards => _model.Boards;

    public bool IsLoading => _model.IsLoading;

    public Guid CurrentBoardId => _currentBoardStateService.CurrentBoardId;

    public AddOrEditBoardFormViewModel GetEditBoardViewModel => new AddOrEditBoardFormViewModel(_model)
    {
        IsEditMode = true
    };

    public DeleteBoardDialogViewModel GetDeleteBoardDialogViewModel => new DeleteBoardDialogViewModel(_model);

    public IAsyncRelayCommand RefreshCommand { get; private set; }
    private async Task OnRefreshCommandExecute()
    {
        await _model.RestoreAsync();
    }
    private bool CanRehreshCommandExecute() => !_model.IsLoading;

    public IRelayCommand<Guid> SetCurrentBoardIdCommand { get; private set; }
    private void OnSetCurrentBoardIdCommandExecuted(Guid id)
    {
        _currentBoardStateService.CurrentBoardId = id;

        SetCurrentBoardIdCommand.NotifyCanExecuteChanged();
    }
    private bool CanSetCurrentBoardIdCommandExecute(Guid id) => id != _currentBoardStateService.CurrentBoardId;

    private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName ?? string.Empty);
    private void _currentBoardStateService_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e) => OnPropertyChanged(e.PropertyName ?? string.Empty);
}

