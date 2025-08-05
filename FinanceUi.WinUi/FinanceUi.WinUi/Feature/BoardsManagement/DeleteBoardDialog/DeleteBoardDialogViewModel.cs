using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Framework;

namespace FinanceUi.WinUi.Feature.BoardsManagement.DeleteBoardDialog
{
	public class DeleteBoardDialogViewModel : DisposableObservableObject
	{
		private readonly BoardsManagementModel _model;

		public DeleteBoardDialogViewModel(BoardsManagementModel model)
		{
			_model = model;

			NameToConfirm = string.Empty;
			DeleteCommand = new AsyncRelayCommand(OnDeleteCommandExecuted, CanDeleteCommandExecuted);
		}

		private string _nameToConfirm;
		public string NameToConfirm
		{
			get => _nameToConfirm;
			set
			{
				SetField(ref _nameToConfirm, value);
				RaiseIsValidChanged();
			}
		}

		private BriefBoardDto _boardDto;
		public BriefBoardDto BoardDto
		{
			get => _boardDto;
			set => SetField(ref _boardDto, value);
		}

		public bool IsValid => BoardDto is not null && NameToConfirm == BoardDto.Title;

		public void RaiseIsValidChanged() => OnPropertyChanged(nameof(IsValid));

		public IAsyncRelayCommand DeleteCommand { get; set; }
		private async Task OnDeleteCommandExecuted()
		{
			if (!IsValid || BoardDto?.Id is null)
				return;

			await _model.DeleteBoardAsync(BoardDto.Id.Value);
		}
		private bool CanDeleteCommandExecuted() => IsValid;
	}
}
