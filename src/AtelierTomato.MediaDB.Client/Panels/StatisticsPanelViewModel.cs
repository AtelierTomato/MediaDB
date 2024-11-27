using AtelierTomato.MediaDB.APIClient;
using AtelierTomato.MediaDB.Client.Panels.Models;
using System.ComponentModel;
using System.Composition;
using System.Windows.Input;

namespace AtelierTomato.MediaDB.Client.Panels
{
	[Export]
	[Shared]
	public class StatisticsPanelViewModel : INotifyPropertyChanged
	{
		private RestClient _client;

		public event PropertyChangedEventHandler? PropertyChanged;

		public ICommand GetStatisticsCommand { get; }
		private MediaDBStats? mediaDBStats;
		public MediaDBStats? MediaDBStats
		{
			get => mediaDBStats;
			set
			{
				if (mediaDBStats != value)
				{
					mediaDBStats = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MediaDBStats)));
				}
			}
		}

		[ImportingConstructor]
		public StatisticsPanelViewModel(RestClient client)
		{
			_client = client;
			GetStatisticsCommand = new DelegateCommand(GetStatistics);
		}

		private async void GetStatistics()
		{
			MediaDBStats = new MediaDBStats
			(
				await _client.CountSeriesAsync(),
				await _client.CountSeriesNamesAsync(),
				await _client.CountSeriesParentsAsync(),
				await _client.CountPartsAsync(),
				await _client.CountPartNamesAsync(),
				await _client.CountPartGroupInfoAsync(),
				await _client.CountPartGroupNamesAsync()
			);
		}
	}
}
