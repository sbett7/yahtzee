using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {
    /*
     *   This Class is used to represent a player in the Yahtzee game.
     *   A player has their own specific scores and name.  This Class
     *   stores the player's name and scores.  A player's scores are also
     *   score from this Class.
     *   The Player's grand total score is also kept here.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */


    [Serializable]
    public class Player {
        private string name;
        private int combinationsToDo;
        private Score[] scores;
        private int grandTotal;
        

        private const int NUMBER_OF_COMBINATIONS = 13;
        private const int NUMBER_OF_SCORES = 19;
        private const int UPPER_SECTION_BONUS_POINTS = 35;
        private const int REQUIRED_SCORE_FOR_BONUS = 63;
        private const int YAHTZEE_BONUS = 100;

        private const int NUMBER_TO_ADDRESS = 1;

        public Player(string name, Label[] labels) {
            //set the name of the player and initialise the scores and combinations variables.
            this.name = name;
            scores = new Score[NUMBER_OF_SCORES];
            combinationsToDo = NUMBER_OF_COMBINATIONS;

            // for every label, set the scoretype to its position in the array
            if (labels != null) {
                for (int i = 0; i < labels.Length; i++) {
                    InitialiseScoreLabels((ScoreType)i, labels[i]);
                }
            }
        }

        /// <summary>
        /// Initialises each score label with their specific ScoreType.
        /// </summary>
        /// <param name="score">The ScoreType that is to be assigned.</param>
        /// <param name="label">The Label of the ScoreType that is to be used.</param>
        private void InitialiseScoreLabels(ScoreType score, Label label) {
            switch (score) {
                case ScoreType.Ones:
                    scores[(int)score] = new CountingCombination(score, label);
                    break;
                case ScoreType.Twos:
                    scores[(int)score] = new CountingCombination(score, label);
                    break;
                case ScoreType.Threes:
                    scores[(int)score] = new CountingCombination(score, label);
                    break;
                case ScoreType.Fours:
                    scores[(int)score] = new CountingCombination(score, label);
                    break;
                case ScoreType.Fives:
                    scores[(int)score] = new CountingCombination(score, label);
                    break;
                case ScoreType.Sixes:
                    scores[(int)score] = new CountingCombination(score, label);
                    break;
                case ScoreType.SubTotal:
                    scores[(int)score] = new BonusOrTotal(label);
                    break;
                case ScoreType.BonusFor63Plus:
                    scores[(int)score] = new BonusOrTotal(label);
                    break;
                case ScoreType.SectionATotal:
                    scores[(int)score] = new BonusOrTotal(label);
                    break;
                case ScoreType.ThreeOfAKind:
                    scores[(int)score] = new TotalOfDice(score, label);
                    break;
                case ScoreType.FourOfAKind:
                    scores[(int)score] = new TotalOfDice(score, label);
                    break;
                case ScoreType.FullHouse:
                    scores[(int)score] = new FixedScore(score, label);
                    break;
                case ScoreType.SmallStraight:
                    scores[(int)score] = new FixedScore(score, label);
                    break;
                case ScoreType.LargeStraight:
                    scores[(int)score] = new FixedScore(score, label);
                    break;
                case ScoreType.Chance:
                    scores[(int)score] = new TotalOfDice(score, label);
                    break;
                case ScoreType.Yahtzee:
                    scores[(int)score] = new FixedScore(score, label);
                    break;
                case ScoreType.YahtzeeBonus:
                    scores[(int)score] = new BonusOrTotal(label);
                    break;
                case ScoreType.SectionBTotal:
                    scores[(int)score] = new BonusOrTotal(label);
                    break;
                case ScoreType.GrandTotal:
                    scores[(int)score] = new BonusOrTotal(label);
                    break;
            }
        }

        /// <summary>
        /// Property that can set and get the name of a player.
        /// </summary>
        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        /// <summary>
        /// Determines which score calculation must be performed for the type of ScoreType
        /// and passes the face values of the dice to be used in the calculation.
        /// </summary>
        /// <param name="score">The ScoreType that is to be calculated.</param>
        /// <param name="dieFaceValues">An integer array that contains the values of each die.</param>
        public void ScoreCombination(ScoreType score, int[] dieFaceValues) {
            switch (score) {
                case ScoreType.Ones:
                    CalculateCountingCombination(score, dieFaceValues);
                    break;
                case ScoreType.Twos:
                    CalculateCountingCombination(score, dieFaceValues);
                    break;
                case ScoreType.Threes:
                    CalculateCountingCombination(score, dieFaceValues);
                    break;
                case ScoreType.Fours:
                    CalculateCountingCombination(score, dieFaceValues);
                    break;
                case ScoreType.Fives:
                    CalculateCountingCombination(score, dieFaceValues);
                    break;
                case ScoreType.Sixes:
                    CalculateCountingCombination(score, dieFaceValues);
                    break;
                case ScoreType.ThreeOfAKind:
                    CalculateTotalOfDiceCombination(score, dieFaceValues);
                    break;
                case ScoreType.FourOfAKind:
                    CalculateTotalOfDiceCombination(score, dieFaceValues);
                    break;
                case ScoreType.FullHouse:
                    CalculatingFixedCombination(score, dieFaceValues);
                    break;
                case ScoreType.SmallStraight:
                    CalculatingFixedCombination(score, dieFaceValues);
                    break;
                case ScoreType.LargeStraight:
                    CalculatingFixedCombination(score, dieFaceValues);
                    break;
                case ScoreType.Chance:
                    CalculateTotalOfDiceCombination(score, dieFaceValues);
                    break;
                case ScoreType.Yahtzee:
                    CalculatingFixedCombination(score, dieFaceValues);
                    break;
            }
            //decrease number of combinations to do and show scores.
            combinationsToDo--;
            CalculateScoreTotals();
        }

        /// <summary>
        /// Calculates the counting combination score for that specific ScoreType.
        /// </summary>
        /// <param name="score">The counting combination ScoreType.</param>
        /// <param name="dieValues">An integer array with all of the values from the die.</param>
        private void CalculateCountingCombination(ScoreType score, int[] dieValues) {
            //cast the specific score element as a CountingCombination
            CountingCombination combination = (CountingCombination) scores[(int) score];



            //if the roll was a yahtzee and the yahtzee combination has been done and 
            //is not 0, add a yahtzee bonus to the yahtzee bonus score
            combination.CheckForYahtzee(dieValues);
            if (combination.IsYahtzee && scores[(int)ScoreType.Yahtzee].Points != 0
                && scores[(int)ScoreType.Yahtzee].Done) {
                scores[(int)ScoreType.YahtzeeBonus].Points += YAHTZEE_BONUS;
            }

            combination.CalculateScore(dieValues);

            
        }

        /// <summary>
        /// Calculates the fixed combination score for that specific ScoreType.
        /// </summary>
        /// <param name="score">The fixed combination ScoreType.</param>
        /// <param name="dieValues">An integer array with all of the values from the die.</param>
        private void CalculatingFixedCombination(ScoreType score, int[] dieValues) {
            //cast the specific score element as a FixedScore
            FixedScore combination = (FixedScore)scores[(int)score];

            combination.CalculateScore(dieValues);

            //if the roll was a yahtzee and the yahtzee combination has been done and 
            //is not 0, add a yahtzee bonus to the yahtzee bonus score
            combination.CheckForYahtzee(dieValues);
            if (combination.IsYahtzee && scores[(int)ScoreType.Yahtzee].Points != 0
                && scores[(int)ScoreType.Yahtzee].Done && score != ScoreType.Yahtzee) {
                scores[(int)ScoreType.YahtzeeBonus].Points += YAHTZEE_BONUS;
            }
            //If the roll was a yahtzee and the related upper section score has been done
            //perform the yahtzee joker rule
            if (combination.IsYahtzee && scores[combination.YahtzeeNumber - NUMBER_TO_ADDRESS].Done &&
                scores[(int)ScoreType.Yahtzee].Done) {
                combination.PlayYahtzeeJoker();
            }  
        }

        /// <summary>
        /// Calculates the total of dice combination score for that specific ScoreType.
        /// </summary>
        /// <param name="score">The total of dice combination ScoreType.</param>
        /// <param name="dieValues">An integer array with all of the values from the die.</param>
        private void CalculateTotalOfDiceCombination(ScoreType score, int[] dieValues) {
            //cast the specific score element as a FixedScore
            TotalOfDice combination = (TotalOfDice)scores[(int)score];

            combination.CalculateScore(dieValues);

            //if the roll was a yahtzee and the yahtzee combination has been done and 
            //is not 0, add a yahtzee bonus to the yahtzee bonus score
            combination.CheckForYahtzee(dieValues);
            if (combination.IsYahtzee && scores[(int)ScoreType.Yahtzee].Points != 0
                && scores[(int)ScoreType.Yahtzee].Done) {
                scores[(int)ScoreType.YahtzeeBonus].Points += YAHTZEE_BONUS;
            }

            //if the combination was and if the related upper section score was done,
            // perform yahtzee joker rule
            if (combination.IsYahtzee && (!scores[combination.YahtzeeNumber - NUMBER_TO_ADDRESS].Done ||
                !scores[(int)ScoreType.Yahtzee].Done)) {
                combination.Points = 0;

            }
        }


        /// <summary>
        /// Calculates the total scores.
        /// </summary>
        private void CalculateScoreTotals() {
            //get the values of each total score label
            SetSubTotal();
            SetBonusFor63Plus();
            SetUpperTotal();
            SetLowerTotal();
            SetGrandTotal();

        }

        /// <summary>
        /// calculates the subtotal score.
        /// </summary>
        private void SetSubTotal() {
            int summedScore = 0;
            for (int i = 0; i <= (int) ScoreType.Sixes; i++) {
                summedScore += scores[i].Points;
            }

            scores[(int) ScoreType.SubTotal].Points = summedScore;
        }

        /// <summary>
        /// sets the Bonus Upper Total Score if the subtotal score is greater than 63.
        /// </summary>
        private void SetBonusFor63Plus() {
            if (scores[(int) ScoreType.SubTotal].Points >= REQUIRED_SCORE_FOR_BONUS) {
                scores[(int)ScoreType.BonusFor63Plus].Points = UPPER_SECTION_BONUS_POINTS;
            } else {
                scores[(int) ScoreType.BonusFor63Plus].Points = 0;
            }
        }

        /// <summary>
        /// Calculates the Lower Total Score.
        /// </summary>
        private void SetLowerTotal() {
            int summedScore = 0;
            for (int i = (int)ScoreType.ThreeOfAKind; i <= (int)ScoreType.YahtzeeBonus; i++) {
                summedScore += scores[i].Points;
            }

            scores[(int)ScoreType.SectionBTotal].Points = summedScore;
        }

        /// <summary>
        /// Calculates the Upper Total Score.
        /// </summary>
        private void SetUpperTotal() {
            int subTotal = scores[(int)ScoreType.SubTotal].Points;
            int bonus = scores[(int) ScoreType.BonusFor63Plus].Points;
            scores[(int)ScoreType.SectionATotal].Points = subTotal + bonus;
        }


        /// <summary>
        /// Calculates the Grand Total Score based off of the Upper and Lower Total Scores.
        /// </summary>
        private void SetGrandTotal() {
            //get upper and lower totals
            int upperTotal = scores[(int) ScoreType.SectionATotal].Points;
            int lowerTotal = scores[(int)ScoreType.SectionBTotal].Points;

            // grand total is equal to upper total + lower total.
            scores[(int) ScoreType.GrandTotal].Points = upperTotal + lowerTotal;
            GrandTotal = scores[(int) ScoreType.GrandTotal].Points;
        }



        /// <summary>
        /// Property setter and getter for the Grand Total.
        /// </summary>
        public int GrandTotal {
            get {
                return grandTotal;
            }
            set {
                grandTotal = value;
            }
        }

        /// <summary>
        /// Checks if the 
        /// </summary>
        /// <param name="scoreType"></param>
        /// <returns></returns>
        public bool IsAvailable(ScoreType scoreType) {

            //return false is the score has been completed else return true
            if (scores[(int) scoreType].Done) {
                return false;
            } else {
                return true;
            }
        }

        /// <summary>
        /// Shows the scores of each score label.
        /// </summary>
        public void ShowScores() {
            //for every scoretype show the score
            for (ScoreType i = ScoreType.Ones; i <= ScoreType.GrandTotal; i++) {
                scores[(int)i].ShowScore();
            }

        }


        /// <summary>
        /// Checks if the Player has completed all of the combinations.
        /// </summary>
        /// <returns></returns>
        public bool IsFinished() {
            // if the combinations to do is zero return true.
            if (combinationsToDo == 0) {
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Loads the labels from a previous game to the scoreTotals array.
        /// </summary>
        /// <param name="scoreTotals">A Label array for the score labels.</param>
        public void Load(Label[] scoreTotals) {
            for (int i = 0; i < scores.Length; i++) {
                scores[i].Load(scoreTotals[i]);
            }
        }
    }
}

