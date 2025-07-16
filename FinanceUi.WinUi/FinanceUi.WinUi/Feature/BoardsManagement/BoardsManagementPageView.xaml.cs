using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FinanceUi.WinUi.Feature.BoardsManagement
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class BoardsManagementPageView : Page
	{
        private BoardsManagementPageViewModel _viewModel;

		public BoardsManagementPageView()
		{
			InitializeComponent();
		}

        private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            _viewModel = (args.NewValue as BoardsManagementPageViewModel)!;
        }

        private async void AddBoardButton_Click(object sender, RoutedEventArgs e)
        {
			var dialog = new AddOrEditBoardFormPage()
			{
				XamlRoot = this.XamlRoot,
				DataContext = _viewModel.GetAddOrEditFormViewModel
			};

			await dialog.ShowAsync();
        }
	}
}
