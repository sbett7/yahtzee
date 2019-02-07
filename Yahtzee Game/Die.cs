using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
    /*
     *   This Class represents a single die in the Yahtzee game.
     *   
     *   This class can simulate the rolling of a die and set whether
     *   or not the die is active.  The face value of the die is displayed
     *   on the UI.
     *   
     *   A debugging tool to test the different scoring combinations has also been included.
     *   This will make use of a text file with all of the die rolls.
     *   Please set DEBUG to true to make use of this debugging method.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    public class Die {
        private int faceValue;
        private bool active;

        [NonSerialized]
        private Label label;

        private static Random random;

        [NonSerialized]
        private static string rollFileName = Game.defaultPath + "\\basictestrolls.txt";
        private static StreamReader rollFile = new StreamReader(rollFileName);

        private static bool DEBUG = true;

        //MAXIMUM VALUES OF THE DIE
        private const int MAX_NUMBER = 7;
        private const int MIN_NUMBER = 1;

        public Die(Label diceLabel) {
            label = diceLabel;
            random = new Random();
            Active = true;
        }

        /// <summary>
        /// Gets the simulated face value of the die.
        /// </summary>
        /// <returns>Returns a number corresponding to a side of a six-sided die.</returns>
        public int GetFaceValue() {
            return faceValue;
        }

        /// <summary>
        /// A property that can get and set whether the die object is active or not.
        /// </summary>
        public bool Active {
            get {
                return active;
            }
            set {

                active = value;
            }
        }

        /// <summary>
        /// Simulates the die rolling and gets a number to a side of a six-sided die.
        /// </summary>
        public void Roll() {

            if (!DEBUG) {
                //get next face value and show on the label
                faceValue = random.Next(MIN_NUMBER, MAX_NUMBER);
                label.Text = faceValue.ToString();
                label.Refresh();
            } else {
                //get next die value from debug test file
                faceValue = int.Parse(rollFile.ReadLine());
                label.Text = faceValue.ToString();
                label.Refresh();
            }

        }

        /// <summary>
        /// Loads the face value of the die and presents it on the UI.
        /// </summary>
        /// <param name="label">The label that is related to the die object.</param>
        public void Load(Label label) {
            this.label = label;
            if (faceValue == 0) {
                label.Text = string.Empty;
            } else {
                label.Text = faceValue.ToString();
            }
        }

    }
}