using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;

namespace Yahtzee_Game {
    /*
     *   This Class is an Abstract Class that is also a
     *   subclass of the Score Class.  
     *   This Class is used to represent a singular 
     *   dice combination.
     *
     *   A helper sort function is provided to sort an
     *   integer array.  
     *   
     *   Helper properties and methods that are used to check 
     *   whether a dice roll has resulted in a Yahtzee
     *   are also provided.
     *   
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    abstract class Combination : Score {
        protected bool isYahtzee;
        protected int yahtzeeNumber;

        //DIE VALUES
        private const int FIRST_DIE = 0;
        private const int SECOND_DIE = 1;

        public Combination(Label label) : base(label) {

        }

        public abstract void CalculateScore(int[] scores);


        /// <summary>
        /// Sorts an integer array in ascending order.
        /// </summary>
        /// <param name="array">The array to be sorted.</param>
        /// <returns>Returns the sorted integer array.</returns>
        public int[] Sort(int[] array) {
            int temp; //temporary integer storage


            for (int i = 0; i < array.Length; i++) {
                //from every element since i, to the length of the array
                for (int j = i + 1; j < array.Length; j++) {

                    // if element at j is less than the element at i
                    // switch the position of the two values.
                    if (array[j] < array[i]) {

                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// A property that gets and sets whether a boolean that
        /// relates to whether a roll of the dice results in a Yahtzee.
        /// </summary>
        public bool IsYahtzee {
            get {
                return isYahtzee;
            }

            set {
                isYahtzee = value;
            }
        }

        /// <summary>
        /// A property that gets and sets an integer to the
        /// face value of a Yahtzee roll.
        /// </summary>
        public int YahtzeeNumber {
            get {
                return yahtzeeNumber;
            }
            set {
                yahtzeeNumber = value;
            }
        }

        /// <summary>
        /// Checks whether a roll of the dice has resulted
        /// in a yahtzee combination.
        /// It will also set the yahtzeeNumber to the face value
        /// of the first die.
        /// </summary>
        /// <param name="scores">An integer array containing the face values of the die.</param>
        public void CheckForYahtzee(int[] scores) {
            IsYahtzee = true; //initially set isYahtzee to true

            //if at any point a die is not equal to the first die then
            //set the isYahtzee boolean to false
            for (int i = SECOND_DIE; i < scores.Length; i++) {
                if (scores[FIRST_DIE] != scores[i]) {
                    IsYahtzee = false;
                }
            }
            YahtzeeNumber = scores[FIRST_DIE];
        }
    }
}
