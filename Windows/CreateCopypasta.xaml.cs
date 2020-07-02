using System;
using System.Windows;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	public partial class CreateCopypasta : Window
	{
		public CreateCopypasta()
		{
			InitializeComponent();
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (ContentBox.Text == "")
			{
				MessageBox.Show(Titles.ContentEmptyError, Titles.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			var pasta = new Copypasta
			{
				Title = TitleBox.Text,
				Content = ContentBox.Text,
				DateAdded = DateTime.Now,
				IsFavourite = FavouriteCheckbox.IsChecked.Value,
				ChannelFrom = ""
			};

			DatabaseOperations.AddToDatabase(pasta);
		}
	}
}
