using System.Composition;
using System.Windows.Controls;

namespace AtelierTomato.MediaDB.Client.Panels
{
	/// <summary>
	/// Interaction logic for StatisticsPanel.xaml
	/// </summary>
	[Export]
	[Shared]
	public partial class StatisticsPanel : UserControl
	{
		[ImportingConstructor]
		public StatisticsPanel(StatisticsPanelViewModel viewModel)
		{
			DataContext = viewModel;
			InitializeComponent();
		}
	}
}
