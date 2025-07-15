using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement.BoardsList;

public class BoardsListViewModel : DisposableObservableObject
{
    private readonly BoardsManagementModel _model;

    public BoardsListViewModel(BoardsManagementModel model)
    {
        _model = model;
    }

    //public ReadOnlyObservableCollection<BoardDto> Boards => _model.Boards;
    public ReadOnlyObservableCollection<BoardDto> Boards => new ReadOnlyObservableCollection<BoardDto>(new ObservableCollection<BoardDto>(new List<BoardDto>()
    {
        new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        },
        new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        },
        new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        },
        new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        },
        new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        },
        new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000 },
            new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000 },
            new BoardDto() 
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000 },
            new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        },new BoardDto()
        {
            Title = "Домашняя",
            PaidTotalGoal = 277000,
            TotalGoal= 500000
        },
        new BoardDto()
        {
            Title = "Машина",
            PaidTotalGoal = 32800,
            TotalGoal = 40000
        }
    }));
}

