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
			var result = MessageBox.Show(Titles.ConfirmDeletion, Titles.ConfirmDeletionTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.Yes)
			{
				DatabaseOperations.DeleteCopypasta(Copypasta);
			}
		}
	}
}
