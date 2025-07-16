using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Feature.Shared;

namespace FinanceUi.WinUi.Feature.BoardsManagement.Paginator
{
	public class BoardsPaginatorViewModel : DisposableObservableObject
	{
		private BoardsManagementModel _model;

		public BoardsPaginatorViewModel(BoardsManagementModel model)
		{
			_model = model;

			RefreshCommand = new AsyncRelayCommand(OnRefreshCommandExecute, CanRefreshCommandExecuted);
		}

		public GetAllBoardsDto GetGetAllBoardsDto => _model.GetAllBoardsDto;

		public int TotalBoardsCount => _model.TotalBoardsCount;

		public IAsyncRelayCommand RefreshCommand { get; set; }
		private async Task OnRefreshCommandExecute()
		{
			await _model.RestoreAsync();
		}
		private bool CanRefreshCommandExecuted() => !_model.IsLoading;
	}
}
