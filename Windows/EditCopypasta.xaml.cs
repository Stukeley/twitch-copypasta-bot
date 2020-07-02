using System;
using System.Windows;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	public partial class EditCopypasta : Window
	{
		private Copypasta Copypasta;

		public EditCopypasta(Copypasta copypasta)
		{
			InitializeComponent();

			//? Translation

			if (Titles.Language == "EN")
			{
				FavouriteCheckbox.Content = Titles.Favourite_EN;
				DeleteCopypasta.Content = Titles.Delete_EN;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(TitleBox, Titles.Title_EN);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ContentBox, Titles.Content_EN);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(DateAddedPicker, Titles.DateAdded_EN);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ChannelFromBox, Titles.ChannelFrom_EN);

			}
			else
			{
				FavouriteCheckbox.Content = Titles.Favourite;
				DeleteCopypasta.Content = Titles.Delete;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(TitleBox, Titles.Title);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ContentBox, Titles.Content);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(DateAddedPicker, Titles.DateAdded);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(ChannelFromBox, Titles.ChannelFrom);
			}

			//? End translation

			Copypasta = copypasta;

			TitleBox.Text = Copypasta.Title;
			ContentBox.Text = Copypasta.Content;
			FavouriteCheckbox.IsChecked = Copypasta.IsFavourite;
			DateAddedPicker.SelectedDate = Copypasta.DateAdded;
			ChannelFromBox.Text = Copypasta.ChannelFrom;
		}

		private void EditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// If nothing changed, just leave
			//? Is there a more optimal way to check this?
			if (Copypasta.Title == TitleBox.Text && Copypasta.IsFavourite == FavouriteCheckbox.IsEnabled && Copypasta.ChannelFrom == ChannelFromBox.Text
				&& Copypasta.Content == ContentBox.Text && Copypasta.DateAdded == DateAddedPicker.SelectedDate.Value)
			{
				return;
			}

			Copypasta.Title = TitleBox.Text;
			Copypasta.IsFavourite = FavouriteCheckbox.IsEnabled;
			Copypasta.ChannelFrom = ChannelFromBox.Text;

			if (ContentBox.Text != "")
			{
				Copypasta.Content = ContentBox.Text;
			}

			if (DateAddedPicker.SelectedDate.HasValue)
			{
				//! Important - DatePicker only picks date, not time
				//! Time has to be set separately
				var now = DateTime.Now;

				var dateTime = new DateTime(DateAddedPicker.SelectedDate.Value.Year, DateAddedPicker.SelectedDate.Value.Month, DateAddedPicker.SelectedDate.Value.Day,
					now.Hour, now.Minute, now.Second);

				Copypasta.DateAdded = dateTime;
			}

			DatabaseOperations.UpdateCopypasta(Copypasta);
		}

		private void DeleteCopypasta_Click(object sender, RoutedEventArgs e)
		{
			MessageBoxResult result;

			if (Titles.Language == "EN")
			{
				result = MessageBox.Show(Titles.ConfirmDeletion_EN, Titles.ConfirmDeletionTitle_EN, MessageBoxButton.YesNo, MessageBoxImage.Question);
			}
			else
			{
				result = MessageBox.Show(Titles.ConfirmDeletion, Titles.ConfirmDeletionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
			}

			if (result == MessageBoxResult.Yes)
			{
				DatabaseOperations.DeleteCopypasta(Copypasta);
			}
		}
	}
}
