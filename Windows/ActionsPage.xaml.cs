using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TwitchCopypastaBot.Bot;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	public partial class ActionsPage : UserControl
	{
		public ActionsPage()
		{
			InitializeComponent();

			RefreshTextBlocks();
		}

		private void RefreshTextBlocks()
		{
			//? Translation

			if (Titles.Language == "EN")
			{
				CurrentChannel.Text = Titles.CurrentChannel_EN;
				TimeConnected.Text = Titles.BotRunningFor_EN;
				ChangeChannelBlock.Text = Titles.ChangeChannel_EN;
				ChangeChannelButton.Content = Titles.Change_EN;
				SaveToTxtButton.Content = Titles.SaveToTxt_EN;
				ClearButton.Content = Titles.ClearDatabase_EN;
				ChooseLogsDirectoryButton.Content = Titles.ChooseLogsDirectory_EN;
				ChooseBotInfoDirectoryButton.Content = Titles.ChooseBotInfoDirectory_EN;
				ChangeLanguageButton.Content = Titles.ChangeLanguage_EN;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ChangeChannel, Titles.ChannelFrom_EN);
			}
			else
			{
				CurrentChannel.Text = Titles.CurrentChannel;
				TimeConnected.Text = Titles.BotRunningFor;
				ChangeChannelBlock.Text = Titles.ChangeChannel;
				ChangeChannelButton.Content = Titles.Change;
				SaveToTxtButton.Content = Titles.SaveToTxt;
				ClearButton.Content = Titles.ClearDatabase;
				ChooseLogsDirectoryButton.Content = Titles.ChooseLogsDirectory;
				ChooseBotInfoDirectoryButton.Content = Titles.ChooseBotInfoDirectory;
				ChangeLanguageButton.Content = Titles.ChangeLanguage;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ChangeChannel, Titles.ChannelFrom);
			}

			//? End translation

			if (!TwitchChatBot.Instance.IsActive)
			{
				StartButton.Content = Titles.Language == "EN" ? Titles.StartBot_EN : Titles.StartBot;
				TimeConnected.Visibility = Visibility.Hidden;
			}
			else
			{
				StartButton.Content = Titles.Language == "EN" ? Titles.StopBot_EN : Titles.StopBot;
				TimeConnected.Visibility = Visibility.Visible;
			}

			CurrentChannel.Text = CurrentChannel.Text.Replace("[0]", TwitchChatBot.ChannelName);
			ChangeChannel.Text = ChangeChannel.Text.Replace("[0]", TwitchChatBot.ChannelName);

			TimeConnected.Text = TimeConnected.Text.Replace("[1]", TwitchChatBot.Instance.DateStarted.Hour.ToString() + ":" +
				TwitchChatBot.Instance.DateStarted.Minute.ToString());
		}

		private void ChangeChannelButton_Click(object sender, RoutedEventArgs e)
		{
			// Make sure anything was typed to the TextBox AND that it's not the same channel
			if (!string.IsNullOrWhiteSpace(ChangeChannel.Text) && TwitchChatBot.ChannelName.ToLower() != ChangeChannel.Text.ToLower())
			{
				TwitchChatBot.ChannelName = ChangeChannel.Text;

				if (TwitchChatBot.Instance.IsActive)
				{
					TwitchChatBot.Instance.Disconnect();
					Thread.Sleep(500);
					TwitchChatBot.Instance.Connect();
				}
			}
		}

		private void StartButton_Click(object sender, RoutedEventArgs e)
		{
			if (!TwitchChatBot.Instance.IsActive)
			{
				TwitchChatBot.Instance.Connect();
			}
			else
			{
				TwitchChatBot.Instance.Disconnect();
			}

			RefreshTextBlocks();
		}

		private void SaveToTxtButton_Click(object sender, RoutedEventArgs e)
		{
			var filePath = Path.Combine(Models.Titles.LogsDirectoryName, "Copypastas" + DateTime.Now.ToString().Replace(':', '.') + ".txt");
			DatabaseOperations.WritePastasToTextFile(filePath);
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result;

			if (Titles.Language == "EN")
			{
				result = MessageBox.Show(Titles.ConfirmDatabaseDeletion_EN, Titles.ConfirmDatabaseDeletionTitle_EN, MessageBoxButton.YesNo, MessageBoxImage.Question);
			}
			else
			{
				result = MessageBox.Show(Titles.ConfirmDatabaseDeletion, Titles.ConfirmDatabaseDeletionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
			}

			if (result == MessageBoxResult.Yes)
			{
				DatabaseOperations.ClearDatabase();
			}
		}

		private void ChooseLogsDirectoryButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				var result = dialog.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					//make it the new Directory for logs (file name stays the same)
					//gets only the folder
					var path = Path.GetFileName(dialog.SelectedPath);
					Titles.CurrentLogFileName = path;
					Properties.Settings.Default["LogsFolderPath"] = path;
					Properties.Settings.Default.Save();
				}
			}
		}

		private void ChooseBotInfoDirectoryButton_Click(object sender, RoutedEventArgs e)
		{
			using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
			{
				var result = dialog.ShowDialog();
				if (result == System.Windows.Forms.DialogResult.OK)
				{
					//make it the new Directory for getting bot info
					//gets folder and the file name
					var path = dialog.SelectedPath;
					TwitchChatBot.BotInfoPath = path;
					Properties.Settings.Default["BotInfoPath"] = path;
					Properties.Settings.Default.Save();
				}
			}
		}

		private void ChangeLanguageButton_Click(object sender, RoutedEventArgs e)
		{
			if (Titles.Language == "EN")
			{
				Properties.Settings.Default["Language"] = "PL";
			}
			else
			{
				Properties.Settings.Default["Language"] = "EN";
			}

			Properties.Settings.Default.Save();

			System.Windows.Forms.Application.Restart();
			Application.Current.Shutdown();
		}
	}
}
