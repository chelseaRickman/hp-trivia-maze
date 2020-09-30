using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HPTriviaMaze
{
    [Serializable]
    public class Room
    {
        private string imagePath = "";
        private Dictionary<string, Room> neighbors;
        private Dictionary<string, string> doors;

        public Room()
        {
            this.neighbors = new Dictionary<string, Room>();
            neighbors.Add("up", null);
            neighbors.Add("right", null);
            neighbors.Add("down", null);
            neighbors.Add("left", null);

            this.doors = new Dictionary<string, string>();
            doors.Add("up", "");
            doors.Add("right", "");
            doors.Add("down", "");
            doors.Add("left", "");
        }

        public void setImage(String imagePath)
        {
            this.imagePath = imagePath;
        }

        public void setNeighborsAndDoors(int currentRow, int currentColumn, Room[,] maze)
        {
            if(currentRow - 1 >= 0)
            {
                neighbors["up"] = maze[currentRow - 1, currentColumn];
                doors["up"] = "locked";
            }
    
            if (currentColumn + 1 < maze.GetLength(1))
            {
                neighbors["right"] = maze[currentRow, currentColumn + 1];
                doors["right"] = "locked";
            }
           
            if(currentRow + 1 < maze.GetLength(0))
            {
                neighbors["down"] = maze[currentRow + 1, currentColumn];
                doors["down"] = "locked";
            }
           
            if(currentColumn - 1 >= 0)
            {
                neighbors["left"] = maze[currentRow, currentColumn - 1];
                doors["left"] = "locked";
            }
        }

        public string getImage()
        {
            return this.imagePath;
        }

        public Dictionary<string,Room> getNeighbors()
        {
            return this.neighbors;
        }

        public Dictionary<string,string> getDoors()
        {
            return this.doors;
        }
    }
}
