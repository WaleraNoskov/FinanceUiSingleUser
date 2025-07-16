using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;

public class BoardsListViewModel : DisposableObservableObject
{
    private readonly BoardsManagementModel _model;

    public BoardsListViewModel(BoardsManagementModel model)
    {
        _model = model;
    }

    public ReadOnlyObservableCollection<BoardDto> Boards => _model.Boards;

    public AddOrEditBoardFormViewModel GetEditBoardViewModel => new AddOrEditBoardFormViewModel(_model)
    {
        IsEditMode = true
    };
}

