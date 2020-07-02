using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TwitchCopypastaBot.Bot;
using TwitchCopypastaBot.Models;
using TwitchCopypastaBot.Windows;

namespace TwitchCopypastaBot
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			ChangeContent(new MainPage(), Models.Titles.MainPageTitle);

			Titles.Language = Properties.Settings.Default["Language"].ToString();

			//? Translation

			if (Titles.Language == "EN")
			{
				MainPageText.Text = Titles.MainPageTitle_EN;
				BrowsePageText.Text = Titles.BrowsePageTitle_EN;
				AddPageText.Text = Titles.AddPageTitle_EN;
				ActionsPageText.Text = Titles.ActionsPageTitle_EN;
			}
			else
			{
				MainPageText.Text = Titles.MainPageTitle;
				BrowsePageText.Text = Titles.BrowsePageTitle;
				AddPageText.Text = Titles.AddPageTitle;
				ActionsPageText.Text = Titles.ActionsPageTitle;
			}

			//? End translation

			if (!string.IsNullOrEmpty(Properties.Settings.Default["LogsFolderPath"].ToString()))
			{
				Titles.LogsDirectoryName = Properties.Settings.Default["LogsFolderPath"].ToString();
			}

			if (!string.IsNullOrEmpty(Properties.Settings.Default["BotInfoPath"].ToString()))
			{
				TwitchChatBot.BotInfoPath = Properties.Settings.Default["BotInfoPath"].ToString();
			}
		}

		private void ChangeContent(UserControl newPage, string title)
		{
			ContentGrid.Children.Clear();
			ContentGrid.Children.Add(newPage);
			CurrentPageText.Text = title;
		}

		private void Homepage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var title = Titles.Language == "EN" ? Titles.MainPageTitle_EN : Titles.MainPageTitle;
			ChangeContent(new MainPage(), title);
		}

		private void Browse_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			// Wszystkie w jednym miejscu jakoś
			var title = Titles.Language == "EN" ? Titles.BrowsePageTitle_EN : Titles.BrowsePageTitle;
			ChangeContent(new BrowsePage(), title);
		}

		private void Add_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			// Dodawanie nowej pasty bez bota
			// Inconsistent naming
			var window = new CreateCopypasta();
			window.Show();
		}

		private void Actions_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var title = Titles.Language == "EN" ? Titles.ActionsPageTitle_EN : Titles.ActionsPageTitle;
			ChangeContent(new ActionsPage(), title);
		}

		private void Emote_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var credits = new CreditsWindow();
			credits.Show();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Save strings to resource file
			// todo: make this work xd (why doesn't it already?)
			Properties.Settings.Default["LogsFolderPath"] = Titles.LogsDirectoryName;
			Properties.Settings.Default["BotInfoPath"] = TwitchChatBot.BotInfoPath;
			Properties.Settings.Default.Save();
			Properties.Settings.Default.Reload();
		}
	}
}
