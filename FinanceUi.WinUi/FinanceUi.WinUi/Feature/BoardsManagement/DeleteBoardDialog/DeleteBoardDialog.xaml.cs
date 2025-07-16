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

namespace FinanceUi.WinUi.Feature.BoardsManagement.DeleteBoardDialog;

public sealed partial class DeleteBoardDialogView : ContentDialog
{
    private DeleteBoardDialogViewModel _viewModel;

    public DeleteBoardDialogView()
    {
        InitializeComponent();
    }

	private void ContentDialog_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
	{
        _viewModel = (args.NewValue as DeleteBoardDialogViewModel)!;
	}
}
