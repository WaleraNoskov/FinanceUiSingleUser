using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Dtos.Board;
using FinanceUi.WinUi.Framework;

namespace FinanceUi.WinUi.Feature.BoardsManagement.Paginator
{
	public class BoardsPaginatorViewModel : DisposableObservableObject
	{
		private BoardsManagementModel _model;

		public BoardsPaginatorViewModel(BoardsManagementModel model)
		{
			_model = model;
            _model.PropertyChanged += _model_PropertyChanged;

			RefreshCommand = new AsyncRelayCommand(OnRefreshCommandExecute, CanRefreshCommandExecuted);
			Pages = new ObservableCollection<int>();
		}

        public ObservableCollection<int> Pages { get; set; }

		public GetAllBoardsDto GetGetAllBoardsDto => _model.GetAllBoardsDto;

		public int TotalBoardsCount => _model.TotalBoardsCount;

		public IAsyncRelayCommand RefreshCommand { get; set; }
		private async Task OnRefreshCommandExecute()
		{
			await _model.RestoreAsync();
		}
		private bool CanRefreshCommandExecuted() => !_model.IsLoading;

        private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_model.TotalBoardsCount))
			{
				Pages.Clear();
				for (var i = 1; i <= _model.TotalBoardsCount; i++)
					Pages.Add(i);
			}
        }
    }
}
