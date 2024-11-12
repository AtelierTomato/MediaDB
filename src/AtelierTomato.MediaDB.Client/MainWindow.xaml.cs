using System.Composition;
using System.Windows;

namespace AtelierTomato.MediaDB.Client;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
[Export]
[Shared]
public partial class MainWindow : Window
{
	[ImportingConstructor]
	public MainWindow(MainWindowViewModel viewModel)
	{
		DataContext = viewModel;
		InitializeComponent();
	}
}