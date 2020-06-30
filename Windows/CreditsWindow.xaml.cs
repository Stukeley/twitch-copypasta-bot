using System.Windows;
using System.Windows.Input;

namespace TwitchCopypastaBot.Windows
{
	public partial class CreditsWindow : Window
	{
		public CreditsWindow()
		{
			InitializeComponent();
		}

		private void Github_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/Stukeley");
		}
	}
}
