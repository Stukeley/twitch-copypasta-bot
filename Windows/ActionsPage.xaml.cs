using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using TwitchCopypastaBot.Bot;
using TwitchCopypastaBot.Database;

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
			// TODO: maybe not hardcode this

			if (!TwitchChatBot.Instance.IsActive)
			{
				StartButton.Content = "Uruchom bota";
			}
			else
			{
				StartButton.Content = "Zatrzymaj bota";
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

				TwitchChatBot.Instance.Disconnect();
				Thread.Sleep(1000);
				TwitchChatBot.Instance.Connect();
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

		private void DebugButton_Click(object sender, RoutedEventArgs e)
		{
			var filePath = Path.Combine(Models.Titles.LogsDirectoryName, "Copypastas" + DateTime.Now.ToString().Replace(':', '.') + ".txt");
			DatabaseOperations.WritePastasToTextFile(filePath);
		}

		private void ClearButton_Click(object sender, RoutedEventArgs e)
		{
			//todo: some confirmation?
			DatabaseOperations.ClearDatabase();
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
				}
			}
		}
	}
}
