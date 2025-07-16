using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace FinanceUi.WinUi.Feature.BoardsManagement.AddOrEditBoardForm;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class AddOrEditBoardDialogView : ContentDialog
{
	private AddOrEditBoardFormViewModel? _viewModel;

    public AddOrEditBoardDialogView()
    {
        InitializeComponent();
    }

	private void Page_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
	{
		_viewModel = (args.NewValue as AddOrEditBoardFormViewModel)!;
		this.Title = _viewModel.IsEditMode ? "�������������� �����" : "���������� �����";
		this.PrimaryButtonText = _viewModel.IsEditMode ? "���������" : "��������";
	}

	private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
	{
		_viewModel?.SaveCommand.NotifyCanExecuteChanged();
    }
}
