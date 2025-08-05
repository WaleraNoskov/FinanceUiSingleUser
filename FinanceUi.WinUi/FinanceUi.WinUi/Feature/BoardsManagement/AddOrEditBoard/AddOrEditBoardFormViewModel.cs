using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Framework;
using Microsoft.Windows.AppNotifications;
using Microsoft.Windows.AppNotifications.Builder;

namespace FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;

public class AddOrEditBoardFormViewModel : DisposableObservableObject
{
	private readonly BoardsManagementModel _model;

	public AddOrEditBoardFormViewModel(BoardsManagementModel model)
	{
		_model = model;

		Title = string.Empty;
		SaveCommand = new AsyncRelayCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
	}

	private bool _isEditModel;
	public bool IsEditMode
	{
		get => _isEditModel;
		set => SetField(ref _isEditModel, value);
	}

	private string _title;
	public string Title
	{
		get => _title;
		set
		{
			SetField(ref _title, value);
			OnPropertyChanged(nameof(IsValid));
		}
	}

	private BriefBoardDto? _boardDto;
	public BriefBoardDto? CurrentBoardDto
	{
		get => _boardDto;
		set
		{
			SetField(ref _boardDto, value);
			Title = value?.Title ?? string.Empty;
		}
	}

	public bool IsValid => !string.IsNullOrWhiteSpace(Title) && Title != (CurrentBoardDto?.Title ?? string.Empty);

	public IAsyncRelayCommand SaveCommand { get; set; }
	private async Task OnSaveCommandExecuted()
	{
		if (!IsValid)
			return;

		var dto = CurrentBoardDto ?? new BriefBoardDto();
		dto.Title = Title;

		AppNotification notification;

		if (!IsEditMode)
		{
			await _model.CreateBoard(dto);

			notification = new AppNotificationBuilder()
				.AddText($"Доска \"{dto.Title}\" добавлена")
				.AddText("Ищите в разделе \"Доски\"")
				.BuildNotification();
		}
		else
		{
			await _model.UpdateBoard(dto);

			notification = new AppNotificationBuilder()
				.AddText($"Доска \"{dto.Title}\" обновлена")
				.AddText("Ищите в разделе \"Доски\"")
				.BuildNotification();
		}

		AppNotificationManager.Default.Show(notification);
	}
	private bool CanSaveCommandExecute() => IsValid;
}

