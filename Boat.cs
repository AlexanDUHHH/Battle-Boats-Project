using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battle_Boats
{
    //class needs to be public so that i can pass it between windows for saving and loading a file later
    public class Boat
    {
        //Counts the number of boats that have already been placed. Used to check that the user has inputted all the boats
        public static int NoPlaced = 0;


        //The name of the type of boat (needs to be public so that it can be accessed to bind to the listbox)  :(
        public string BoatType { get; set; }

        //the number of squares that the boat occupies
        public int Length { get; set; }

        //Records the rotation of the boat (e.g 0°, 90° etc)
        private int rotationDirection;

        //Rotation direction for each ship so that i don't need to worry about it in the main code
        public int RotationDirection
        {
            get
            {
                return rotationDirection;
            }
            set
            {
                //we'll be adding 90 to it every time so if it gets to 360, just return it back to 0
                if (value == 360)
                {
                    rotationDirection = 0;
                }
                else
                {
                    rotationDirection = value;
                }
            }
        }


        public string Placed = "Not Placed";

        //A list to store all the grid locations of each boat
        public List<(int row, int col)> boatPositions = new List<(int row, int col)> ();


        //will be used for the data binding when displaying the information of the ships. It needs to be a
        //string to be able to be displayed
        public string DisplayLength { get; set; }

        public Boat(string BoatType, int Length)
        {
            //sets the boat type and the length when the boat has been created
            this.BoatType = BoatType;
            this.Length = Length;
            this.DisplayLength = Convert.ToString(Length);
            this.rotationDirection = 0;
        }
    }
}
