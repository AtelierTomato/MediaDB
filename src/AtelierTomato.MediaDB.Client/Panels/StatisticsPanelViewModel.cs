using System.ComponentModel;
using System.Composition;

namespace AtelierTomato.MediaDB.Client.Panels
{
	[Export]
	[Shared]
	public class StatisticsPanelViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;
	}
}
