using FinanceUi.Application.Services;
using FinanceUi.Core.Contracts;
using FinanceUi.Core.Dtos.Income;
using FinanceUi.Core.Dtos.Planning;
using FinanceUi.Infrastructure.Repositories;
using FinanceUi.WinUi.Feature.Shared;
using FinanceUi.WinUi.Framework;
using FinanceUi.WinUi.State;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceUi.WinUi.Feature.Planning;

internal class PlanningModel : DisposableObservableObject
{
    private readonly PlanningService _planningService;
    private readonly IncomeRepository _incomeRepository;
    private readonly CurrentBoardStateService _currentBoardStateService;

    public PlanningModel(PlanningService planningService, IncomeRepository incomeRepository, CurrentBoardStateService currentBoardStateService)
    {
        _planningService = planningService;
        _incomeRepository = incomeRepository;
        _currentBoardStateService = currentBoardStateService;

        _columns = [];
        _columnsDto = new GetColumnsDto()
        {
            BoardId = _currentBoardStateService.CurrentBoardId,
            Period = new Period(DateOnly.FromDateTime(DateTime.Now).StartOfMonth(), DateOnly.FromDateTime(DateTime.Now).EndOfMonth()),
        };
    }

    private readonly ObservableCollection<Column> _columns;
    public ReadOnlyObservableCollection<Column> Columns => new ReadOnlyObservableCollection<Column>(_columns);

    private GetColumnsDto _columnsDto;
    public GetColumnsDto Dto
    {
        get => _columnsDto;
        private set => SetField(ref _columnsDto, value);
    }

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        private set => SetField(ref _isLoading, value);
    }

    public async Task RestoreAsync()
    {
        _isLoading = true;
        var columns = await _planningService.GetColumns(Dto);

        _columns.Clear();
        foreach (var column in columns)
            _columns.Add(column);

        _isLoading = false;
        OnPropertyChanged(nameof(Columns));
    }

    public async Task CreateIncome(BriefIncomeDto dto)
    {
        _isLoading = true;

        var id = await _incomeRepository.CreateAsync(dto);

        var existingColumn = _columns.FirstOrDefault(c => dto.Date.IsInPeriod(c.Period));
        if (existingColumn != null)
            _columns.Remove(existingColumn);

        var updatedColumn = await _planningService.GetColumn(new GetColumnDto(_currentBoardStateService.CurrentBoardId, new Period(dto.Date, dto.Date)));

        int indexToInsert = 0;
        if(_columns.Count > 0)
        {
            var previousColumn = _columns
                .OrderBy(c => c.Period.StartDate)
                .Where(c => c.Period.StartDate < updatedColumn.Period.StartDate)
                .LastOrDefault();

            if(previousColumn != null)
                indexToInsert = _columns.IndexOf(previousColumn);
        }

        _columns.Insert(indexToInsert, updatedColumn);

        _isLoading = false;
        OnPropertyChanged(nameof(Columns));
    }

    public async Task EditIncome(BriefIncomeDto dto)
    {
        await _incomeRepository.UpdateAsync(dto);
    }

    public async Task DeleteIncome(Guid id)
    {
        await _incomeRepository.DeleteAsync(id);
    }
}
