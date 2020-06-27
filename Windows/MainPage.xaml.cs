using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Reflection;
using System.Diagnostics;
using TwitchCopypastaBot.Database;

namespace TwitchCopypastaBot.Windows
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
			InitializeComponent();

			RefreshTextBlocks();
		}

		private void RefreshTextBlocks()
		{
			CopypastaBotActive.Text = (Bot.TwitchChatBot.Instance.IsActive ? "Witaj. Bot aktywny!" : "Witaj. Bot jest obecnie nieaktywny.");

			int copypastasInDb, unnamedCopypatasInDb;
			string lastCopypastaDateAdded;

			copypastasInDb = DatabaseOperations.GetCopypastaCount();
			lastCopypastaDateAdded = DatabaseOperations.GetLastCopypastaDate().ToString("dd/MM/yy H:mm:ss");
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
			// Opens up the text file that contains Bot Logs created by _logger

			////var directory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			////var file = Path.Combine(directory, Models.Titles.LogsFileName);

			////Process.Start(file);
		}
	}
}
