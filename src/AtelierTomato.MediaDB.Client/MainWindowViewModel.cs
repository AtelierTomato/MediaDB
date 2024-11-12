using System.ComponentModel;
using System.Composition;

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

	public MainWindowViewModel()
	{
		Title = "Hello!";
	}
}
