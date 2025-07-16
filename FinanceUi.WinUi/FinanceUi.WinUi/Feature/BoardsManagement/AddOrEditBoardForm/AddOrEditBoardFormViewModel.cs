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
		set => SetField(ref _boardDto, value);
	}

	public AddOrEditBoardFormViewModel(BoardsManagementModel model)
	{
		_model = model;

		_boardDto = new();
		SaveCommand = new AsyncRelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
	}

	private bool _isEditModel;
	public bool IsEditMode
	{
		get => _isEditModel;
		set => SetField(ref _isEditModel, value);
	}

	public IAsyncRelayCommand SaveCommand { get; set; }
	private async Task OnSaveCommandExecuted()
	{
		AppNotification notification;

		if (!IsEditMode)
		{
			await _model.CreateBoard(BoardDto);

			notification = new AppNotificationBuilder()
				.AddText($"Доска \"{BoardDto.Title}\" добавлена")
				.AddText("Ищите в разделе \"Доски\"")
				.BuildNotification();
		}
		else
		{
			await _model.UpdateBoard(BoardDto);

			notification = new AppNotificationBuilder()
				.AddText($"Доска \"{BoardDto.Title}\" обновлена")
				.AddText("Ищите в разделе \"Доски\"")
				.BuildNotification();
		}

		AppNotificationManager.Default.Show(notification);
	}
	private bool CanSaveCommandExecute() => !string.IsNullOrWhiteSpace(BoardDto.Title);
}

