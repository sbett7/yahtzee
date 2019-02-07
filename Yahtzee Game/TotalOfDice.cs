using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
    /*
     *   This Class is a Subclass of the Combination Class.
     *   It represents one of the three scoring combinations:
     *   
     *   Three of a Kind
     *   Four of a Kind
     *   Chance
     *   
     *   This Class is used to determine whether the criteria
     *   for one of the three combinations has been met and sets the
     *   score as the sum of all the die face values.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    class TotalOfDice : Combination {

        private int numberOfOneKind;

        //Combination Types
        private const int THREE_OF_A_KIND = 3;
        private const int FOUR_OF_A_KIND = 4;
        private const int CHANCE = 0;

        public TotalOfDice(ScoreType score, Label label) : base(label) {
            numberOfOneKind = GetNumberOfOneKindFromScoreEnum(score);
        }

        /// <summary>
        /// Calculates the score for a total of dice combination.
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        public override void CalculateScore(int[] scores) {
            scores = Sort(scores);
            int numberOfRepeats = 0;

            // for every die check if there are the required number
            // of repeating values, do this for every die.
            for (int i = 0; i < scores.Length; i++) {
                numberOfRepeats = 0;

                for (int j = 0; j < scores.Length; j++) {
                    if (scores[i] == scores[j]) {
                        numberOfRepeats++;
                    }
                    // if there are sum the scores, or if the score type 
                    // is chance just sum the scores.
                    if (numberOfRepeats == numberOfOneKind ||
                        numberOfOneKind == CHANCE) {
                        Points = scores.Sum();
                    }
                }
            }
                //set done to be true to show that combination has been completed.
                done = true;
        }

        /// <summary>
        /// Gets the number of repeating values that are required.
        /// </summary>
        /// <param name="score">The score combination for this combination.</param>
        /// <returns>Returns the number of repeating values that are required.</returns>
        private int GetNumberOfOneKindFromScoreEnum(ScoreType score) {
            switch (score) {
                case ScoreType.ThreeOfAKind:
                    return THREE_OF_A_KIND;
                case ScoreType.FourOfAKind:
                    return FOUR_OF_A_KIND;
                default:

                    return CHANCE;
            }
        }
    }
}
