using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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
using TwitchCopypastaBot.Windows;

namespace TwitchCopypastaBot
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			//ChangeContent(new MainPage(), Models.Titles.MainPageTitle);
		}

		private void ChangeContent(UserControl newPage, string title)
		{
			ContentGrid.Children.Clear();
			ContentGrid.Children.Add(newPage);
			CurrentPageText.Text = title;
		}

		private void Homepage_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new MainPage(), Models.Titles.MainPageTitle);
		}

		private void Browse_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			// Wszystkie w jednym miejscu jakoś
		}

		private void Edit_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			// wyszukiwarka po tytule lub zawartości z możliwością edycji
		}

		private void Actions_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			ChangeContent(new ActionsPage(), Models.Titles.ActionsPageTitle);
		}

		private void Emote_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			var credits = new CreditsWindow();
			credits.Show();
		}
	}
}
