using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using System.IO;
using System.Collections.ObjectModel;

namespace Battle_Boats
{

    public partial class PlayGameWindow : UserControl
    {

        //STATIC VARIABLES:



        static PlayGameWindow instance;

        //Basic setup 

        //The player's list of available boats
        static List<Boat> playerBoats = new List<Boat>
        {
            new Boat("Aircraft Carrier", 5),
            new Boat("Battleship",4),
            new Boat("Cruiser",3),
            new Boat("Submarine",3),
            new Boat("Destroyer",2)

        };


        static List<Boat> computerBoats = new List<Boat>
        {
            new Boat("Aircraft Carrier", 5),
            new Boat("Battleship",4),
            new Boat("Cruiser",3),
            new Boat("Submarine",3),
            new Boat("Destroyer",2)
        };




        //the user array of ships / the ocean
        //e - empty
        //b - boat
        //h - hit 
        //m - miss
        static string[,] computerGrid =
        {
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" }
        };


        static string[,] playerGrid =
        {
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" },
            {"e","e","e","e","e","e","e","e" }
        };

        //allows me to access the player grid from the window
        static Grid WindowPlayerGrid = new Grid();
        //now for the Shooting grid
        static Grid WindowShootingGrid = new Grid();



        //all the other buttons and labeles just so that i can delete them later (idk how to access them otherwise)
        static TextBox WindowInputPositionBox = new TextBox();
        static TextBlock WindowQuestion = new TextBlock();
        static Button WindowRotateBoatButton = new Button();
        static Button WindowPlaceBoatButton = new Button();
        static Button WindowStartGameButton = new Button();

        //---------------------------------------------------------------------------------------------------------------

        //CONSTRUCTORS:

        /// <summary>
        /// Constructor called when going from Play Game and Resume Game buttons from the Main Menu
        /// </summary>
        /// <param name="saved">
        /// Used to specify if the user pressed the Play Game or Resume Game button (if resume game button clicked then saved = true)
        /// </param>
        public PlayGameWindow(bool saved)
        {
            InitializeComponent();


            //????????????????????????
            //crude bug fix
            //does a full reset of the lists
            playerBoats.Clear();
            computerBoats.Clear();
            
            for(int i = 0; i < playerGrid.GetLength(0); i++)
            {
                for(int j = 0; j < playerGrid.GetLength(1); j++)
                {
                    playerGrid[i, j] = "e";
                }
            }

            for (int i = 0; i < computerGrid.GetLength(0); i++)
            {
                for (int j = 0; j < computerGrid.GetLength(1); j++)
                {
                    computerGrid[i, j] = "e";
                }
            }

            playerBoats = new List<Boat>{
                new Boat("Aircraft Carrier", 5),
            new Boat("Battleship", 4),
            new Boat("Cruiser", 3),
            new Boat("Submarine", 3),
            new Boat("Destroyer", 2)};

            computerBoats = new List<Boat>{
            new Boat("Aircraft Carrier", 5),
            new Boat("Battleship", 4),
            new Boat("Cruiser", 3),
            new Boat("Submarine", 3),
            new Boat("Destroyer", 2)};

            Boat.NoPlaced = 0;



            instance = this;

            //Binds the data of the boats onto the BoatsListBox so that it can display all the information of the boats
            BoatsListBox.ItemsSource = playerBoats;

            //putting the xaml window grid into a static variable that can be accessed from anywhere
            WindowPlayerGrid = PlayerGrid;
            WindowShootingGrid = ShootingGrid;


            //all the input so that i can "delete"/remove them later
            WindowInputPositionBox = InputPositionBox;
            WindowQuestion = Question;
            WindowRotateBoatButton = RotateButton;
            WindowPlaceBoatButton = PlaceBoatButton;
            WindowStartGameButton = StartGameButton;

            //crude bug fix
            //makes the menu button invisible and uniteractable
            MenuButton.Visibility = Visibility.Collapsed;

            if (saved)
            {
                //calls the load save method
                LoadSave();
                //create button grid method
                CreateButtonGrid();

                //removes the lisbox
                BoatsListBox.Visibility = Visibility.Collapsed;
                //makes the menu button visible
                MenuButton.Visibility = Visibility.Visible;

            }

        }


