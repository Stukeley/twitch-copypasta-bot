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
			string title = TitleBox.Text;

			if (ContentBox.Text == "")
			{
				ContentBox.Text = "Treść nie może być pusta!";
				return;
			}

			string content = ContentBox.Text;

			DateTime dateAdded = DateTime.Now;

			bool isFavourite = FavouriteCheckbox.IsChecked.Value;

			var pasta = new Copypasta
			{
				Title = title,
				Content = content,
				DateAdded = dateAdded,
				IsFavourite = isFavourite,
				ChannelFrom = ""
			};

			DatabaseOperations.AddToDatabase(pasta);
		}
	}
}
