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

namespace Battle_Boats
{


    /// <summary>
    /// Interaction logic for WinPage.xaml
    /// </summary>
    public partial class WinPage : UserControl
    {
        
        public WinPage(string winner)
        {
            InitializeComponent();
            WinnerLabel.Content = $"{winner} is the winner";
        }

        private void ButtonClick_MainMenu(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new MainMenu(true);
        }
    }
}