        /// <summary>
        /// This constructor is used only when we are pressing the back button from the Pause menu. (I couldn't think of any other way to do it.
        /// </summary>
        public PlayGameWindow()
        {
            InitializeComponent();


            instance = this;




            WindowPlayerGrid = PlayerGrid;
            WindowShootingGrid = ShootingGrid;






            //all the input so that i can immedietly delete and remove them later
            WindowInputPositionBox = InputPositionBox;
            WindowQuestion = Question;
            WindowRotateBoatButton = RotateButton;
            WindowPlaceBoatButton = PlaceBoatButton;
            WindowStartGameButton = StartGameButton;

            //removes the boat placement UI from the grid so it can no longer be interacted with.
            BoatsListBox.Visibility = Visibility.Collapsed;
            WindowInputPositionBox.Visibility = Visibility.Collapsed;
            WindowPlaceBoatButton.Visibility = Visibility.Collapsed;
            WindowQuestion.Visibility = Visibility.Collapsed;
            WindowRotateBoatButton.Visibility = Visibility.Collapsed;
            WindowStartGameButton.Visibility = Visibility.Collapsed;




            //adds the images to the shooting grid
            for (int i = 0; i < computerGrid.GetLength(0); i++)
            {
                for (int j = 0; j < computerGrid.GetLength(1); j++)
                {
                    if (computerGrid[i, j] == "h")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowShootingGrid);
                    }
                    else if (computerGrid[i, j] == "m")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\BlueBackground.png", WindowShootingGrid);

                    }

                }
            }

            //marks the locations of the player's boats on their grid
            for (int i = 0; i < playerGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playerGrid.GetLength(1); j++)
                {
                    if (playerGrid[i, j] == "b")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowPlayerGrid);
                    }
                    else if (playerGrid[i, j] == "h")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\BigRedX.jpeg", WindowPlayerGrid);

                    }

                }
            }

            CreateButtonGrid();
        }


        //---------------------------------------------------------------------------------------------------------------

        //EVENTS:

        /// <summary>
        /// 
        /// Deletes the button once it has been clicked, and adds a colour on the grid depending on if it has been hit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ButtonClick_Shoot(object sender, RoutedEventArgs e)
        {


            //finds the pressed button to do operations with it later
            Button clickedButton = (Button)sender;


            //Converts the content (the number of each button which also represents its position in the grid) into a string
            string tempConversion = Convert.ToString(clickedButton.Content);
            string char1 = Convert.ToString(tempConversion[0]);
            string char2 = Convert.ToString(tempConversion[1]);
            //converts the string into a tuple so we can easily (and cleanly) access the row and column
            (int row, int col) buttonPosition = (Convert.ToInt32(char1) - 1, Convert.ToInt32(char2) - 1);







            //checks if the position of the button pressed contains a boat. If it does then the button outputs "HIT", otherwise "MISS"
            if (computerGrid[buttonPosition.row, buttonPosition.col] == "b")
            {
                //removes the button
                clickedButton.Visibility = Visibility.Collapsed;

                //When a boat is hit. It turns the square red
                AddImageToGrid(buttonPosition.row, buttonPosition.col, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowShootingGrid);

                //changes the value of the ComputerArray (the player hit)
                DeleteBoatOrBoatPosition((buttonPosition.row, buttonPosition.col), computerBoats, true, computerGrid);
            }
            //otherwise it turns the square blue
            else
            {
                clickedButton.Visibility = Visibility.Collapsed;

                //miss turns the square blue
                AddImageToGrid(buttonPosition.row, buttonPosition.col, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\BlueBackground.png", WindowShootingGrid);

                //changes value of computer arrray (The player missed)
                computerGrid[buttonPosition.row, buttonPosition.col] = "m";
            }

            //starts the computer shoot method
            ComputerShoot();

        }

        /// <summary>
        /// Event called when the user presses the PlaceBoat button. after some checks & validation it calls the actual PlaceBoat method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_PlaceBoat(object sender, RoutedEventArgs e)
        {
            //Retrieves the input position that was put in the input position box
            string inputPosition = InputPositionBox.Text;

            //accesses the item from the list box that the user selected. This is done again in the else statement but it has been converted into a boat type.
            var selected = BoatsListBox.SelectedItem;

            //Checks to see if the input was valid
            if (!ValidateInputPosition(inputPosition))
            {
                //returns the cursor to the InputPositionBox so the user can try again
                InputPositionBox.Focus();
                return;
            }
            //Makes sure the user has selected something from the list of boats, else it throws an error at the user >:)
            else if (selected == null)
            {
                BoatsListBox.Focus();
                MessageBox.Show("You have not selected a boat to place from the list of boats", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }
            //checks to see if the player will place too many boats (5 is the max)
            else if (Boat.NoPlaced == 5)
            {
                MessageBox.Show("You cannot place any more boats", "Boat Limit Reached", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                //converts the inputPositions into ints
                //taking away 1 bc i'm expecting the user to input the position with the grid NOT starting from 0
                int rowPos = Convert.ToInt32(Convert.ToString(inputPosition[0])) - 1;
                int colPos = Convert.ToInt32(Convert.ToString(inputPosition[1])) - 1;


                //converts the selected item into a boat so that we can actually act on it and use its data.
                Boat selectedBoat = (Boat)BoatsListBox.SelectedItem;


                PlaceBoat(selectedBoat, rowPos, colPos, selectedBoat.RotationDirection, true, playerGrid);

                //updates the boat details so that it shows that the boat has been placedpause
                ShowBoatDetails.Text = $"{selectedBoat.BoatType} \nBoat Roation: {selectedBoat.RotationDirection}° \nPlaced: {selectedBoat.Placed}";

            }

        }

        /// <summary>
        /// Event called when the user presses the Rotate Boat button. It changes the RotationDirection property of the boat class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_RotateBoat(object sender, RoutedEventArgs e)
        {

            /*
             * Rotation Direction Values:
             * 0 = from input position upwards
             * 90 = from input position right
             * 180 = from input position downwards
             * 270 = from input position left
            */


            //incase the user forgot to select a boat.
            if ((Boat)BoatsListBox.SelectedItem == null)
            {
                MessageBox.Show("Your forgot to select a boat before rotating.", "Boat not selected", MessageBoxButton.OK, MessageBoxImage.Warning); ;
            }
            else
            {
                //converts the selected value into Boat type
                Boat selectedBoat = (Boat)BoatsListBox.SelectedValue;
                //increases the rotation direction of the boat (it changes the value of the actual boat type in the list (isn't that nice))
                selectedBoat.RotationDirection += 90;


                //updates the textbox that shows the details of the boat for the user
                ShowBoatDetails.Text = $"{selectedBoat.BoatType} \nBoat Roation = {selectedBoat.RotationDirection}°";
            }


        }

        /// <summary>
        /// Starts the game: removes all place boat UI, creates button grid and random boat placement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_StartGame(object sender, RoutedEventArgs e)
        {
            //checks to see that the player has placed all of the boats
            if (Boat.NoPlaced < 5)
            {
                MessageBox.Show("Not all of your boats have been placed", "Boat placement error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                Button clickedButton = (Button)sender;



                //removes the boat placement UI from the grid so it can no longer be interacted with.
                BoatsListBox.Visibility = Visibility.Collapsed;
                WindowInputPositionBox.Visibility = Visibility.Collapsed;
                WindowPlaceBoatButton.Visibility = Visibility.Collapsed;
                //for some reason this one doesn't work vvvv
                WindowQuestion.Visibility = Visibility.Collapsed;
                WindowRotateBoatButton.Visibility = Visibility.Collapsed;
                clickedButton.Visibility = Visibility.Collapsed;

                //creates the grid of buttons 
                CreateButtonGrid();

                //creates the random positions of the computer boats
                PlaceComputerBoats();

                //writes to the console to show the postions of the computer boats for debugging
                for (int i = 0; i < computerGrid.GetLength(0); i++)
                {
                    for (int j = 0; j < computerGrid.GetLength(1); j++)
                    {
                        Console.Write(" " + computerGrid[i, j] + " ");
                    }
                    Console.WriteLine();
                }

                //now makes the menu button visible
                MenuButton.Visibility = Visibility.Visible;

            }


        }

        /// <summary>
        /// Shows details of the selected boat. Used to tell the rotation of each boat so that you don't need to think about it mentally
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBox_SelectedBoat(object sender, RoutedEventArgs e)
        {
            Boat b = null;

            //finds the selected boat in the players list of boats. b = the boat found
            for (int i = 0; i < playerBoats.Count; i++)
            {
                if (playerBoats[i] == (Boat)BoatsListBox.SelectedItem)
                {
                    b = playerBoats[i];
                }
            }

            //prints the details of the boat
            ShowBoatDetails.Text = $"{b.BoatType} \nBoat Roation: {b.RotationDirection}° \nPlaced: {b.Placed}";


        }

        /// <summary>
        /// Opens the pause menu, and passes in all the grids and boat lists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_PauseMenu(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            window.Content = new PauseMenu(playerBoats, computerBoats, playerGrid, computerGrid);
        }

        //---------------------------------------------------------------------------------------------------------------
        
        //METHODS:

        /// <summary>
        /// Places a boat onto the player grid, both visually and in the array
        /// </summary>
        /// <param name="selectedBoat">
        /// The boat that has been currently selected to be placed (player boat or computer boat)
        /// </param>
        /// <param name="boatGrid">
        /// The inputted grid to the method. It will either be the player grid or the computer grid.
        /// </param>
        /// <param name="playerPlacing">
        /// Checks to see if the player is the one placing the boat onto a grid. If it is the computer it does the check for the computer
        /// </param>
        static void PlaceBoat(Boat selectedBoat, int rowPos, int colPos, int rotationDirection, bool playerPlacing, string[,] boatGrid)
        {
            //checks to see if the player has already placed the selected boat
            if (selectedBoat.Placed == "Placed!")
            {
                MessageBox.Show("You have already placed this boat", "Boat Placement Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //if check ahead fails (there is an error) then exit the method:
            else if (!CheckAhead(rowPos, colPos, selectedBoat, rotationDirection, playerPlacing, boatGrid))
            {
                return;
            }
            
            else
            {
                //counts the number of positions that have been placed. Once it = Length of the boat then the loop breaks
                int noPositionsPlaced = 0;
                switch (rotationDirection)
                {
                    case 0:

                        //bc the rotationDirection is 0, the boat positions will be places upwards from the input position. so the rowPos decreases
                        while (noPositionsPlaced < selectedBoat.Length)
                        {
                            //if the player is placing a boat then it adds the images as well.
                            if (playerPlacing)
                            {
                                //places the red square image (RedBackground.png) into the correct position
                                AddImageToGrid(rowPos, colPos, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowPlayerGrid);
                            }
                            //converts the selected position of a grid (player of computer) to "b" (boat)
                            boatGrid[rowPos, colPos] = "b";

                            //adds the location of the positon to the position for the boat
                            selectedBoat.boatPositions.Add((rowPos, colPos));

                            //decreases row position
                            rowPos--;

                            //increases the number of boat positions places
                            noPositionsPlaced++;




                        }
                        break;
                    case 90:
                        while (noPositionsPlaced < selectedBoat.Length)
                        {
                            if (playerPlacing)
                            {
                                //places the red square image (RedBackground.png) into the correct position
                                AddImageToGrid(rowPos, colPos, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowPlayerGrid);
                            }

                            //adds the position of a boat to the player grid
                            boatGrid[rowPos, colPos] = "b";

                            selectedBoat.boatPositions.Add((rowPos, colPos));

                            //increases column position
                            colPos++;

                            //increases the number of boat positions places
                            noPositionsPlaced++;




                        }
                        break;
                    case 180:
                        while (noPositionsPlaced < selectedBoat.Length)
                        {
                            if (playerPlacing)
                            {
                                //places the red square image (RedBackground.png) into the correct position
                                AddImageToGrid(rowPos, colPos, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowPlayerGrid);
                            }

                            boatGrid[rowPos, colPos] = "b";

                            selectedBoat.boatPositions.Add((rowPos, colPos));

                            //decreases column position
                            rowPos++;

                            //increases the number of boat positions places
                            noPositionsPlaced++;




                        }
                        break;
                    case 270:
                        while (noPositionsPlaced < selectedBoat.Length)
                        {
                            if (playerPlacing)
                            {
                                //places the red square image (RedBackground.png) into the correct position
                                AddImageToGrid(rowPos, colPos, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowPlayerGrid);
                            }

                            boatGrid[rowPos, colPos] = "b";


                            selectedBoat.boatPositions.Add((rowPos, colPos));

                            //decreases column position
                            colPos--;

                            //increases the number of boat positions places
                            noPositionsPlaced++;




                        }
                        break;
                }
            }

            if (playerPlacing)
            {
                //increases the total number of boats placed (only for the player)
                Boat.NoPlaced++;
                //will show in boat details that the boat has been placed
                selectedBoat.Placed = "Placed!";
            }
        }

        /// <summary>
        /// Checks the player or computer grid to see if a boat has already been placed in the way of another boat being placed
        /// </summary>
        /// <param name="playerPlacing">
        /// Checks to see if the player is the one placing the boat onto a grid. If it is the computer it does the check for the computer
        /// </param>
        /// <param name="boatGrid">
        /// The inputted grid to the method. It will either be the player grid or the computer grid.
        /// </param>
        /// <param name="selectedBoat">
        /// The boat that has been currently selected to be placed (player boat or computer boat)
        /// </param>
        /// <returns>
        /// Returns TRUE if OKAY, FALSE if ERROR
        /// </returns>
        static bool CheckAhead(int rowPos, int colPos, Boat selectedBoat, int rotationDirection, bool playerPlacing, string[,] boatGrid)
        {
            int noPositionsPlaced = 0;

            


            //uses the same method as the while loop in place boat
            switch (rotationDirection)
            {
                case 0:
                    //bc the rotationDirection is 0, the boat positions will be places upwards from the input position. so the rowPos decreases
                    while (noPositionsPlaced < selectedBoat.Length)
                    {

                        //checks to see if the player has gone out of bounds of the grid
                        if (rowPos < 0 || colPos < 0 || rowPos > 7 || colPos > 7)
                        {
                            //only shows this message and others if the player is placing a boat. 
                            if (playerPlacing)
                            {
                                MessageBox.Show("Your boat has gone out of bounds of the grid.", "Out of bounds", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        //if there is another boat in the position we are trying to place the new boat then throw an error
                        else if (boatGrid[rowPos, colPos] == "b")
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show($"Your {selectedBoat.BoatType} is going to collide with another boat, please place it elsewhere", "Boat Collision Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        //decreases row position
                        rowPos--;

                        //increases the number of boat positions places
                        noPositionsPlaced++;
                    }
                    break;
                case 90:
                    while (noPositionsPlaced < selectedBoat.Length)
                    {

                        if (rowPos < 0 || colPos < 0 || rowPos > 7 || colPos > 7)
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show("Your boat has gone out of bounds of the grid.", "Out of bounds", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        else if (boatGrid[rowPos, colPos] == "b")
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show($"Your {selectedBoat.BoatType} is going to collide with another boat, please place it elsewhere", "Boat Collision Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        //increases column position
                        colPos++;

                        //increases the number of boat positions places
                        noPositionsPlaced++;
                    }
                    break;
                case 180:
                    while (noPositionsPlaced < selectedBoat.Length)
                    {

                        if (rowPos < 0 || colPos < 0 || rowPos > 7 || colPos > 7)
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show("Your boat has gone out of bounds of the grid.", "Out of bounds", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        else if (boatGrid[rowPos, colPos] == "b")
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show($"Your {selectedBoat.BoatType} is going to collide with another boat, please place it elsewhere", "Boat Collision Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        //increases column position
                        rowPos++;

                        //increases the number of boat positions places
                        noPositionsPlaced++;
                    }
                    break;
                case 270:
                    while (noPositionsPlaced < selectedBoat.Length)
                    {

                        if (rowPos < 0 || colPos < 0 || rowPos > 7 || colPos > 7)
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show("Your boat has gone out of bounds of the grid.", "Out of bounds", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        else if (boatGrid[rowPos, colPos] == "b")
                        {
                            if (playerPlacing)
                            {
                                MessageBox.Show($"Your {selectedBoat.BoatType} is going to collide with another boat, please place it elsewhere", "Boat Collision Detected", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            return false;
                        }
                        //decreases column position
                        colPos--;

                        //increases the number of boat positions places
                        noPositionsPlaced++;
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Does various input validation checks to make sure that the user inputted correctly
        /// </summary>
        /// <returns>
        /// If input is correct then returns true, else false
        /// </returns>
        private bool ValidateInputPosition(string inputPosition)
        {
            //checks if null
            if (inputPosition.Length == 0)
            {
                MessageBox.Show("Nothing was input into the box", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            //checks the length of the input to see if it is too long.
            else if (inputPosition.Length > 2 || inputPosition.Length < 2)
            {
                MessageBox.Show("Your input positon was the wrong length", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }


            int rowPos = 0;
            int colPos = 0;
            try
            {
                //tries to convert to an int, if it fails it goes to the catch block
                rowPos = Convert.ToInt32(Convert.ToString(inputPosition[0]));
                colPos = Convert.ToInt32(Convert.ToString(inputPosition[1]));


                //checks if the inputs are within the accepted range
                if (rowPos < 1 || colPos < 1 || rowPos > 8 || colPos > 8)
                {
                    MessageBox.Show("One of your inputs was out of range for the grid", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("One of your inputs was not a number", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }

        /// <summary>
        ///places the red square image (RedBackground.png) into the correct position
        /// </summary>
        /// <param name="grid">
        /// The specified grid that we want the place the specified image (by uri) into
        /// </param>
        static void AddImageToGrid(int rowPos, int colPos, string uri, Grid grid)
        {
            //places the red square image (RedBackground.png) into the correct position
            Image im = new Image();
            ImageSource iS = new BitmapImage(new Uri(uri));
            im.Source = iS;
            grid.Children.Add(im);
            Grid.SetColumn(im, colPos);
            Grid.SetRow(im, rowPos);

            //adjusts the height of the image to almost perfectly fit the square (it's a bit annoying but whatever)
            im.Width = 25;
            im.Height = 25;
        }

        /// <summary>
        /// using the other place boat method, it places all of the computer's boats into random positons
        /// </summary>
        static void PlaceComputerBoats()
        {
            //used to create all the random positons and rotations of the boat
            Random rand = new Random();

            //checks to see if each random position and rotation has worked (used in the next while loop)
            bool pass;

            //used to pick random rotation directions from
            int[] rotationDirections = { 0, 90, 180 };
            foreach (Boat b in computerBoats)
            {
                pass = false;
                while (!pass)
                {
                    //random row and column
                    int rowPos = rand.Next(0, 8);
                    int colPos = rand.Next(0, 8);

                    //create random index to choose a rotation direction in the above array
                    int randRotDir = rand.Next(0, 3);

                    //the place boat method.
                    PlaceBoat(b, rowPos, colPos, rotationDirections[randRotDir], false, computerGrid);

                    //if the boat has no recorded positions then we know it didn't work so try again.
                    if (b.boatPositions.Count > 0)
                    {
                        //if it did work pass = true and try for the next boat
                        pass = true;
                    }
                }
            }
        }

        /// <summary>
        /// creates the grid of buttons that the player can use to shoot so that they don't have to be created manually
        /// </summary>
        static void CreateButtonGrid()
        {
            //creates the grid of buttons for the player to click to be able to shoot
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    string pos = string.Concat(Convert.ToString(row + 1), Convert.ToString(col + 1));

                    //create a button only if the player hasn't shot there yet (it is not a miss and not a hit)
                    if (computerGrid[row, col] != "m" && computerGrid[row, col] != "h")
                    {
                        CreateButton(pos, row, col, WindowShootingGrid);

                    }

                }
            }
        }

        /// <summary>
        /// thx google. Creates each button in the Shooting grid rather than having to do it manually
        /// </summary>
        static void CreateButton(string content, int row, int column, Grid grid)
        {
            Button button = new Button();
            button.Content = content;
            button.Click += ButtonClick_Shoot;
            grid.Children.Add(button);
            Grid.SetRow(button, row);
            Grid.SetColumn(button, column);



        }

        /// <summary>
        /// Deletes the boat position that was hit from the list of boat positions of the specified boat.
        /// </summary>
        /// <param name="gridPosition">
        /// The position that was hit on the grid
        /// </param>
        /// <param name="targetBoats">
        /// The user's or the computer's list of boats
        /// </param>
        /// <param name="isPlayerTurn">
        /// Is it the player's turn?
        /// </param>
        /// <param name="boatGrid">
        /// The user's or the computer's boat grid.
        /// </param>
        static void DeleteBoatOrBoatPosition((int row, int col) gridPosition, List<Boat> targetBoats, bool isPlayerTurn, string[,] boatGrid)
         {
            Boat selectedBoat = null;



            //find the boat located at that position
            for (int i = 0; i < targetBoats.Count; i++)
            {
                for (int j = 0; j < targetBoats[i].boatPositions.Count; j++)
                {
                    if (targetBoats[i].boatPositions[j] == gridPosition)
                    {
                        selectedBoat = targetBoats[i];
                        break;
                    }
                }
            }

            //make sure the boat selected isn't null (makes sure that a boat has actually been selected)
            if (selectedBoat != null)
            {
                //if it ISN'T the player's turn then we need to add the BidRedX image to the player's grid
                if (!isPlayerTurn)
                {
                    AddImageToGrid(gridPosition.row, gridPosition.col, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\BigRedX.jpeg", WindowPlayerGrid);

                }



                //checks to see that if we remove the final boat position, then the boat will be dead. In that case just remove the boat from the list because it's DEAD
                if (selectedBoat.boatPositions.Count - 1 == 0)
                {
                    targetBoats.Remove(selectedBoat);
                }
                else
                {
                    //loops through the boat positions for the selected boat
                    for (int i = 0; i < selectedBoat.boatPositions.Count; i++)
                    {
                        //checks if the boat position that we're at is the same as the once that has been selected
                        if (gridPosition == selectedBoat.boatPositions[i])
                        {
                            //remove that boat position from the list
                            selectedBoat.boatPositions.RemoveAt(i);
                            break;
                        }
                    }
                }
            }



            //changes the id of the position
            boatGrid[gridPosition.row, gridPosition.col] = "h";


            CheckWin(targetBoats, isPlayerTurn);

        }
        /// <summary>
        /// Computer chooses a random position to shoot at.
        /// </summary>
        static void ComputerShoot()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! i want this to work but the problem is it delays everything until all of the methods have been exited. Which delays the image being placed on the computer grid :/
            //stops the computer for 3 seconds to make it seem like the computer is choosing where to shoot
            //Thread.Sleep(3000);

            Random rand = new Random();

            int row = rand.Next(0, 8);
            int col = rand.Next(0, 8);

            //checks to see if the computer has already chosen a point on the playerGrid then it trys again
            while (playerGrid[row, col] == "h")
            {
                row = rand.Next(0, 8);
                col = rand.Next(0, 8);
            }

            //checks to see if the place it hit is empty
            if (playerGrid[row, col] == "e")
            {
                //if it's empty then it says that it has missed
                playerGrid[row, col] = "m";
            }
            else
            {
                //otherwise it calls this method.
                DeleteBoatOrBoatPosition((row, col), playerBoats, false, playerGrid);

            }

        }

        /// <summary>
        /// Loads a previous save from the LoatTextFile.txt
        /// </summary>
        static void LoadSave()
        {
            //!!!!!!!!!!!!!!!!!!!!!!!!! when you hand in the project you're going to need to sort out the file namee it won't work for sir
            string[] lines = File.ReadAllLines(@"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\bin\Debug\net7.0-windows\LoadTextFile.txt");

            //loops through all the lines
            for (int i = 0; i < lines.Length; i++)
            {
                //checks to see if an empty line has been left, if so skip over it
                if (lines[i] == null)
                {
                    continue;
                }
                else
                {
                    //checking for the lines in which the player grid is located
                    if (i < 8)
                    {
                        //places the first 8 lines (which represents the playerGrid, into the playerGrid variable)
                        for (int j = 0; j < 8; j++)
                        {
                            playerGrid[i, j] = Convert.ToString(lines[i][j]);
                        }
                    }
                    else if (i > 8 && i < 17)
                    {
                        //now do the same for the computerGrid
                        for (int j = 0; j < 8; j++)
                        {
                            //we have to do i - 9 becuase otherwise it will be out of range for the computerGrid
                            computerGrid[i - 9, j] = Convert.ToString(lines[i][j]);
                        }
                    }
                    //now beyond the range of both grids and into the saved boats
                    else
                    {
                        //checks to see if it is the range of lines in which the player boats was saved in
                        if (i > 16 && i < 22)
                        {
                            //splits up the lines into an array based on a space between words
                            string[] line = lines[i].Split(' ');

                            //check the boatType of each boat at the start of the line
                            //then it adds the saved position in the text file to the specified boatPositions list
                            switch (line[0])
                            {
                                //only put aircraft here bc we saved it as "Aircraft Carrier" which will split it up.
                                case "Aircraft":
                                    //starting j at 2 bc we need to include "Aircraft" and "Carrier" as two separate elements in the array (same reason we do Line.Length - 2)
                                    for (int j = 2; j < line.Length - 2; j += 2)
                                    {
                                        //converts the two numbers into ints and adds it to the specific boat
                                        playerBoats[0].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Battleship":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        playerBoats[1].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Cruiser":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        playerBoats[2].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Submarine":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        playerBoats[3].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Destroyer":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        playerBoats[4].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                            }
                        }
                        //now for the saved data of the computer boats
                        else if (i > 22)
                        {



                            //splits up the lines
                            string[] line = lines[i].Split(' ');

                            //check the boatType of each boat at the start of the line
                            switch (line[0])
                            {
                                //only put aircraft here bc we saved it as "Aircraft Carrier" which will split it up.
                                case "Aircraft":
                                    //starting j at 2 bc we need to include "Aircraft" and "Carrier" as two separate elements in the array (same reason we do Line.Length - 2)
                                    for (int j = 2; j < line.Length - 2; j += 2)
                                    {
                                        computerBoats[0].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Battleship":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        computerBoats[1].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Cruiser":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        computerBoats[2].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Submarine":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        computerBoats[3].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                                case "Destroyer":
                                    for (int j = 1; j < line.Length - 1; j += 2)
                                    {
                                        computerBoats[4].boatPositions.Add((Convert.ToInt32(line[j]), Convert.ToInt32(line[j + 1])));
                                    }
                                    break;
                            }
                        }
                    }
                }
            }


            //removes any boats that have nothing in them
            for (int i = 0; i < playerBoats.Count; i++)
            {
                if (playerBoats[i].boatPositions.Count == 0)
                {
                    playerBoats.Remove(playerBoats[i]);
                    i--;
                }
            }

            for (int i = 0; i < computerBoats.Count; i++)
            {
                if (computerBoats[i].boatPositions.Count == 0)
                {
                    computerBoats.Remove(computerBoats[i]);
                    i--;
                }
            }











            //adds the images to the shooting grid
            for (int i = 0; i < computerGrid.GetLength(0); i++)
            {
                for (int j = 0; j < computerGrid.GetLength(1); j++)
                {
                    if (computerGrid[i, j] == "h")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowShootingGrid);
                    }
                    else if (computerGrid[i, j] == "m")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\BlueBackground.png", WindowShootingGrid);

                    }

                }
            }

            //marks the locations of the player's boats on their grid
            for (int i = 0; i < playerGrid.GetLength(0); i++)
            {
                for (int j = 0; j < playerGrid.GetLength(1); j++)
                {
                    if (playerGrid[i, j] == "b")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\RedBackground.png", WindowPlayerGrid);
                    }
                    else if (playerGrid[i, j] == "h")
                    {
                        AddImageToGrid(i, j, @"D:\Alexander\Documents\CODE\c#\hills tasks\Battle Boats Project\Battle Boats\Images\BigRedX.jpeg", WindowPlayerGrid);

                    }

                }
            }

            //removes all the start game UI which would have been created
            WindowInputPositionBox.Visibility = Visibility.Collapsed;
            WindowPlaceBoatButton.Visibility = Visibility.Collapsed;
            WindowQuestion.Visibility = Visibility.Collapsed;
            WindowRotateBoatButton.Visibility = Visibility.Collapsed;
            WindowStartGameButton.Visibility = Visibility.Collapsed;





        }
        /// <summary>
        /// Checks to see who has won. then opens the winPage
        /// </summary>
        static void CheckWin(List<Boat> targetBoats, bool isPlayerTurn)
        {
            if (targetBoats.Count == 0)
            {
                if (isPlayerTurn)
                {
                    instance.Content = new WinPage("Player");
                }
                else
                {
                    instance.Content = new WinPage("Computer");

                }
            }
        }

    }
}