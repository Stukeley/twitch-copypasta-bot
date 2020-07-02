using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TwitchCopypastaBot.Database;
using TwitchCopypastaBot.Models;

namespace TwitchCopypastaBot.Windows
{
	public partial class BrowsePage : UserControl
	{
		private List<Copypasta> CopypastaSource;

		// Already clreated blocks, so that they don't have to be created ever again (just once)
		private List<CopypastaBlock> UnfilteredBlocks;

		//! Calculations

		// _Block_Block_Block_
		private int BlocksPerRow;

		// _Block_Block_Block_
		// always BlocksPerRow + 1 spaces
		private double HorizontalMarginSize;

		// _Block_Block_Block_
		//
		// _Block_Block_Block_
		// between these two
		private double VerticalMarginSize;

		// _Block_Block_Block_
		//
		// _Block_Block_Block_
		// total height of these
		private double TotalHeight;

		public BrowsePage()
		{
			InitializeComponent();

			//? Translation

			if (Titles.Language == "EN")
			{
				ResetSearch.Content = Titles.ResetFilters_EN;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(SearchTitleBox, Titles.Title_EN);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(SearchContentBox, Titles.Content_EN);
				SearchFavouritesBox.Content = Titles.Favourite_EN;
			}
			else
			{
				ResetSearch.Content = Titles.ResetFilters;
				MaterialDesignThemes.Wpf.HintAssist.SetHint(SearchTitleBox, Titles.Title);
				MaterialDesignThemes.Wpf.HintAssist.SetHint(SearchContentBox, Titles.Content);
				SearchFavouritesBox.Content = Titles.Favourite;
			}

			//? End translation

			CopypastaSource = DatabaseOperations.WritePastasToList();

			UnfilteredBlocks = new List<CopypastaBlock>();

			// Make necessary calculations
			var tempBlock = new CopypastaBlock(null);

			BlocksPerRow = (int)(CopypastaGrid.MaxWidth / tempBlock.MaxWidth);

			// Create this many columns here

			for (int i = 0; i < BlocksPerRow; i++)
			{
				var column = new ColumnDefinition();
				column.Width = new GridLength(1, GridUnitType.Star);
				CopypastaGrid.ColumnDefinitions.Add(column);
			}

			// 20 - let's assume it's the width of the VerticalScrollBar xD
			HorizontalMarginSize = Math.Floor((CopypastaGrid.MaxWidth - 20 - (BlocksPerRow * tempBlock.MaxWidth)) / 4); ;

			VerticalMarginSize = 30;

			//! TotalHeight should be changed every time the grid is updated!

			TotalHeight = (CopypastaSource.Count / BlocksPerRow + 1) * tempBlock.MaxHeight + (CopypastaSource.Count / BlocksPerRow + 1) * VerticalMarginSize;

			this.CopypastaGrid.Height = TotalHeight;

			// Create all rows
			for (int i = 0; i < CopypastaSource.Count; i++)
			{
				if (i % BlocksPerRow == 0)
				{
					var row = new RowDefinition();
					row.Height = new GridLength(VerticalMarginSize + tempBlock.MaxHeight);
					CopypastaGrid.RowDefinitions.Add(row);
				}
			}

			CreateCopypastaBlocks();

			SearchTitleBox.TextChanged += SearchBox_TextChanged;
			SearchContentBox.TextChanged += SearchBox_TextChanged;
			SearchFavouritesBox.Checked += SearchFavouritesBox_Checked;
			SearchFavouritesBox.Unchecked += SearchFavouritesBox_Checked;
		}

