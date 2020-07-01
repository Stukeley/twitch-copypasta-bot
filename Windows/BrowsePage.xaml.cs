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

		// grid without any filters
		private UIElementCollection UnfilteredUiCollection;

		//! Calculations

		// _Block_Block_Block_
		private int BlocksPerRow;

		// _Block_Block_Block_
		// always 4 spaces
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

			CopypastaSource = DatabaseOperations.WritePastasToList();

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
		}

		public void CreateCopypastaBlocks()
		{
			if (CopypastaSource == null)
			{
				// display some error
				return;
			}

			// Column number
			int i = 0;

			// Row number
			int j = 0;

			// Vertical position - initially just the margin
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

				if (i % BlocksPerRow == 0)
				{
					// change vertical margin every 3 pastas added - fourth added copypasta will be lower than the previous three
					verticalPosition = verticalPosition + block.MaxHeight + VerticalMarginSize;

					j++;
					var row = new RowDefinition();
					row.Height = new GridLength(VerticalMarginSize + block.MaxHeight);
					CopypastaGrid.RowDefinitions.Add(row);
				}
			}

			//save the current layout for future use (during searches)
			UnfilteredUiCollection = CopypastaGrid.Children;
		}

		// without creating blocks all over again - take the ones that match the search criteria, reorganize them and that's it
		private void UpdateCopypastaBlocks()
		{
			if (UnfilteredUiCollection == null)
			{
				//?
				return;
			}

			int i = 0;
			double verticalPosition = VerticalMarginSize;

			foreach (var elem in UnfilteredUiCollection)
			{
				if (elem is CopypastaBlock)
				{
					var temp = elem as CopypastaBlock;

					CopypastaGrid.Children.Add(temp);

					Grid.SetColumn(temp, i % BlocksPerRow);
					i++;

					temp.Margin = new Thickness(HorizontalMarginSize, verticalPosition, 0, 0);


					if (i % BlocksPerRow == 0)
					{
						verticalPosition = verticalPosition + temp.Height + VerticalMarginSize;
					}

				}
			}
		}

		private void SearchFavouritesBox_Checked(object sender, RoutedEventArgs e)
		{
			if (UnfilteredUiCollection == null)
			{
				//?
				return;
			}

			int i = 0;
			double verticalPosition = VerticalMarginSize;

			foreach (var elem in UnfilteredUiCollection)
			{
				if (elem is CopypastaBlock)
				{
					var temp = elem as CopypastaBlock;

					if (temp.Copypasta.IsFavourite == SearchFavouritesBox.IsChecked)
					{
						Grid.SetColumn(temp, i % 3);
						i++;

						temp.Margin = new Thickness(HorizontalMarginSize, verticalPosition, 0, 0);

						CopypastaGrid.Children.Add(temp);

						if (i % 3 == 0)
						{
							verticalPosition = verticalPosition + temp.Height + VerticalMarginSize;
						}
					}
				}
			}
		}

		private void SearchTitleBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void SearchContentBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void ResetSearch_Click(object sender, RoutedEventArgs e)
		{
			if (UnfilteredUiCollection != null)
			{
				foreach (var elem in UnfilteredUiCollection)
				{
					if (elem is CopypastaBlock)
					{
						CopypastaGrid.Children.Add(elem as CopypastaBlock);
					}
				}

				SearchTitleBox.Text = "";
				SearchContentBox.Text = "";
				SearchFavouritesBox.IsChecked = false;
			}
		}
	}
}
