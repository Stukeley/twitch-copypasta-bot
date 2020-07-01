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
			CopypastaBotActive.Text = (Bot.TwitchChatBot.Instance.IsActive ? "Witaj. Bot aktywny!" : "Witaj. Bot jest obecnie nieaktywny.");

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
				CopypastaUntitledCount.Text = "Wszystkie Copypasty w bazie danych mają tytuł. Hura!";
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
