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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	/// <summary>
	/// Interaction logic for CopypastaBlock.xaml
	/// </summary>
	public partial class CopypastaBlock : UserControl
	{
		private Copypasta Copypasta;

		public CopypastaBlock(Copypasta copypasta)
		{
			InitializeComponent();

			// debug purposes only
			if (copypasta == null)
			{
				TitleBlock.Text = "Error";
				ContentBlock.Text = "Something went wrong";
				DateBlock.Text = DateTime.Now.ToString("dd.MM.y H:mm");
				FavouriteIcon.IsEnabled = false;
			}
			else
			{
				Copypasta = copypasta;
				ContentBlock.Text = Copypasta.Content;
				DateBlock.Text = Copypasta.DateAdded.ToString("dd.MM.y H:mm");

				if (Copypasta.Title == null)
				{
					TitleBlock.Text = Titles.Pasta_NoTitle;
				}
				else
				{
					TitleBlock.Text = Copypasta.Title;
				}

				if (Copypasta.IsFavourite)
				{
					FavouriteIcon.IsEnabled = true;
					FavouriteIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Star;
					FavouriteIcon.Foreground = Brushes.Gold;
				}
				else
				{
					FavouriteIcon.IsEnabled = true;
					FavouriteIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.StarOutline;
					FavouriteIcon.Foreground = Brushes.Black;
				}
			}

		}

		private void RefreshFavouriteStar()
		{
			if (FavouriteIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.StarOutline)
			{
				FavouriteIcon.IsEnabled = true;
				FavouriteIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Star;
				FavouriteIcon.Foreground = Brushes.Gold;
			}
			else
			{
				FavouriteIcon.IsEnabled = true;
				FavouriteIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.StarOutline;
				FavouriteIcon.Foreground = Brushes.Black;
			}
		}

		private void FavouriteIcon_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			Copypasta.IsFavourite = !Copypasta.IsFavourite;
			DatabaseOperations.UpdateCopypasta(Copypasta);
			RefreshFavouriteStar();
		}

		private void TitleBlock_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var editWindow = new EditCopypasta(this.Copypasta);
			editWindow.Show();
		}
	}
}
