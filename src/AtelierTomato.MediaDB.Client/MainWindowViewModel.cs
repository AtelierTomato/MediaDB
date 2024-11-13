using AtelierTomato.MediaDB.Client.Panels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Composition;
using System.Windows.Input;

namespace AtelierTomato.MediaDB.Client;

[Export]
[Shared]
public class MainWindowViewModel : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler? PropertyChanged;

	private string title = string.Empty;
	public string Title
	{
		get => title;
		set
		{
			if (title != value)
			{
				title = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}
	}

	private ObservableCollection<Section> sections;
	public ObservableCollection<Section> Sections
	{
		get => sections;
		set
		{
			if (sections != value)
			{
				sections = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sections)));
			}
		}
	}

	private Section selectedSection;
	public Section SelectedSection
	{
		get => selectedSection;
		set
		{
			if (selectedSection != value)
			{
				selectedSection = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSection)));
			}
		}
	}

	public ICommand PressButtonCommand { get; }
	[ImportingConstructor]
	public MainWindowViewModel(StatisticsPanel statisticsTab)
	{
		Title = "MediaDB Client";
		PressButtonCommand = new DelegateCommand(ButtonPressed);
		Sections =
		[
			new Section
			{
				Name = "Statistics",
				//Icon = new Image
				//{
				//	Source = new BitmapImage
				//	{
				//		UriSource = new Uri("/placeholder.png")
				//	}
				//},
				Content = statisticsTab
			}
		];
	}

	private void ButtonPressed()
	{
		throw new NotImplementedException();
	}
}
