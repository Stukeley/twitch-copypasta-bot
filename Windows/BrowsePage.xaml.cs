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
		private double VerticalMarginSize;

		public BrowsePage()
		{
			InitializeComponent();

			CopypastaSource = DatabaseOperations.WritePastasToList();

			// Make necessary calculations
			var tempBlock = new CopypastaBlock(null);

			BlocksPerRow = (int)(this.Width / tempBlock.Width);

			HorizontalMarginSize = Math.Floor((this.Width - (3 * tempBlock.Width)) / 4);

			VerticalMarginSize = 30;
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

			// Vertical position - initially just the margin
			double verticalPosition = VerticalMarginSize;

			foreach (var item in CopypastaSource)
			{
				var block = new CopypastaBlock(item);

				block.Margin = new Thickness(HorizontalMarginSize, verticalPosition, 0, 0);

				var gridColumn = i % 3;
				i++;

				if (i % 3 == 0)
				{
					// change vertical margin every 3 pastas added - fourth added copypasta will be lower than the previous three
					verticalPosition = verticalPosition + block.Height + VerticalMarginSize;
				}

				Grid.SetColumn(block, gridColumn);
				CopypastaGrid.Children.Add(block);
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
