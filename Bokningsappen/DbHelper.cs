using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen
{
    internal class DbHelper
    {
        private static readonly Random random = new();

        private static readonly string[] firstName =
            {
                "Karin",
                "Anders",
                "Johan",
                "Eva",
                "Maria",
                "Mikael",
                "Anna",
                "Sara",
                "Erik",
                "Per",
                "Christina",
                "Lena",
                "Lars",
                "Emma",
                "Kerstin",
                "Karl",
                "Marie",
                "Peter",
                "Thomas",
                "Karl",
                "Jan",
                "Maria",
                "Karin",
                "Lena"
            };

        private static readonly string[] lastName =
            {
                "Andersson",
                "Johansson",
                "Karlsson",
                "Nilsson",
                "Eriksson",
                "Larsson",
                "Olsson",
                "Persson",
                "Svensson",
                "Gustafsson"
            };

        private static readonly string[] roomName =
            {
                "Sagittarius",
                "Zodiac",
                "Libra",
                "Cygnus",
                "Kepler",
                "Moon",
                "Mars",
                "Monoceros",
                "Orion",
                "Pisces",
                "Earth",
                "Pegasus",
                "Andromeda",
                "Equinox",
                "Calypso",
                "Gemini",
                "Leo",
                "Neptune",
                "Eclipse",
                "Pleiades",
            };

        private static readonly string[] Course =
            {
                "Data Science",
                "Artificial Intelligence",
                "Machine Learning",
                "Business Intelligence",
                "Software Development",
                "Cybersecurity",
                "Digital Marketing",
                "Computer Science"
            };

        internal static string GetName()
        {
            return GetFirstName() + " " + GetLastName();
        }

        internal static string GetFirstName()
        {
            return firstName[random.Next(firstName.Length)];
        }

        internal static string GetLastName()
        {
            return lastName[random.Next(lastName.Length)];
        }

        internal static string GetCourse()
        {
            return Course[random.Next(Course.Length)];
        }

        internal static string GetRoomName(int i)
        {
            return roomName[i];
        }
    }
}
