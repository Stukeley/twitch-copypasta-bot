﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	public partial class CopypastaBlock : UserControl
	{
		public Copypasta Copypasta;

		public CopypastaBlock(Copypasta copypasta)
		{
			InitializeComponent();

			//? How is this ever null?

			if (copypasta != null)
			{
				Copypasta = copypasta;
				ContentBlock.Text = Copypasta.Content;
				DateBlock.Text = Copypasta.DateAdded.ToString("dd.MM.y H:mm");

				if (string.IsNullOrEmpty(Copypasta.Title))
				{
					TitleBlock.Text = Titles.Language == "EN" ? Titles.Pasta_NoTitle_EN : Titles.Pasta_NoTitle;
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
