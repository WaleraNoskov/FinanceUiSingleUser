using CommunityToolkit.Mvvm.Input;
using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Planning;
using FinanceUi.WinUi.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.WinUi.Feature.Planning.ColumnsManagement;

internal class ColumnsWidgetViewModel : DisposableObservableObject
{
    private readonly PlanningModel _model;

    public ColumnsWidgetViewModel(PlanningModel model)
    {
        _model = model;
        _model.PropertyChanged += OnPropertyChanged;

        RestoreAsyncCommand = new AsyncRelayCommand(OnRestoreAsyncCommandExecuted, CanRestoreAsyncCommandExecute);
    }

    public GetColumnsDto Dto => _model.Dto;
    public ReadOnlyObservableCollection<Column> Columns => _model.Columns;
    public bool IsLoading => _model.IsLoading;

    public IAsyncRelayCommand RestoreAsyncCommand { get; private set; }
    private async Task OnRestoreAsyncCommandExecuted()
    {
        await _model.RestoreAsync();
    }
    private bool CanRestoreAsyncCommandExecute() => !_model.IsLoading;

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.PropertyName))
            OnPropertyChanged(e.PropertyName);
    }
}
