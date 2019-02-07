using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace Yahtzee_Game {
    /*
     *   This Class represents a game of Yahtzee.
     *   All of the main game logic is performed in this class.
     *   The game will simulate turns where players will roll the
     *   die and choose their scores.  
     *   
     *   This Class also directly accesses functions in the Form1 Class
     *   as it makes changes to the UI based upon the player's score combination
     *   choices.
     *
     *   Author:             Sean Betts        Hayden Topping
     *   Student Number:     08865434          09188797   
     *   Date:               June 2016
     */
    public enum ScoreType {
        Ones, Twos, Threes, Fours, Fives, Sixes,
        SubTotal, BonusFor63Plus, SectionATotal,
        ThreeOfAKind, FourOfAKind, FullHouse,
        SmallStraight, LargeStraight, Chance, Yahtzee,
        YahtzeeBonus, SectionBTotal, GrandTotal
    }
    [Serializable]
    public class Game {

        private BindingList<Player> players = new BindingList<Player>();
        private int currentPlayerIndex;
        private Player currentPlayer;
        private Die[] dice;
        private int playersFinished;
        private int numRolls;

        [NonSerialized]
        private Form1 form;

        [NonSerialized]
        private Label[] scoreTotals;

        public static string defaultPath = Environment.CurrentDirectory;
        private static string savedGameFile = defaultPath + "\\YahtzeeGame.dat";


        //NUMBER OF DIE CONSTANT
        private const int NUMBER_OF_DICE = 5;

        // PLAYER INDEX CONSTANTS
        private const int NEW_GAME_STARTED = -1;
        private const int FIRST_PLAYER = 0;

        //ROLL NUMBER CONSTANTS
        private const int FIRST_ROLL = 1;
        private const int SECOND_ROLL = 2;
        private const int THIRD_ROLL = 3;

        //ONE PLAYER PLAYING
        private const int ONE_PLAYER = 1;




        public Game(Form1 form) {
            this.form = form;

            //initialise data structures and start next(first) turn
            currentPlayerIndex = NEW_GAME_STARTED;
            InitialiseDice();
            InitialisePlayers();
            NextTurn();


        }

        /// <summary>
        /// Initialises the binding list that holds the players with two new players.
        /// </summary>
        private void InitialisePlayers() {
            //initialise list and add two new players.
            players = new BindingList<Player>();
            players.Add(new Player("Player 1", form.GetScoresTotals()));
            players.Add(new Player("Player 2", form.GetScoresTotals()));

        }

        /// <summary>
        /// Property for the players binding list.
        /// </summary>
        public BindingList<Player> Players {
            get {
                return players;
            }

        }

        /// <summary>
        /// Initialises all of the dice labels for use in the game.
        /// </summary>
        private void InitialiseDice() {
            dice = new Die[NUMBER_OF_DICE];

            //get the die and set each label to one of the die.
            Label[] diceLabels = form.GetDice();
            for (int i = 0; i < NUMBER_OF_DICE; i++) {
                dice[i] = new Die(diceLabels[i]);
            }
        }

        /// <summary>
        /// Moves to the next player in game.
        /// </summary>
        public void NextTurn() {
            numRolls = FIRST_ROLL;

            SetUiForNextTurn();
            GetNextPlayer();
            ShowMessage();
            CheckGameIsFinished();
        }

        /// <summary>
        /// Refreshes the die and resets the UI for the next turn.
        /// </summary>
        private void SetUiForNextTurn() {
            form.DisableAndClearCheckBoxes();
            DisableAllScoreButtons();
        }

        /// <summary>
        /// Gets the next player in the list and sets the UI and scores
        /// to that players details and scores.
        /// </summary>
        private void GetNextPlayer() {

            currentPlayerIndex++;
            //if player index is greater than the list of players reset index to first player.
            if (currentPlayerIndex >= players.Count) {
                currentPlayerIndex = FIRST_PLAYER;
            }

            //set current player by player index and show their score.
            currentPlayer = players[currentPlayerIndex];
            currentPlayer.ShowScores();
            form.ShowPlayerName(currentPlayer.Name);

        }

        /// <summary>
        /// Simulates the rolling of a die shows.
        /// </summary>
        public void RollDice() {
            //increment the number of rolls
            numRolls++;
            form.ShowPlayerName(currentPlayer.Name); //update player name in case player has changed it
            ShowMessage(); //show the message related to the number of rolls
            SetScoreButtonStatus();

            //for each die, check if its active and roll the die
            foreach (Die die in dice) {
                if (die.Active) {
                    die.Roll();
                }
            }
        }


        /// <summary>
        /// Presents the designated message for the action that the player has taken.
        /// </summary>
        public void ShowMessage() {
            switch (numRolls) {
                case FIRST_ROLL:
                    form.ShowMessage("Roll 1");
                    break;
                case SECOND_ROLL:
                    form.ShowMessage("Roll 2 or choose a combination to score");
                    break;
                case THIRD_ROLL:
                    form.ShowMessage("Roll 3 or choose a combination to score");
                    break;
                default:
                    form.ShowMessage("Choose a combination to Score");
                    form.DisableRollButton();
                    break;

            }
        }

        /// <summary>
        /// Prevents the die from being changed by setting it to inactive.
        /// </summary>
        /// <param name="diceIndex">The array index of the dice that is to be made inactive.</param>
        public void HoldDie(int diceIndex) {
            dice[diceIndex].Active = false;
        }

        /// <summary>
        /// Allows the die to be changed by setting it as active.
        /// </summary>
        /// <param name="diceIndex">The array index of the dice that is to be made inactive.</param>
        public void ReleaseDie(int diceIndex) {
            dice[diceIndex].Active = true;
        }

        /// <summary>
        /// Takes the scoreType of the pressed button and passes it to the player
        /// object to calculate the score and sets the UI to the end of turn or victory screen.
        /// </summary>
        /// <param name="scoreType">The ScoreType that has been pressed by the </param>
        public void ScoreCombination(ScoreType scoreType) {
            // calculate player score and show them on the UI
            currentPlayer.ScoreCombination(scoreType, GetDiceValues());
            currentPlayer.ShowScores();

            //set UI for end of turn
            SetUiForEndOfTurn();

        }

        /// <summary>
        /// Sets the UI to prevent rolling or selecting score buttons again
        /// and shows the OK button.
        /// </summary>
        public void SetUiForEndOfTurn() {
            form.DisableRollButton();
            DisableAllScoreButtons();
            form.ShowMessage("Your turn has ended - click OK");
            form.ShowOkButton();
        }

        /// <summary>
        /// Gets the integer values of each of the die and stores them 
        /// in an integer array.
        /// </summary>
        /// <returns>Returns an integer array with the die scores</returns>
        private int[] GetDiceValues() {
            int[] scores = new int[NUMBER_OF_DICE];

            //for each die get the faceValue and store it in an integer array 
            for (int i = 0; i < NUMBER_OF_DICE; i++) {
                scores[i] = dice[i].GetFaceValue();
            }
            return scores;
        }

        /// <summary>
        /// Disables all of the score buttons.
        /// </summary>
        private void DisableAllScoreButtons() {
            for (ScoreType i = ScoreType.Ones; i < ScoreType.YahtzeeBonus; i++) {
                form.DisableScoreButton(i);
            }
        }

        /// <summary>
        /// Sets the score buttons depending on the current player's available scorings.
        /// </summary>
        private void SetScoreButtonStatus() {
            for (ScoreType i = ScoreType.Ones; i < ScoreType.YahtzeeBonus; i++) {
                if (currentPlayer.IsAvailable(i)) {
                    form.EnableScoreButton(i);
                } else {
                    form.DisableScoreButton(i);
                }
            }
        }

        /// <summary>
        /// Checks if all players have finished.
        /// </summary>
        /// <returns>Returns true if all players are finished, else false.</returns>
        private bool CheckPlayersForGameFinished() {
            foreach (Player player in Players) {
                if (!player.IsFinished()) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Will disable the UI and stop the Game if all players
        /// have finished.
        /// </summary>
        private void CheckGameIsFinished() {
            //check to see if game is over
            if (CheckPlayersForGameFinished()) {
                // disable UI and present winner
                form.DisableRollButton();
                DisableAllScoreButtons();
                GameFinishedMessage();

            }
        }

        /// <summary>
        /// Finds which players have won the game.
        /// </summary>
        /// <returns>Returns the player with the highest Grand Total.</returns>
        private Player[] FindWinningPlayer() {
            Player[] winningPlayer = new Player[players.Count];
            int winningPlayerIndex = 0;
            int highestScore = 0;
            //for each player check the grand total to find the highest total.
            foreach (Player player in players) {
                if (player.GrandTotal > highestScore) {
                    highestScore = player.GrandTotal;
                }

            }
            foreach (Player player in players) {
                if (player.GrandTotal == highestScore) {
                    winningPlayer[winningPlayerIndex] = player;
                    winningPlayerIndex++;
                }
            }
            return winningPlayer;
        }


        /// <summary>
        /// Displays the message box for the end of game.
        /// </summary>
        private void GameFinishedMessage() {
            DialogResult userInput;
            Player[] winningPlayer = new Player[players.Count];
            StringBuilder winningPlayers = new StringBuilder();

            // if more than one player show winning players
            if (players.Count > ONE_PLAYER) {
                //get winning players
                winningPlayer = FindWinningPlayer();
                //construct a string with the name of the players
                foreach (Player player in winningPlayer) {
                    if (player != null) {
                        winningPlayers.Append(player.Name + "\n");
                    }
                }
                //show message box
                userInput = MessageBox.Show("The Winning Players are: \n" +winningPlayers + 
                    "\nDo you wish to play again?", "Game Over", MessageBoxButtons.YesNo);
            } else { //display victory message
                userInput = MessageBox.Show("You have won.\nDo you wish to play again?", "Game Over", MessageBoxButtons.YesNo);
            }
            CheckUserInput(userInput);
        }

        /// <summary>
        /// Checks the user input to see whether to start a new game or exit
        /// the program.
        /// </summary>
        /// <param name="userInput">The result of the user input.</param>
        private void CheckUserInput(DialogResult userInput) {
            if (userInput == DialogResult.Yes) {
                form.StartNewGame();
            } else {
                Application.Exit();
            }
        }

        /// <summary>
        /// Loads the previous game.
        /// </summary>
        /// <param name="form">The UI form.</param>
        /// <returns>Returns the game.</returns>
        public static Game Load(Form1 form) {
            Game game = null;
            if (File.Exists(savedGameFile)) {
                try {
                    Stream bStream = File.Open(savedGameFile, FileMode.Open);
                    BinaryFormatter bFormatter = new BinaryFormatter();
                    game = (Game)bFormatter.Deserialize(bStream);
                    bStream.Close();
                    game.form = form;
                    game.ContinueGame();
                    return game;
                } catch {
                    MessageBox.Show("Error reading saved game file.\nCannot load saved game.");
                }
            } else {
                MessageBox.Show("No current saved game.");
            }
            return null;
        }

        /// <summary>
        /// Saves the current game.
        /// </summary>
        public void Save() {
            try {
                Stream bStream = File.Open(savedGameFile, FileMode.Create);
                BinaryFormatter bFormatter = new BinaryFormatter();
                bFormatter.Serialize(bStream, this);
                bStream.Close();
                MessageBox.Show("Game saved");
            } catch (Exception e) {

                //MessageBox.Show(e.ToString());
                MessageBox.Show("Error saving game.\nNo game saved.");
            }
        }

        /// <summary>
        /// Sets the necessary values from the loaded game to continue the game.
        /// </summary>
        private void ContinueGame() {
            LoadLabels(form);
            for (int i = 0; i < dice.Length; i++) {
                //uncomment one of the following depending how you implmented Active of Die
                // dice[i].SetActive(true);
                dice[i].Active = true;
            }

            form.ShowPlayerName(currentPlayer.Name);
            form.EnableRollButton();
            form.EnableCheckBoxes();
            // can replace string with whatever you used
            form.ShowMessage("Roll 1");
            currentPlayer.ShowScores();
        }//end ContinueGame

        /// <summary>
        /// Link the labels on the GUI form to the dice and players
        /// </summary>
        /// <param name="form"></param>
        private void LoadLabels(Form1 form) {
            Label[] diceLabels = form.GetDice();
            for (int i = 0; i < dice.Length; i++) {
                dice[i].Load(diceLabels[i]);
            }
            for (int i = 0; i < players.Count; i++) {
                players[i].Load(form.GetScoresTotals());
            }

        }
    }
}
