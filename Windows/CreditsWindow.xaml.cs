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
