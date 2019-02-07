using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yahtzee_Game {

    /*
     *   This class acts as the UI for the Yahtzee Game.
     *   It holds and initialises all of the objects that are
     *   displayed on the UI.  
     *   It holds all of the logic related to UI changes.
     *   
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    public partial class Form1 : Form {

        private Label[] dice;
        private Button[] scoreButtons;
        private Label[] scoreTotals;
        private CheckBox[] checkBoxes;
        private Game game;

        //SIZE OF ARRAYS
        private const int NUMBER_OF_DICE_AND_CHECKBOXES = 5;
        private const int NUMBER_OF_BUTTONS = 16;
        private const int NUMBER_OF_SCORE_TOTAL_LABELS = 19;

        private const int DIE_ONE = 0;
        private const int DIE_TWO = 1;
        private const int DIE_THREE = 2;
        private const int DIE_FOUR = 3;
        private const int DIE_FIVE = 4;


        public Form1() {
            InitializeComponent();
            InitializeLabelsAndButtons();
            SetInitialFormSettings();

        }


        /// <summary>
        /// Initialises the arrays containing all of the UI labels and buttons.
        /// </summary>
        private void InitializeLabelsAndButtons() {
            dice = new Label[NUMBER_OF_DICE_AND_CHECKBOXES] {
                label1, label2, label3, label4, label5
            };

            //additional null positions added to use ScoreType Enum as array index
            scoreButtons = new Button[NUMBER_OF_BUTTONS] {
                button1, button2, button3, button4, button5, button6,
                null,null,null,
                button7, button8, button9, button10, button11, button12,
                button13
            };

            scoreTotals = new Label[NUMBER_OF_SCORE_TOTAL_LABELS] {
                scoreLabel1, scoreLabel2, scoreLabel3, scoreLabel4, scoreLabel5,
                scoreLabel6,
                subTotalScoreLabel, bonus63ScoreTotalLabel, upperTotalScoreLabel,
                scoreLabel7, scoreLabel8, scoreLabel9, scoreLabel10,
                scoreLabel11,scoreLabel12, scoreLabel13,
                yahtzeeBonusScoreLabel, lowerTotalScoreLabel, grandTotalScoreLabel

            };
            checkBoxes = new CheckBox[NUMBER_OF_DICE_AND_CHECKBOXES] {
                checkBox1, checkBox2, checkBox3, checkBox4, checkBox5
            };
        }

        /// <summary>
        /// Returns the Dice Label Array
        /// </summary>
        /// <returns>Returns the Dice Label Array</returns>
        public Label[] GetDice() {
            return dice;
        }

        /// <summary>
        /// Returns the Score Total Label Array
        /// </summary>
        /// <returns>Returns the Score Total Label Array</returns>
        public Label[] GetScoresTotals() {
            return scoreTotals;
        }

        /// <summary>
        /// Displays the Player's name in the playerLabel on the GUI.
        /// </summary>
        /// <param name="name">Name of the player</param>
        public void ShowPlayerName(string name) {
            playerLabel.Text = name;
        }

        /// <summary>
        /// Enables the usage of the Roll Button.
        /// </summary>
        public void EnableRollButton() {
            rollButton.Enabled = true;
        }

        /// <summary>
        /// Disables the usage of the Roll Button.
        /// </summary>
        public void DisableRollButton() {
            rollButton.Enabled = false;

        }
        /// <summary>
        /// Enables the usage of the Dice Checkboxes
        /// </summary>
        public void EnableCheckBoxes() {
            foreach (CheckBox checkBox in checkBoxes) {
                checkBox.Enabled = true;
            }
        }

        /// <summary>
        /// Disables and resets the checked status of the Dice Checkboxes.
        /// </summary>
        public void DisableAndClearCheckBoxes() {
            foreach (CheckBox checkBox in checkBoxes) {
                checkBox.Checked = false;
                checkBox.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the Score Button associated with the combo.
        /// </summary>
        /// <param name="combo">The score type that corresponds with the button to enable.</param>
        public void EnableScoreButton(ScoreType combo) {
            if (scoreButtons[(int)combo] != null) {
                scoreButtons[(int)combo].Enabled = true;
            }
        }

        /// <summary>
        /// Disables the usage of the Score Button related to the combo.
        /// </summary>
        /// <param name="combo">The score type that corresponds with the button to disable.</param>
        public void DisableScoreButton(ScoreType combo) {
            if (scoreButtons[(int)combo] != null) {
                scoreButtons[(int)combo].Enabled = false;
            }
        }

        /// <summary>
        /// Enables or disables all of the score buttons depending upon the input boolean.
        /// </summary>
        /// <param name="status">A boolean that will enable the score buttons if it is true.</param>
        private void SetAllScoreButtons(bool status) {
            //for each button, set the enabled status to the input
            foreach (Button button in scoreButtons) {
                if (button != null) {
                    button.Enabled = status;
                }
            }
        }

        /// <summary>
        /// Sets the initial conditions of the GUI when a new game has started.
        /// </summary>
        private void SetInitialFormSettings() {
            DisableAndClearCheckBoxes();
            DisableRollButton();
            SetAllScoreButtons(false);
            SetNumericPlayerCounter(false);
            saveToolStripMenuItem.Enabled = false;
        }


        /// <summary>
        /// Sets whether the numeric player counter is enabled or not.
        /// </summary>
        /// <param name="status">A boolean that will enable the numeric counter if it is true.</param>
        private void SetNumericPlayerCounter(bool status) {
            playerNumericCounter.Enabled = status;
        }

        /// <summary>
        /// Clears the die label on the Windows Form.
        /// </summary>
        private void ResetDieLabels() {
            foreach (Label dieLabel in dice) {
                dieLabel.Text = "";
            }
        }


        /// <summary>
        /// Checks the Dice Checkbox associated with an index value.
        /// </summary>
        /// <param name="index">Address of the Dice Checkbox to be checked.</param>
        public void CheckCheckBox(int index) {
            checkBoxes[index].Checked = true;
        }

        /// <summary>
        /// Displays a message to the GUI.
        /// </summary>
        /// <param name="message">A string containing the message.</param>
        public void ShowMessage(string message) {
            messageLabel.Text = message;
        }

        /// <summary>
        /// Shows the OK button
        /// </summary>
        public void ShowOkButton() {
            okButton.Visible = true;

        }

        /// <summary>
        /// Resets all arrays and containers and restarts/starts the Yahtzee game.
        /// </summary>
        public void StartNewGame() {
            game = new Game(this);
            EnableRollButton();
            ResetDieLabels();

            SetNumericPlayerCounter(true);
            playerBindingSource.DataSource = game.Players;

            loadToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = true;
        }

        /// <summary>
        /// Will hold or release a die depending upon the user's input.
        /// </summary>
        /// <param name="die">The number of the die that is having its status changed.</param>
        /// <param name="status">A boolean that will hold the die if true, else it will release it.</param>
        public void ChangeDieStatus(int die, bool status) {
            if (status) {
                game.HoldDie(die);
            } else {
                game.ReleaseDie(die);
            }
        }

        /// <summary>
        /// Updates the game's data grid view with the latest scores.
        /// </summary>
        private void UpdatePlayersDataGridView() {
            game.Players.ResetBindings();
        }


        private void newToolStripMenuItem_Click(object sender, EventArgs e) {
            StartNewGame();
            EnableRollButton();
        }

        private void rollButton_Click(object sender, EventArgs e) {
            game.RollDice();
            EnableCheckBoxes();
            SetNumericPlayerCounter(false);

        }

        private void okButton_Click(object sender, EventArgs e) {
            game.NextTurn();
            EnableRollButton();

            okButton.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Ones);
            UpdatePlayersDataGridView();
        }

        private void button2_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Twos);
            UpdatePlayersDataGridView();
        }

        private void button3_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Threes);
            UpdatePlayersDataGridView();
        }

        private void button4_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Fours);
            UpdatePlayersDataGridView();
        }

        private void button5_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Fives);
            UpdatePlayersDataGridView();
        }

        private void button6_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Sixes);
            UpdatePlayersDataGridView();
        }

        private void button7_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.ThreeOfAKind);
            UpdatePlayersDataGridView();

        }

        private void button8_Click(object sender, EventArgs e) {

            game.ScoreCombination(ScoreType.FourOfAKind);
            UpdatePlayersDataGridView();
        }

        private void button9_Click(object sender, EventArgs e) {

            game.ScoreCombination(ScoreType.FullHouse);
            UpdatePlayersDataGridView();

        }

        private void button10_Click(object sender, EventArgs e) {

            game.ScoreCombination(ScoreType.SmallStraight);
            UpdatePlayersDataGridView();

        }

        private void button11_Click(object sender, EventArgs e) {

            game.ScoreCombination(ScoreType.LargeStraight);
            UpdatePlayersDataGridView();

        }

        private void button12_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Chance);
            UpdatePlayersDataGridView();

        }

        private void button13_Click(object sender, EventArgs e) {
            game.ScoreCombination(ScoreType.Yahtzee);
            UpdatePlayersDataGridView();

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) {
            ChangeDieStatus(DIE_ONE, checkBox1.Checked);

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e) {
            ChangeDieStatus(DIE_TWO, checkBox2.Checked);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e) {
            ChangeDieStatus(DIE_THREE, checkBox3.Checked);
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e) {
            ChangeDieStatus(DIE_FOUR, checkBox4.Checked);
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e) {
            ChangeDieStatus(DIE_FIVE, checkBox5.Checked);
        }

        private void playerNumericCounter_ValueChanged(object sender, EventArgs e) {

            if (playerNumericCounter.Value < game.Players.Count) {
                Player lastPlayer = game.Players[game.Players.Count - 1];
                game.Players.Remove(lastPlayer);
            } else {
                game.Players.Add(new Player("Player " + playerNumericCounter.Value, scoreTotals));
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e) {
            game = Game.Load(this);
            playerBindingSource.DataSource = game.Players;
            UpdatePlayersDataGridView();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {

            game.Save();
        }
    }

}

