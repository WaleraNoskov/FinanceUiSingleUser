using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.Core.Services;
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement;

public class BoardsManagementModel : DisposableObservableObject
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
        private set => SetField(ref _isLoading, value); // от DisposableObservableObject
    }

    public BoardsManagementModel(IBoardService boardService)
    {
        _boardService = boardService;

        _boards = new ObservableCollection<BoardDto>();
        _getAllDto = new GetAllBoardsDto
        {
            Filter = string.Empty,
            PaginationParams = new Core.Contracts.PaginationParams()
            {
                PageSize = 10
            },
            SortingParams = new Core.Contracts.SortingParams()
            {
                PropertyName = "Title"
            }
        };
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
            Debug.WriteLine($"Failed to restore boards: {ex.Message}");
        }
        
        IsLoading = false;
    }

    public async Task CreateBoard(BriefBoardDto board)
    {
        await _boardService.CreateBoard(board);
        await RestoreAsync();
    }

    public async Task UpdateBoard(BriefBoardDto board)
    {
        await _boardService.UpdateBoard(board);
        await RestoreAsync();
    }

    public async Task DeleteBoardAsync(Guid id)
    {
        await _boardService.DeleteBoard(id);
        await RestoreAsync();
    }
}