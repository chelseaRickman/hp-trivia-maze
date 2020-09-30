using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HPTriviaMaze
{
    [Serializable]
    public class QuestionAnswer
    {
        private string question;
        private string answer;
        private string questionType;
        private static SQLiteConnection sqlite_connection;
        private static SQLiteCommand sqlite_command;
        private static SQLiteDataReader sqlite_datareader;
        private static List<int> questionsAlreadyAsked = new List<int>();
        private static int numberOfQuestions;


        public QuestionAnswer(string question, string answer, string questionType)
        {
            this.question = question;
            this.answer = answer;
            this.questionType = questionType;
        }

        public static void SqliteSetup()
        {
            if(sqlite_connection != null)
            {
                sqlite_connection.Close();
            }
            // Create a new database connection
            sqlite_connection = new SQLiteConnection("Data source=hpquestions.db;Version=3;Compress=True;");

            // Open the connection
            sqlite_connection.Open();

            // Create a new SQL command
            sqlite_command = sqlite_connection.CreateCommand();

            // Get the number of records within the database
            int numberOfRecords = 0;
            sqlite_command.CommandText = "SELECT * FROM HPTrivia";
            sqlite_datareader = sqlite_command.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                numberOfRecords++;
            }
            numberOfQuestions = numberOfRecords;
            sqlite_datareader.Close();
        }

        public static QuestionAnswer getQuestionAnswer()
        {
            QuestionAnswer question;
            Random random = new Random();
            int randomQuestion;
            do
            {
                randomQuestion = random.Next(1, numberOfQuestions + 1);
            }
            while (questionsAlreadyAsked.Contains(randomQuestion) && questionsAlreadyAsked.Count < numberOfQuestions);

            if(questionsAlreadyAsked.Count >= numberOfQuestions)
            {
                questionsAlreadyAsked.Clear();
            }

            sqlite_command.CommandText = @"SELECT * FROM HPTrivia WHERE num=$number";
            sqlite_command.Parameters.AddWithValue("$number", randomQuestion);
            sqlite_datareader = sqlite_command.ExecuteReader();
            
            sqlite_datareader.Read();
            questionsAlreadyAsked.Add(randomQuestion);
            question = new QuestionAnswer(sqlite_datareader["question"] + "", sqlite_datareader["answer"] + "", sqlite_datareader["type"] + "");

            sqlite_datareader.Close();

            return question;
        }

        public static void closeDatabaseConnection()
        {
            if(sqlite_connection != null)
            {
                sqlite_connection.Close();
            } 
        }

        public string getQuestion()
        {
            return this.question;
        }

        public string getAnswer()
        {
            return this.answer;
        }

        public string getQuestionType()
        {
            return this.questionType;
        }

        public static List<int> getQuestionsAlreadyAsked()
        {
            return questionsAlreadyAsked;
        }

        public static void setQuestionsAlreadyAsked(List<int> listOfQuestionsAlreadyAsked)
        {
            questionsAlreadyAsked = listOfQuestionsAlreadyAsked;
        }

        
    }
}
