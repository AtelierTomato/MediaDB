using System.Composition.Hosting;
using System.Windows;

namespace AtelierTomato.MediaDB.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
	protected override void OnStartup(StartupEventArgs e)
	{
		base.OnStartup(e);

		var configuration = new ContainerConfiguration()
			.WithAssembly(typeof(App).Assembly);

		var container = configuration.CreateContainer();
		if (container.TryGetExport<MainWindow>(out var mainWindow))
		{
			mainWindow.Show();
		}
	}
}
