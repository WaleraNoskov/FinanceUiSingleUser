using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FinanceUi.Windows.Feature.BoardsManagement.Controls.BoardsSearchBar
{
	public sealed partial class BoardsSearchBar : UserControl
	{
		private BoardsSearchBarViewModel _viewModel;

        private CancellationTokenSource? _cts;

        public BoardsSearchBar()
		{
			InitializeComponent();
		}

        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
			_viewModel = (args.NewValue as BoardsSearchBarViewModel)!;
        }

        private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            _cts?.Cancel(); 
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            try
            {
                await Task.Delay(250, token);
                if (!token.IsCancellationRequested && _viewModel.RestoreCommand.CanExecute(null))
                {
                    await _viewModel.RestoreCommand.ExecuteAsync(null);
                }
            }
            catch (TaskCanceledException)
            {
                // Нормально, пользователь продолжает ввод
            }
        }
    }
}
