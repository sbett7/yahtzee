using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
    /*
     *   This Class is a Subclass of the Combination Class.
     *   It represents one of the four scoring combinations:
     *   
     *   Full House
     *   Small Straight
     *   Large Straight
     *   Yahtzee
     *   
     *   This Class is used to determine whether the criteria
     *   for one of the three combinations has been met and sets the
     *   points to a fixed score that is dependent upon the combination.
     *     
     *   A method has been included to assign the fixed scores for the
     *   combination for the Yahtzee Joker rule.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    class FixedScore : Combination {

        private ScoreType scoreType;

        //SCORE VALUES OF COMBINATIONS
        private const int FULL_HOUSE_SCORE = 25;
        private const int SMALL_STRAIGHT_SCORE = 30;
        private const int LARGE_STRAIGHT_SCORE = 40;
        private const int YAHTZEE_SCORE = 50;

        //DIES IN ARRAY
        private const int DIE_1 = 0;
        private const int DIE_2 = 1;
        private const int DIE_3 = 2;
        private const int DIE_4 = 3;
        private const int DIE_5 = 4;

        //ITERATOR FOR CALCULATING SCORE
        private const int ITERATOR = 1;


        public FixedScore(ScoreType score, Label label) : base(label) {
            scoreType = score;
        }

        /// <summary>
        /// Performs the fixed scoring calculation depending upon the score type.
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        public override void CalculateScore(int[] scores) {
            bool GotScore = false;

            scores = Sort(scores);

            //set done to be true to show that combination has been completed.
            done = true;

            switch (scoreType) {
                case ScoreType.FullHouse:
                    FullHouse(scores);

                    break;
                case ScoreType.SmallStraight:
                    GotScore = SmallStraight(scores);
                    // check if conditions have been met and assign score.
                    if (GotScore) {
                        Points = SMALL_STRAIGHT_SCORE;
                    }
                    break;
                case ScoreType.LargeStraight:
                    GotScore = LargeStraight(scores);
                    // check if conditions have been met and assign score.
                    if (GotScore) {
                        Points = LARGE_STRAIGHT_SCORE;
                    }
                    break;

                default:
                    GotScore = Yahtzee(scores);
                    // check if conditions have been met and assign score.
                    if (GotScore) {
                        Points = YAHTZEE_SCORE;
                    }
                    break;
            }

        }

        /// <summary>
        /// Performs the scoring calculation for the Full House combination.
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        private void FullHouse(int[] scores) {
            //case 1: die 1 = die 2 = die 3
            if (scores[DIE_1] == scores[DIE_2] && scores[DIE_2] == scores[DIE_3]) {
                // if die 4 == die 5 set points to full house score else set to 0
                Points = scores[DIE_4] == scores[DIE_5] ? FULL_HOUSE_SCORE : 0;

                //case 2: die 3 == die 4 == die 5
            } else if (scores[DIE_3] == scores[DIE_4] && scores[DIE_4] == scores[DIE_5]) {
                // if die 1 == die 2 set points to full house score else set to 0
                Points = scores[DIE_1] == scores[DIE_2] ? FULL_HOUSE_SCORE : 0;

                // case 3: conditions not met so points is 0
            } else {
                Points = 0;
            }
        }

        /// <summary>
        /// Performs the scoring calculation for the Small Straight combination.
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        /// <returns>Returns a boolean to indicate whether the scoring
        ///  conditions have been met.</returns>
        private bool SmallStraight(int[] scores) {


            int numberOfMatches = 0;
            //for every iteration to the fourth die in the scores array
            for (int i = 0; i < DIE_5; i++) {

                // case 1: current score + 1 is equal to next score
                // increase criteria iterator.
                if (scores[i] + ITERATOR == scores[i + ITERATOR]) {
                    numberOfMatches++;

                  //case 2: scores are same as each other.
                } else if (scores[i] == scores[i + ITERATOR]) {
                    continue;
                    //case 3: current score + 1 does not equal next score 
                    // and i is not at the fourth die's position
                } else if (scores[i] + ITERATOR != scores[i + ITERATOR] && i != DIE_4) {
                    numberOfMatches = 0;
                }
                // if there are at least three matches the conditions
                // have been met
                if (numberOfMatches == 3) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Performs the scoring calculation for the Large Straight combination.
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        /// <returns>Returns a boolean to indicate whether the scoring
        ///  conditions have been met.</returns>
        private bool LargeStraight(int[] scores) {
            // for every iteration up to the fourth die in scores array.
            for (int i = 0; i < DIE_5; i++) {

                // if the current score + 1 is not equal to the next score
                // return false
                if (scores[i] + ITERATOR != scores[i + ITERATOR]) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Performs the scoring calculation for the Yahtzee combination. 
        /// </summary>
        /// <param name="scores">An array with the face values of each
        /// die.</param>
        /// <returns>Returns a boolean to indicate whether the scoring
        ///  conditions have been met.</returns>
        private bool Yahtzee(int[] scores) {
            // if one die is not equal to the first die return false
            for (int i = DIE_2; i < scores.Length; i++) {
                if (scores[DIE_1] != scores[i]) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Sets the score of the combination when the 
        /// Yahtzee Joker condition is met.
        /// </summary>
        public void PlayYahtzeeJoker() {
            switch (scoreType) {
                case ScoreType.FullHouse:
                    Points = FULL_HOUSE_SCORE;
                    break;
                case ScoreType.SmallStraight:
                    Points = SMALL_STRAIGHT_SCORE;
                    break;
                case ScoreType.LargeStraight:
                    Points = LARGE_STRAIGHT_SCORE;
                    break;
            }
        }

    }
}
