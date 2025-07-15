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
			IncomesCount = 23,
			PaymentsCount= 388,
			GoalsCount = 7
		},
		new BoardDto()
		{
			Title = "Машина",
			IncomesCount = 3,
			PaymentsCount = 12,
			GoalsCount = 2
		}
	}));
}

