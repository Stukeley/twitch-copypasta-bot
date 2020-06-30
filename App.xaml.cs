using System.Windows;
using TwitchCopypastaBot.Bot;

namespace TwitchCopypastaBot
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Exit(object sender, ExitEventArgs e)
		{
			if (TwitchChatBot.Instance.IsActive)
			{
				TwitchChatBot.Instance.Disconnect();
			}
		}
	}
}
