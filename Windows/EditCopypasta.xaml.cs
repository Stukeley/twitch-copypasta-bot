using System;
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
using System.Windows.Shapes;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	//todo: add hints for the boxes
	public partial class EditCopypasta : Window
	{
		public Copypasta Copypasta;

		public EditCopypasta(Copypasta copypasta)
		{
			InitializeComponent();

			// debug only
			if (copypasta == null)
			{
				TitleBox.Text = "Error!";
				ContentBox.Text = "Something went wrong";
			}
			else
			{
				Copypasta = copypasta;

				TitleBox.Text = Copypasta.Title;
				ContentBox.Text = Copypasta.Content;
				FavouriteCheckbox.IsChecked = Copypasta.IsFavourite;
				DateAddedPicker.SelectedDate = Copypasta.DateAdded;
				ChannelFromBox.Text = Copypasta.ChannelFrom;
			}
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
				Copypasta.DateAdded = DateAddedPicker.SelectedDate.Value;
			}

			DatabaseOperations.UpdateCopypasta(Copypasta);
		}
	}
}
