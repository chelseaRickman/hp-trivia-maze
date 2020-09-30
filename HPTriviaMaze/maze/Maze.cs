using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace HPTriviaMaze
{
    [Serializable]
    // This class initializes the setup of the maze
    public class Maze
    {
        private Room[,] mazeLayout;

        public Maze()
        {
            mazeLayout = new Room[4, 4];
            setUpRooms();
        }

        private void setUpRooms()
        {
            // Fills the maze with empty rooms
            for (int row = 0; row < mazeLayout.GetLength(0); row++)
            {
                for (int column = 0; column < mazeLayout.GetLength(1); column++)
                {
                    mazeLayout[row, column] = new Room();
                }
            }

            setUpRoomDetails();
            setUpRoomNeighbors();
        }

        private void setUpRoomDetails()
        {
            // Assigns an image to each room within the maze, and assigns each room's neighbors
            int i = 0;
            foreach(Room room in mazeLayout)
            {
                room.setImage(RoomImages.getImagePath(i));
                i++;
            }
        }

        private void setUpRoomNeighbors()
        {
            // Assigns the neighbors and doors to each room within the maze
            int rows = mazeLayout.GetLength(0);
            int columns = mazeLayout.GetLength(1);

            int currentRow = 0;
            int currentColumn = 0;

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    currentRow = i;
                    currentColumn = j;
                    mazeLayout[i, j].setNeighborsAndDoors(currentRow, currentColumn, mazeLayout);
                }
            }
        }

        public Room[,] getMazeLayout()
        {
            return this.mazeLayout;
        }
    }
}
