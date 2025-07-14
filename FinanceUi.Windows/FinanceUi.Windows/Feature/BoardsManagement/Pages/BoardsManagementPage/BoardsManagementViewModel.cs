using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceUi.Windows.Feature.BoardsManagement.Controls.BoardsSearchBar;
using FinanceUi.Windows.Feature.Shared;

namespace FinanceUi.Windows.Feature.BoardsManagement.Pages.BoardsManagementPage;

internal class BoardsManagementViewModel : ObservableObject
{
	private readonly BoardsSearchBarViewModel _boardSearchBarViewModel;
	public BoardsSearchBarViewModel BoardSearchBarViewModel => _boardSearchBarViewModel;

	public BoardsManagementViewModel(BoardsSearchBarViewModel boardsSearchBarViewModel)
	{
		_boardSearchBarViewModel = boardsSearchBarViewModel;
	}
}
