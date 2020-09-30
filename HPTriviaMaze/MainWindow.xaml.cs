// Chelsea Rickman

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace HPTriviaMaze
{
    [Serializable]
    public partial class MainWindow : Window
    {
        private static GameData gameData;

        public MainWindow()
        {
            InitializeComponent();
            this.Show();
            gameData = new GameData();
            playBackgroundMusic();
        }

        // Menu event handlers
        private void mnuNewGame_Click(object sender, RoutedEventArgs e)
        {
            startNewGame();
        }

        private void mnuLoadGame_Click(object sender, RoutedEventArgs e)
        {
            loadGame();
        }

        private void mnuSaveGame_Click(object sender, RoutedEventArgs e)
        {
            saveGame();
        }

        private void mnuGameInfo_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.Show();
        }

        private void mnuPauseGame_Click(object sender, RoutedEventArgs e)
        {
            Pause pauseWindow = new Pause();
            pauseWindow.Show();
        }

        private void mnuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void mnuQuit_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "Would you like to save the current game?\nClick Yes to save and quit, No to quit game without saving, or Cancel to return to the game";
            string caption = "Quitting the game";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult response = MessageBox.Show(messageBoxText, caption, button, icon);
            if (response == MessageBoxResult.Yes)
            {
                saveGame();
                setUpElementVisibility(false);
                directionalButtonsVisibility(false);
            }
            else if (response == MessageBoxResult.No)
            {
                setUpElementVisibility(false);
                directionalButtonsVisibility(false);
            }
        }


        // Button event handlers
        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            startNewGame();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            loadGame();
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            directionButton_Click("up");
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            directionButton_Click("left");
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            directionButton_Click("right");
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            directionButton_Click("down");
        }

        private void btnSubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            gameData.QuestionIsDisplayed = false;
            txtQuestionDisplay.Text = "";
            btnSubmitAnswer.IsEnabled = false;
            bool userAnsweredCorrectly = false;

            string[] userAnswer = txtboxAnswerSubmit.Text.Trim().ToUpper().Split(' ');
            foreach (string word in userAnswer)
            {
                if (gameData.CurrentAnswer.Contains(word) || word.Contains(gameData.CurrentAnswer))
                {
                    userAnsweredCorrectly = true;
                }
            }
            txtboxAnswerSubmit.Text = "";
            if (userAnsweredCorrectly)
            {
                correctAnswer();
            }
            else
            {
                wrongAnswer();
            }
        }

        private void directionButton_Click(string direction)
        {
            directionalButtonsVisibility(false);
            gameData.DirectionUserSelected = direction;
            displayQuestion();
        }

        // MediaElement event handlers
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaMusic.Volume = (double)volumeSlider.Value;
        }

        private void Music_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaMusic.Position = new TimeSpan(0);
        }

        private void playBackgroundMusic()
        {
            // I grabbed the XAML code for the volumeSlider here: https://docs.microsoft.com/en-us/dotnet/framework/wpf/graphics-multimedia/how-to-control-a-mediaelement-play-pause-stop-volume-and-speed
            mediaMusic.Source = new Uri("../../audio/hedwigs_theme.mp3", UriKind.Relative);
            mediaMusic.Play();
            mediaMusic.Volume = (double)volumeSlider.Value;
        }

        public void startNewGame()
        {
            QuestionAnswer.SqliteSetup();
            setInitialCharacterLocation();
            setUpElementVisibility(true);
            directionalButtonsVisibility(false);
            Maze maze = new Maze();
            gameData.MazeLayout = maze.getMazeLayout();
            gameData.CurrentRoom = gameData.MazeLayout[0, 0];
            gameData.Exit = gameData.MazeLayout[3, 3];
            gamePlay();
        }
        private void saveGame()
        {
            Serialize.serializeSave(gameData);
            MessageBox.Show("Save complete.");
        }

        private void loadGame()
        {
            directionalButtonsVisibility(false);
            gameData = Serialize.deserializeLoad();
            Canvas.SetTop(Character, gameData.CharacterTop);
            Canvas.SetLeft(Character, gameData.CharacterLeft);
            QuestionAnswer.setQuestionsAlreadyAsked(gameData.QuestionsAlreadyAsked);
            setUpElementVisibility(true);

            // Reestablish database connection if necessary
            QuestionAnswer.SqliteSetup();
            gamePlay();
        }

        private void setInitialCharacterLocation()
        {
            Canvas.SetTop(Character, 6);
            gameData.CharacterTop = 6;
            Canvas.SetLeft(Character, 7);
            gameData.CharacterLeft = 7;
        }

        private void gamePlay()
        {
            bool wonGame = checkIfGameIsWon();
            bool gameOver = checkForGameOver();

            if(wonGame || gameOver)
            {
                setUpElementVisibility(false);
                directionalButtonsVisibility(false);
            }
            else
            {
                displayCurrentRoom();
                displayDirections();
            } 
        }

        private void displayCurrentRoom()
        {
            // Found code to update an image dynamically here: https://stackoverflow.com/questions/3787137/change-image-source-in-code-behind-wpf/3787280
            Uri uri = new Uri(gameData.CurrentRoom.getImage(), UriKind.Relative);
            CurrentRoomDisplay.Source = new BitmapImage(uri);

            string[] imagePath = gameData.CurrentRoom.getImage().Split('/');
            string[] imageName = imagePath[imagePath.Length - 1].Split('.')[0].Split('_');

            string currentRoomLabel = "";
            foreach(string word in imageName)
            {
                currentRoomLabel += capitalize(word) + " ";
            }

            txtCurrentRoomLabel.Text = $"Current Room:\n{currentRoomLabel}";
        }

        private string capitalize(string word)
        {
            string capitalizedWord = "";
            capitalizedWord += char.ToUpper(word[0]);
            capitalizedWord += word.Substring(1);

            return capitalizedWord;
        }

        private void updateGameInstructions(bool chooseDirection)
        {
            if (chooseDirection)
            {
                txtMazeInstructions.Text = "Which door would you like to unlock? Click one of the directions shown.";
            }
            else
            {
                txtMazeInstructions.Text = "Answer the question to unlock the door.";
            }
        }

        private bool checkForGameOver()
        {
            Dictionary<string, string> currentRoomDoors = gameData.CurrentRoom.getDoors();
            int doorsAvailable = 0;
            int doorsPermanentlyLocked = 0;
            foreach (KeyValuePair<string, string> door in currentRoomDoors)
            {
                if (door.Value != "")
                {
                    doorsAvailable += 1;
                }
                if (door.Value == "perm_locked")
                {
                    doorsPermanentlyLocked += 1;
                }
            }
            if (doorsAvailable == doorsPermanentlyLocked)
            {
                gameOver();
                return true;
            }

            Dictionary<string, string> exitDoors = gameData.Exit.getDoors();
            doorsAvailable = 0;
            doorsPermanentlyLocked = 0;
            foreach (KeyValuePair<string, string> door in exitDoors)
            {
                if (door.Value != "")
                {
                    doorsAvailable += 1;
                }
                if (door.Value == "perm_locked")
                {
                    doorsPermanentlyLocked += 1;
                }
            }
            if (doorsAvailable == doorsPermanentlyLocked)
            {
                gameOver();
                return true;
            }

            return false;
        }

        private void displayDirections()
        {
            Dictionary<string, string> currentRoomDoors = gameData.CurrentRoom.getDoors();

            if (!gameData.QuestionIsDisplayed)
            {
                updateGameInstructions(true);

                if (currentRoomDoors["up"] == "locked")
                {
                    btnUp.Visibility = Visibility.Visible;
                }
                if (currentRoomDoors["right"] == "locked")
                {
                    btnRight.Visibility = Visibility.Visible;
                }
                if (currentRoomDoors["down"] == "locked")
                {
                    btnDown.Visibility = Visibility.Visible;
                }
                if (currentRoomDoors["left"] == "locked")
                {
                    btnLeft.Visibility = Visibility.Visible;
                }
            }   
        }

        private void gameOver()
        {
            QuestionAnswer.closeDatabaseConnection();
            GameOver gameOverWindow = new GameOver();
            gameOverWindow.Show();
        }

        private void displayQuestion()
        {
            gameData.QuestionIsDisplayed = true;
            gameData.CurrentQuestion = QuestionAnswer.getQuestionAnswer();
            string questionText = "";
            string question = gameData.CurrentQuestion.getQuestion();
            string answer = gameData.CurrentQuestion.getAnswer();
            string type = gameData.CurrentQuestion.getQuestionType();

            if (type == "choice")
            {
                string[] questionAndChoices = question.Split('?');
                questionText += questionAndChoices[0] + '\n';

                string[] choices = questionAndChoices[1].Split(',');
                int selection = (int)'A';
                foreach (string choice in choices)
                {
                    questionText += $"{Convert.ToChar(selection)}. {choice}\n";
                    selection++;
                }
                questionText += "Enter A, B, C, or D.";
            }
            else if (type == "truefalse")
            {
                questionText += $"{question} True or False?\nEnter T for true or F for false.";
            }
            else
            {
                questionText += $"{question}\nEnter your answer.";
            }

            updateGameInstructions(false);
            txtQuestionDisplay.Text = questionText;
            gameData.CurrentAnswer = answer;
            btnSubmitAnswer.IsEnabled = true;
        }

        private void wrongAnswer()
        {
            if (gameData.DirectionUserSelected == "up")
            {
                gameData.CurrentRoom.getDoors()["up"] = "perm_locked";
                gameData.CurrentRoom.getNeighbors()["up"].getDoors()["down"] = "perm_locked";
            }
            else if (gameData.DirectionUserSelected == "right")
            {
                gameData.CurrentRoom.getDoors()["right"] = "perm_locked";
                gameData.CurrentRoom.getNeighbors()["right"].getDoors()["left"] = "perm_locked";
            }
            else if (gameData.DirectionUserSelected == "down")
            {
                gameData.CurrentRoom.getDoors()["down"] = "perm_locked";
                gameData.CurrentRoom.getNeighbors()["down"].getDoors()["up"] = "perm_locked";
            }
            else
            {
                gameData.CurrentRoom.getDoors()["left"] = "perm_locked";
                gameData.CurrentRoom.getNeighbors()["left"].getDoors()["right"] = "perm_locked";
            }

            gamePlay();
        }

        private void correctAnswer()
        {
            if (gameData.DirectionUserSelected == "up")
            {
                gameData.CurrentRoom = gameData.CurrentRoom.getNeighbors()["up"];
                moveCharacter("top", -70.0);
            }
            else if (gameData.DirectionUserSelected == "right")
            {
                gameData.CurrentRoom = gameData.CurrentRoom.getNeighbors()["right"];
                moveCharacter("left", 83.0);
            }
            else if (gameData.DirectionUserSelected == "down")
            {
                gameData.CurrentRoom = gameData.CurrentRoom.getNeighbors()["down"];
                moveCharacter("top", 70.0);
            }
            else
            {
                gameData.CurrentRoom = gameData.CurrentRoom.getNeighbors()["left"];
                moveCharacter("left", -83.0);
            }

            gamePlay();

        }

        private void moveCharacter(string canvasDirection, double pixelsToMove)
        {

            if (canvasDirection == "top")
            {
                gameData.CharacterTop = Canvas.GetTop(Character) + pixelsToMove;
                Canvas.SetTop(Character, gameData.CharacterTop);
            }
            else
            {
                gameData.CharacterLeft = Canvas.GetLeft(Character) + pixelsToMove;
                Canvas.SetLeft(Character, gameData.CharacterLeft);
            }
        }

        private bool checkIfGameIsWon()
        {
            if (gameData.CurrentRoom == gameData.Exit)
            {
                wonTheGame();
                return true;
            }

            return false;
        }

        private void wonTheGame()
        {
            QuestionAnswer.closeDatabaseConnection();
            WonTheGame wonTheGameWindow = new WonTheGame();
            wonTheGameWindow.Show();
        }

        public void setUpElementVisibility(bool gameHasStarted)
        {
            if (gameHasStarted)
            {
                txtWelcome.Visibility = Visibility.Hidden;
                btnNewGame.Visibility = Visibility.Hidden;
                btnLoadGame.Visibility = Visibility.Hidden;

                CurrentRoomDisplay.Visibility = Visibility.Visible;
                txtCurrentRoomLabel.Visibility = Visibility.Visible;
                mazeDisplay.Visibility = Visibility.Visible;
                txtMazeInstructions.Visibility = Visibility.Visible;
                txtQuestionDisplay.Visibility = Visibility.Visible;
                txtboxAnswerSubmit.Visibility = Visibility.Visible;
                btnSubmitAnswer.Visibility = Visibility.Visible;
                Character.Visibility = Visibility.Visible;
            }
            else
            {
                txtWelcome.Visibility = Visibility.Visible;
                btnNewGame.Visibility = Visibility.Visible;
                btnLoadGame.Visibility = Visibility.Visible;

                CurrentRoomDisplay.Visibility = Visibility.Hidden;
                txtCurrentRoomLabel.Visibility = Visibility.Hidden;
                mazeDisplay.Visibility = Visibility.Hidden;
                txtMazeInstructions.Visibility = Visibility.Hidden;
                txtQuestionDisplay.Visibility = Visibility.Hidden;
                txtboxAnswerSubmit.Visibility = Visibility.Hidden;
                btnSubmitAnswer.Visibility = Visibility.Hidden;
                Character.Visibility = Visibility.Hidden;
            }
            
        }

        private void directionalButtonsVisibility(bool visible)
        {
            if (visible)
            {
                btnUp.Visibility = Visibility.Visible;
                btnRight.Visibility = Visibility.Visible;
                btnDown.Visibility = Visibility.Visible;
                btnLeft.Visibility = Visibility.Visible;
            }
            else
            {
                btnUp.Visibility = Visibility.Hidden;
                btnRight.Visibility = Visibility.Hidden;
                btnDown.Visibility = Visibility.Hidden;
                btnLeft.Visibility = Visibility.Hidden;
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            string messageBoxText = "Would you like to save the current game?\nClick Yes to save and exit, No to exit without saving, or Cancel to return to the game";
            string caption = "Exiting the game";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;

            MessageBoxResult response = MessageBox.Show(messageBoxText, caption, button, icon);
            if(response == MessageBoxResult.Yes)
            {
                saveGame();
            }
            else if(response == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }

        // Static methods
        public static GameData getGameData()
        {
            return gameData;
        }
    }
}
