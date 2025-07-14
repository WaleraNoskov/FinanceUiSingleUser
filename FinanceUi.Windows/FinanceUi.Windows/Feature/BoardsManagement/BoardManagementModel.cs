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

public class BoardManagementModel : ObservableObject
{
	private readonly IBoardService _boardService;

	private ObservableCollection<BoardDto> _boards;
	public ReadOnlyObservableCollection<BoardDto> Boards => new ReadOnlyObservableCollection<BoardDto>(_boards);

	private readonly GetAllBoardsDto _getAllDto;
	public GetAllBoardsDto GetAllBoardsDto => _getAllDto;

	private bool _isLoading;
	public bool IsLoading
	{
		get => _isLoading;
		private set => SetField(ref _isLoading, value);
	}

	public BoardManagementModel(IBoardService boardService)
	{
		_boardService = boardService;

		_boards = new ObservableCollection<BoardDto>();

		_getAllDto = new GetAllBoardsDto();
	}

	public async Task RestoreAsync()
	{
		IsLoading = true;

		try
		{
			var result = await _boardService.GetAllAsync(_getAllDto);
			_boards.Clear();
			foreach (var board in result.Items)
				_boards.Add(board);
		}
		catch (Exception ex)
		{
		}
		IsLoading = false;
	}

	public async Task CreateBoard(BriefBoardDto board)
	{
		IsLoading = true;

		await _boardService.CreateBoard(board);
		await RestoreAsync();
	}

	public async Task UpdateBoard(BriefBoardDto board)
	{
		IsLoading = true;

		await _boardService.UpdateBoard(board);
		await RestoreAsync();
	}

	public async Task DeleteBoardAsync(Guid id)
	{
		IsLoading = true;

		await _boardService.DeleteBoard(id);
		await RestoreAsync();
	}
}