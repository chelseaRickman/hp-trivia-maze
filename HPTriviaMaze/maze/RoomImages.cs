using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HPTriviaMaze
{
    public class RoomImages
    {
        private static string[] images =
        {
            "great_hall.jpg",
            "gryffindor_common_room.jpg",
            "slytherin_common_room.jpg",
            "defense_against_the_dark_arts.jpg",
            "potions.jpg",
            "transfiguration.jpg",
            "herbology.jpg",
            "room_of_requirement.png",
            "divination.jpg",
            "moaning_myrtle_bathroom.jpg",
            "trophy_room.jpg",
            "library.jpg",
            "dumbledore_office.jpg",
            "astronomy_tower.png",
            "chamber_of_secrets.jpg",
            "courtyard.png"
        };

        public static string getImagePath(int index)
        {
            return "/room_images/" + images.GetValue(index);
        }
    }
}
