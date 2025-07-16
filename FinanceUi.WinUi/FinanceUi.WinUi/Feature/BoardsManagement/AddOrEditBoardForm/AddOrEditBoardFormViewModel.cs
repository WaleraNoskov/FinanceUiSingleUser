using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.Shared;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;

public class AddOrEditBoardFormViewModel : DisposableObservableObject
{
    private readonly BoardsManagementModel _model;

    private BriefBoardDto _boardDto;
    public BriefBoardDto BoardDto
    {
        get => _boardDto;
        private set => SetField(ref _boardDto, value);
    }

    public AddOrEditBoardFormViewModel(BoardsManagementModel model)
    {
        _model = model;

        _boardDto = new();
        CreateCommand = new AsyncRelayCommand(OnCreateCommandExecuted, CanCreateCommandExecute);
    }

    public IAsyncRelayCommand CreateCommand { get; set; }
    private async Task OnCreateCommandExecuted()
    {
        await _model.CreateBoard(BoardDto);

		AppNotification notification = new AppNotificationBuilder()
	        .AddText($"Доска \"{BoardDto.Title}\" добавлена")
	        .AddText("Ищите в разделе \"Доски\"")
	        .BuildNotification();

		AppNotificationManager.Default.Show(notification);
	}
    private bool CanCreateCommandExecute() => !string.IsNullOrWhiteSpace(BoardDto.Title);
}

