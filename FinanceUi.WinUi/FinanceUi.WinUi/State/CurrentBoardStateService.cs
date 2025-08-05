using FinanceUi.WinUi.Framework;
using System;

namespace FinanceUi.WinUi.State;

public class CurrentBoardStateService : DisposableObservableObject
{
    public CurrentBoardStateService()
    {
        _currentBoardId = Guid.NewGuid();
    }

    private Guid _currentBoardId;
    public Guid CurrentBoardId
    {
        get => _currentBoardId;
        set => SetField(ref _currentBoardId, value);
    }
}
