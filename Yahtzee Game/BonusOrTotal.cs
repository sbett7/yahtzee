using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Yahtzee_Game {
    /*
     *   This Class is a Subclass of the Combination Class.
     *   It represents one of the five total scoring labels:
     *   
     *   Subtotal Score 
     *   Bonus For 63+ Score
     *   Upper Total Score
     *   Yahtzee Bonus Score
     *   Lower Total Score 
     *   Grand Total Score
     *
     *   This class is used to categorise the Score Totals.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    [Serializable]
    class BonusOrTotal : Score {

        public BonusOrTotal(Label label) : base(label) {

        }
    }
}