		public void CreateCopypastaBlocks()
		{
			if (UnfilteredBlocks == null)
			{
				// display some error
				return;
			}

			// Column number
			int i = 0;

			// Row number
			int j = 0;

			// Vertical position is ALWAYS just the margin size - it was initially modified so now I'll just keep this useless line of code
			double verticalPosition = VerticalMarginSize;

			foreach (var item in CopypastaSource)
			{
				var block = new CopypastaBlock(item);

				block.Margin = new Thickness(HorizontalMarginSize, verticalPosition, 0, 0);

				block.HorizontalAlignment = HorizontalAlignment.Left;
				block.VerticalAlignment = VerticalAlignment.Top;

				var gridColumn = i % BlocksPerRow;
				i++;

				CopypastaGrid.Children.Add(block);
				Grid.SetRow(block, j);
				Grid.SetColumn(block, gridColumn);

				// Save the current layout for future use (during searches)
				UnfilteredBlocks.Add(block);

				if (i % BlocksPerRow == 0)
				{
					// change vertical margin every 3 pastas added - fourth added copypasta will be lower than the previous three

					j++;
					var row = new RowDefinition();
					row.Height = new GridLength(VerticalMarginSize + block.MaxHeight);
					CopypastaGrid.RowDefinitions.Add(row);
				}
			}
		}

		// without creating blocks all over again - take the ones that match the search criteria, reorganize them and that's it
		private void UpdateCopypastaBlocks(string title = null, string content = null, bool? isChecked = null)
		{
			if (UnfilteredBlocks == null)
			{
				//?
				return;
			}

			CopypastaGrid.Children.Clear();

			int i = 0;  //column
			int j = 0;  //row

			double verticalPosition = VerticalMarginSize;

			foreach (var elem in UnfilteredBlocks)
			{
				// null or empty means that it's not specified and shouldn't matter
				// Everything is transformed to lowercase for better searching

				if (!string.IsNullOrEmpty(title))
				{
					// Copypasta's Title can be null - if we search for non-null and it's null, do not display it
					if (elem.Copypasta.Title == null)
					{
						continue;
					}

					if (!elem.Copypasta.Title.ToLower().Contains(title.ToLower()))
					{
						// doesn't match
						continue;
					}
				}

				if (!string.IsNullOrEmpty(content))
				{
					if (!elem.Copypasta.Content.ToLower().Contains(content.ToLower()))
					{
						// doesn't match
						continue;
					}
				}

				if (isChecked != null)
				{
					if (!elem.Copypasta.IsFavourite == isChecked)
					{
						// doesn't match
						continue;
					}
				}

				CopypastaGrid.Children.Add(elem);

				Grid.SetColumn(elem, i % BlocksPerRow);
				Grid.SetRow(elem, j);
				i++;

				elem.Margin = new Thickness(HorizontalMarginSize, verticalPosition, 0, 0);

				if (i % BlocksPerRow == 0)
				{
					j++;
				}
			}

			// temporary block to make calculations on
			var tempBlock = UnfilteredBlocks[0];

			TotalHeight = (CopypastaGrid.Children.Count / BlocksPerRow + 1) * tempBlock.MaxHeight + (CopypastaGrid.Children.Count / BlocksPerRow + 1) * VerticalMarginSize;

			// If it's less than the minimum height (one page), make it equal

			this.CopypastaGrid.Height = TotalHeight < CopypastaGrid.MinHeight ? CopypastaGrid.MinHeight : TotalHeight;
			this.CopypastaViewer.ScrollToVerticalOffset(0);
		}

		// Resets Title, Content and Favourite boxes to their initial state
		private void ResetBoxes()
		{
			SearchTitleBox.Text = "";
			SearchContentBox.Text = "";
			SearchFavouritesBox.IsChecked = false;
		}

		private void SearchFavouritesBox_Checked(object sender, RoutedEventArgs e)
		{
			UpdateCopypastaBlocks(SearchTitleBox.Text, SearchContentBox.Text, SearchFavouritesBox.IsChecked);
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateCopypastaBlocks(SearchTitleBox.Text, SearchContentBox.Text, SearchFavouritesBox.IsChecked);
		}

		private void ResetSearch_Click(object sender, RoutedEventArgs e)
		{
			UpdateCopypastaBlocks(null, null, null);
			ResetBoxes();
		}
	}
}
