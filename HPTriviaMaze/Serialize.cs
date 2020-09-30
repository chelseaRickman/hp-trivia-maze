using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HPTriviaMaze
{
    public class Serialize
    {
        public static void serializeSave(GameData gameData)
        {
            try
            {
                using (FileStream fs = File.Create("./saved_game.dat"))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fs, gameData);
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public static GameData deserializeLoad()
        {
            GameData gameData = new GameData();
            try
            {
                using(FileStream fs = File.Open("./saved_game.dat", FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    gameData = (GameData)binaryFormatter.Deserialize(fs);
                }
                return gameData;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
    }
}
