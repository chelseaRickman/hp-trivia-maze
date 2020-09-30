using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HPTriviaMaze
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        private void Developer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAboutDisplay.Text = @"
            Developed by Chelsea Rickman
            August 2020";
        }

        private void HowToPlay_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAboutDisplay.Text = @"
            - Select 'New Game' to start a new game, or 'Load Game' to load a previously saved game.
            - You can save your game at any point in the game, and return to it at a later time.
            - The goal of the game is to get to the room on the map, marked 'END'. If you reach this room, you win the game!
            - To move throughout the maze, you will need to correctly answer Harry Potter trivia questions.
            - The room you are currently in will be displayed, along with the directions (or doors in which) you can go
            - Choose which door you would like to unlock, by clicking one of the displayed directional buttons.
            - If you answer correctly, the door unlocks and you can move through that door into the next room.
            - If you answer incorrectly, that door will become permanently locked and can no longer be used.
            - When entering a room, all the doors lock behind you. Meaning, even if you just went through that door to get into the room, you cannot
                go back through that door without first answering a question correctly.
            - Your character will move through the displayed maze, so you know where you currently are within the maze.
            - Answering questions:
                - There are three types of questions: multiple choice, true/false, and short answer
                - Multiple choice questions are submitted by typing A, B, C, or D within the textbox and clicking 'Submit'.
                - True/false questions are submitted by typing T for true or F for false and clicking 'Submit'.
                - Short answer questions are submitted by typing in the answer within the text box (1 or 2 word answers) and clicking 'Submit'.
            
            WINNING THE GAME
            Get to the room in the maze marked 'END'!

            GAME OVER
            At any point if all the doors in the room you are in become permanently locked or if the two doors that can access the 'END' room become
            permanently locked, you lose the game!S
            ";
        }

        private void Shortcuts_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAboutDisplay.Text = @"
            File: CTRL+F
            New Game: CTRL+N
            Load Game: CTRL+L
            Save game: CTRL+S
            Quit game: CTRL+Q
            Exit: CTRL+X
            About: CTRL+A
            ";
        }

        private void Sources_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            txtAboutDisplay.Text = @"
            I do not own the images or music that were used in this program.
            The images were acquired using Google Images.
            The music is 'Hedwig's Theme' by John Williams, which I purchased through iTunes.
            ";
        }

        private void btnAboutOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
