using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTriviaMaze
{
    [Serializable]
    public class GameData
    {
        private Room currentRoom;
        private Room exit;
        private Room[,] mazeLayout;
        private string currentAnswer = "";
        private string directionUserSelected = "";
        private List<int> questionsAlreadyAsked = new List<int>();
        private QuestionAnswer currentQuestion;
        private double characterTop = 0;
        private double characterLeft = 0;
        private bool questionIsDisplayed = false;

        public GameData()
        {
            this.questionsAlreadyAsked = QuestionAnswer.getQuestionsAlreadyAsked();
        }

        // Properties
        public Room CurrentRoom
        {
            get
            {
                return this.currentRoom;
            }
            set
            {
                this.currentRoom = value;
            }
        }

        public Room Exit
        {
            get
            {
                return this.exit;
            }
            set
            {
                this.exit = value;
            }
        }

        public Room[,] MazeLayout
        {
            get
            {
                return this.mazeLayout;
            }
            set
            {
                this.mazeLayout = value;
            }
        }

        public string CurrentAnswer
        {
            get
            {
                return this.currentAnswer;
            }
            set
            {
                this.currentAnswer = value;
            }
        }

        public string DirectionUserSelected
        {
            get
            {
                return this.directionUserSelected;
            }
            set
            {
                this.directionUserSelected = value;
            }
        }

        public List<int> QuestionsAlreadyAsked
        {
            get
            {
                return this.questionsAlreadyAsked;
            }
        }

        public double CharacterTop
        {
            get
            {
                return this.characterTop;
            }
            set
            {
                this.characterTop = value;
            }
        }

        public double CharacterLeft
        {
            get
            {
                return this.characterLeft;
            }
            set
            {
                this.characterLeft = value;
            }
        }

        public QuestionAnswer CurrentQuestion
        {
            get
            {
                return this.currentQuestion;
            }
            set
            {
                this.currentQuestion = value;
            }
        }

        public bool QuestionIsDisplayed
        {
            get
            {
                return this.questionIsDisplayed;
            }
            set
            {
                this.questionIsDisplayed = value;
            }
        }
    }
}
