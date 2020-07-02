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

			if (Titles.Language == "EN")
			{
				FavouriteCheckbox.Content = Titles.Favourite_EN;
				SaveButton.Content = Titles.Save_EN;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(TitleBox, Titles.Title_EN);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ContentBox, Titles.Content_EN);
			}
			else
			{
				FavouriteCheckbox.Content = Titles.Favourite;
				SaveButton.Content = Titles.Save;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(TitleBox, Titles.Title);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ContentBox, Titles.Content);
			}
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			if (ContentBox.Text == "")
			{
				if (Titles.Language == "EN")
				{
					MessageBox.Show(Titles.ContentEmptyError_EN, Titles.ErrorTitle_EN, MessageBoxButton.OK, MessageBoxImage.Error);
				}
				else
				{
					MessageBox.Show(Titles.ContentEmptyError, Titles.ErrorTitle, MessageBoxButton.OK, MessageBoxImage.Error);
				}

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
