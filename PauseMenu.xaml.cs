using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for PauseMenu.xaml
    /// </summary>
    public partial class PauseMenu : UserControl
    {
        static List<Boat> playerBoats = new List<Boat>();
        static List<Boat> computerBoats = new List<Boat>();

        static string[,] playerGrid;
        static string[,] computerGrid;
        public PauseMenu(List<Boat> PlayerBoats, List<Boat> ComputerBoats, string[,] PlayerGrid, string[,] ComputerGrid)
        {
            InitializeComponent();

            //sets up all of the grids and boats passed into the window
            playerBoats = PlayerBoats;
            computerBoats = ComputerBoats;
            playerGrid = PlayerGrid;
            computerGrid = ComputerGrid;
        }

        public PauseMenu()
        {
            InitializeComponent();
        }

        /// <summary>
        /// saves the game (both grids and both sets of boats) to a text file
        /// </summary>
        private void ButtonClick_SaveGame(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            //changes the button properties to show that the game has been saved
            clickedButton.Content = "Game Saved";
            clickedButton.IsEnabled = false;

            using (StreamWriter sw = new StreamWriter(@"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\bin\Debug\net7.0-windows\LoadTextFile.txt"))
            {
                //writes in each line of the player grid (8 lines)
                for (int i = 0; i < playerGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < playerGrid.GetLength(1); j++)
                    {
                        sw.Write(playerGrid[i, j]);
                    }
                    sw.WriteLine();
                }
                sw.WriteLine();
                //skip one line
                //writes in each line of the computer grid (8 lines)
                for (int i = 0; i < computerGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < computerGrid.GetLength(1); j++)
                    {
                        sw.Write(computerGrid[i, j]);
                    }
                    sw.WriteLine();
                }





                //loops through five times (the total number of boats at the start)
                //when it comes to loading from the save file we use the line numbers as indicators for when the computer's4
                //or player's boat group starts. Therefore if there are boats missing we print an empty line
                for(int i = 0; i < 5; i++)
                {
                    //if i is greater than the current number of boats then we know we are counting dead boats
                    //in that case leave an empty line
                    if(i > playerBoats.Count - 1)
                    {
                        sw.WriteLine();
                    }
                    else
                    {
                        //write the name of the boat
                        sw.Write(playerBoats[i].BoatType.ToString() + " ");

                        //write all the positions of the boat
                        for (int j = 0; j < playerBoats[i].boatPositions.Count; j++)
                        {
                            sw.Write(playerBoats[i].boatPositions[j].row + " " + playerBoats[i].boatPositions[j].col + " ");
                        }
                        sw.WriteLine();
                    }
                    
                }












                //separated player and computer boats ("C" specifies that the next section is the computer boats)
                sw.WriteLine("C");



                for (int i = 0; i < 5; i++)
                {

                    if (i > computerBoats.Count - 1)
                    {
                        sw.WriteLine();
                    }
                    else
                    {
                        sw.Write(computerBoats[i].BoatType.ToString() + " ");

                        for (int j = 0; j < computerBoats[i].boatPositions.Count; j++)
                        {
                            sw.Write(computerBoats[i].boatPositions[j].row + " " + computerBoats[i].boatPositions[j].col + " ");
                        }
                        sw.WriteLine();
                    }

                }



            }
        }
        /// <summary>
        /// Opens the instructions window
        /// </summary>
        private void ButtonClick_Instructions(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            //passes in "Pause" so the program knows where it was called from so that the back button takes you to the same place.
            window.Content = new InstructionsWindow("Pause");
        }
        /// <summary>
        /// Opens the main menu
        /// </summary>
        private void ButtonClick_MainMenu(object sender, RoutedEventArgs e)
        {
            //If the user hasn't saved the game it asks them if they would like to
            if(SaveGameButton.IsEnabled == true)
            {
                MessageBoxResult m = MessageBox.Show("Would you like to save your game?", "Save Game?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //if they wish to save then it exits this method and cancels going back to the main menu
                if(m == MessageBoxResult.Yes)
                {
                    return;
                }
                else
                {
                    Window window = Window.GetWindow(this);
                    window.Content = new MainMenu();
                }
            }
            //if the user has already saved it takes then straight to the main menu
            else
            {
                Window window = Window.GetWindow(this);
                window.Content = new MainMenu();
            }
            
        }

        private void ButtonClick_Back(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Content = new PlayGameWindow();
        }
    }
}
