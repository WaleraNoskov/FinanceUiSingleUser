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
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;

public class BoardsListViewModel : DisposableObservableObject
{
    private readonly BoardsManagementModel _model;

    public BoardsListViewModel(BoardsManagementModel model)
    {
        _model = model;
		_model.PropertyChanged += _model_PropertyChanged;

        RefreshCommand = new AsyncRelayCommand(OnRefreshCommandExecute, CanRehreshCommandExecute);
    }

	public ReadOnlyObservableCollection<BoardDto> Boards => _model.Boards;

    public bool IsLoading => _model.IsLoading;

    public AddOrEditBoardFormViewModel GetEditBoardViewModel => new AddOrEditBoardFormViewModel(_model)
    {
        IsEditMode = true
    };

    public DeleteBoardDialogViewModel GetDeleteBoardDialogViewModel => new DeleteBoardDialogViewModel(_model);

    public IAsyncRelayCommand RefreshCommand { get; set; }
    private async Task OnRefreshCommandExecute()
    {
        await _model.RestoreAsync();
    }
    private bool CanRehreshCommandExecute() => !_model.IsLoading;

	private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		OnPropertyChanged(e.PropertyName);
	}
}

