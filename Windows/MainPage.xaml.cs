using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();

			RefreshTextBlocks();
		}

		// TODO: make this async
		private void RefreshTextBlocks()
		{
			//? Translation

			if (Titles.Language == "EN")
			{
				CopypastaBotActive.Text = (Bot.TwitchChatBot.Instance.IsActive ? Titles.BotActive_EN : Titles.BotInactive_EN);
				CopypastaCountText.Text = Titles.CurrentCopypastaCount_EN;
				CopypastaDateText.Text = Titles.LastCopypastaDate_EN;
				CopypastaUntitledCount.Text = Titles.UnnamedCopypastaCount_EN;
				SeeLogsButton.Text = Titles.OpenLogs_EN;
			}
			else
			{
				CopypastaBotActive.Text = (Bot.TwitchChatBot.Instance.IsActive ? Titles.BotActive : Titles.BotInactive);
				CopypastaCountText.Text = Titles.CurrentCopypastaCount;
				CopypastaDateText.Text = Titles.LastCopypastaDate;
				CopypastaUntitledCount.Text = Titles.UnnamedCopypastaCount;
				SeeLogsButton.Text = Titles.OpenLogs;
			}

			//? End translation

			int copypastasInDb, unnamedCopypatasInDb;
			string lastCopypastaDateAdded;

			copypastasInDb = DatabaseOperations.GetCopypastaCount();

			DateTime? lastCopypastaDate = DatabaseOperations.GetLastCopypastaDate();
			if (lastCopypastaDate == null)
			{
				lastCopypastaDateAdded = "Brak danych";
			}
			else
			{
				lastCopypastaDateAdded = lastCopypastaDate.Value.ToString("dd/MM/yy H:mm:ss");
			}

			unnamedCopypatasInDb = DatabaseOperations.GetUnnamedCopypastaCount();

			CopypastaCountText.Text = CopypastaCountText.Text.Replace("[0]", copypastasInDb.ToString());
			CopypastaDateText.Text = CopypastaDateText.Text.Replace("[1]", lastCopypastaDateAdded);

			if (unnamedCopypatasInDb > 0)
			{
				CopypastaUntitledCount.Text = CopypastaUntitledCount.Text.Replace("[2]", unnamedCopypatasInDb.ToString());
			}
			else
			{
				CopypastaUntitledCount.Text = Titles.Language == "EN" ? Titles.AllHaveTitles_EN : Titles.AllHaveTitles;
			}
		}

		private void SeeLogsButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			if (Directory.Exists(Titles.LogsDirectoryName))
			{
				Process.Start(Titles.LogsDirectoryName);
			}
		}
	}
}
