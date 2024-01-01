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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(bool played)
        {
            InitializeComponent();
            ResumeGameButton.IsEnabled = false;
        }

        /// <summary>
        /// Exits/closes down the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_ExitGame(object sender, RoutedEventArgs e)
        {
            //closes down the game
            //NOTE: game is not automatically saved
            Environment.Exit(0);
        }

        /// <summary>
        /// Opens the instructions window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_OpenInstructions(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new InstructionsWindow("Main");
        }
        /// <summary>
        /// Opens the Start Game Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_PlayGameWindow(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new PlayGameWindow(false);
        }

        private void ButtonClick_ResumeGame(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new PlayGameWindow(true);
        }
    }
}
