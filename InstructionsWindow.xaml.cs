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
    /// Interaction logic for InstructionsWindow.xaml
    /// </summary>
    public partial class InstructionsWindow : UserControl
    {


        static string from = string.Empty;
        public InstructionsWindow(string From)
        {
            InitializeComponent();

            from = From;

            //writes out the paragraph inside the textblock so that it is an actual paragraph.
            InstructionsParagraph.Text =
"Battle Boats is a turn-based strategy game where players eliminate their opponent's fleet of boats by ‘firing’ " +
"at a location on a grid in an attempt to sink them. The first player to sink all of their opponents’ battle boats is " +
"declared the winner. You will be given two 8x8 grids. On one of them, you strategically place your boatsinto position so that " +
"the enemy can't guess where they are. The other grid is usedfor you to guess where the enemy's boats are and to fire a missile at them." +
"Once all of the enemy's ships have been sunk you win!\n\n Placing the boats: \n1.  Select a boat from the list of boats in the centre of " +
"the screen\n2.  Input the position you wish to place your boat in the text box under your grid: \nInput in the form [rowColumn] e.g 87 is" +
" row 8 column 7\n3.  Click the rotate button to rotate your boat by 90 degrees.\n4.  Click place to place your boat into the specifed positon " +
"with the specified rotation.\nNOTE 1: the postion that you place your boat starts at the bottom of the boat. \nNOTE 2: Both the columns and rows of " +
"your grid that you place your boats start at 1 \n5.  Press Start Game\nFrom there " +
"click the buttons on the left grid to guess where the computer's boats might be located and try to win!";
        }
        /// <summary>
        /// Sends you back to which ever window you came from previously NOTE NOT DONE
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonClick_Back(object sender, RoutedEventArgs e)
        {
            //What i want:
            //  A global variable that says whether you came from the MainMenu or the pause menu

            if(from == "Main")
            {
                Window window = Window.GetWindow(this);

                //NOTE do i need to create a new MainMenu every time or can i call an old one that has already been created???
                window.Content = new MainMenu();
            }
            else if(from == "Pause")
            {
                Window window = Window.GetWindow(this);
                window.Content = new PauseMenu();
            }

            
        }
    }
}
