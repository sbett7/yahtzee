using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Yahtzee_Game {
    /*
     *   This is an Abstract Class that represents a single
     *   scoring combination in the Yahtzee game.  It hosts 
     *   a series of properties and functions that relate to
     *   the scoring rules of Yahtzee, as well as a label for 
     *   the specific score that it represents.
     *
     *   This Class will be used to store the achieved score
     *   for the specific scoring combination as well as displaying
     *   that score to the user interface.
     *   
     *   A boolean to represent whether the scoring combination has
     *   been performed is also provided.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    abstract class Score {

        private int points;
        [NonSerialized]
        private Label label;
        protected bool done;

        public Score(Label label) {
            this.label = label;
        }

        /// <summary>
        /// A property that gets and sets the points of a scoring
        /// combination.
        /// </summary>
        public int Points {
            get {
                return points;
            }
            set {
                points = value;
            }

        }

        /// <summary>
        /// A property that will get a boolean related to whether the
        /// scoring combination has been completed.
        /// </summary>
        public bool Done {
            get {
                return done;
            }

        }

        /// <summary>
        /// Presents the achieved score for the score combination
        /// to the screen.  If the scoring combination has not been 
        /// done then the label will be cleared.
        /// </summary>
        public void ShowScore() {
            if (points == 0 && !done) {
                label.Text = "";
            } else {
                label.Text = points.ToString();
            }
        }

        /// <summary>
        /// Loads the specific score label for a previous game.
        /// </summary>
        /// <param name="label">The label that is associated with
        /// the specific scoring combination.</param>
        public void Load(Label label) {
            this.label = label;
        }
    }
}
