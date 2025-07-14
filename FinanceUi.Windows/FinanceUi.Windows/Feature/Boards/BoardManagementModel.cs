using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Entities;
using FinanceUi.Core.Services;
using FinanceUi.Windows.Feature.Shared;

namespace FinanceUi.Windows.Feature.Boards;

internal class BoardManagementModel : ObservableObject
{
	private readonly IBoardService _boardService;

	private ObservableCollection<Board> _boards;
	public ReadOnlyObservableCollection<Board> Boards;

	private readonly GetAllBoardsDto _getAllDto;
	public GetAllBoardsDto GetAllBoardsDto => _getAllDto;

	public BoardManagementModel(IBoardService boardService)
	{
		_boardService = boardService;

		_boards = new ObservableCollection<Board>();
		Boards = new ReadOnlyObservableCollection<Board>(_boards);

		_getAllDto = new GetAllBoardsDto();
	}

	public async Task RestoreAsync()
	{
		
	}
}