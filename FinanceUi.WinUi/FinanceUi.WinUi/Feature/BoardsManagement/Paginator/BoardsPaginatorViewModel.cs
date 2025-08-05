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

            MoveNextCommand = new AsyncRelayCommand(OnMoveNextCommandExecuted, CanMoveNextCommandExecute);
            MoveBackCommand = new AsyncRelayCommand(OnMoveBackCommandExecuted, CanMoveBackCommandExecute);
        }

        public int CurrentPage => _model.GetAllBoardsDto.PaginationParams.Page;

        public int TotalPagesCount => _model.TotalPagesCount;

        public IAsyncRelayCommand MoveNextCommand { get; private set; }
        private async Task OnMoveNextCommandExecuted()
        {
            if ((CurrentPage + 1) > TotalPagesCount)
                return;

            _model.GetAllBoardsDto.PaginationParams.Page++;
            await _model.RestoreAsync();
        }
        private bool CanMoveNextCommandExecute() => (CurrentPage + 1) <= TotalPagesCount;

        public IAsyncRelayCommand MoveBackCommand { get; private set; }
        private async Task OnMoveBackCommandExecuted()
        {
            if (CurrentPage <= 1)
                return;

            _model.GetAllBoardsDto.PaginationParams.Page--;
            await _model.RestoreAsync();
        }
        private bool CanMoveBackCommandExecute() => CurrentPage > 1;

        private void _model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalPagesCount));
            OnPropertyChanged(nameof(CurrentPage));

            MoveNextCommand.NotifyCanExecuteChanged();
            MoveBackCommand.NotifyCanExecuteChanged();
        }
    }
}
