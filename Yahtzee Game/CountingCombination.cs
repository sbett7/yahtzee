using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
    /*
     *   
    *   This Class is a Subclass of the Combination Class.
     *   It represents one of the six counting combinations:
     *   
     *   Ones
     *   Twos
     *   Threes
     *   Fours   
     *   Fives
     *   Sixes
     *
     *   This Class is used to find the number of repeating values
     *   of the specific score combination and add them together to
     *   get the overall score for that combination.
     *
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    class CountingCombination : Combination {
        private int dieValue;

        //CONVERSION CONSTANT
        private const int CONVERT_FROM_ADDRESS_TO_NUMBER = 1;

        public CountingCombination(ScoreType score, Label label) : base(label) {
            dieValue = (int)score + CONVERT_FROM_ADDRESS_TO_NUMBER;
        }


        /// <summary>
        /// Calculates the score for a counting combination.
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        public override void CalculateScore(int[] scores) {
            int numberOfScores = 0;

            //set done to true to indicate the combination has been performed
            done = true;
            //for each score value, check if it is equal to die specific die value
            //and increase counter for the number of found scores of that type
            foreach (int score in scores) {
                if (score == dieValue) {
                    numberOfScores++;

                }
            }
            //points is equal to the number of found scores by the die value
            Points = dieValue * numberOfScores;
        }



    }
}
