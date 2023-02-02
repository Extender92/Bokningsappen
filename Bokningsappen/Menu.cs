using Bokningsappen.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen
{
    internal class Menu
    {
        internal static void MainMenu()
        {
            List<string> mainMenuText = new()
            {
                "Login",
                "Create User",
                "Exit"
            };

            bool menu = true;
            while (menu)
            {
                Console.Clear();
                int index = MenuList("Main Menu", mainMenuText);
                switch (index)
                {
                    case 0:
                        UserMenu(Security.UserLogin());
                        break;
                    case 1:
                        User.CreateUser(false);
                        break;
                    case 2:
                    case -1:
                        menu = false;
                        break;
                }
            }
        }

        private static void UserMenu(User user)
        {
            List<string> userMenuText = new()
            {
                "Book Room",
                "Edit Menu",
                "Log out"
            };

            while (true)
            {
                Console.Clear();
                int index = MenuList("User Menu", userMenuText);
                switch (index)
                {
                    case 0:
                        Room.BookRoom(user);
                        break;
                    case 1:
                        EditMenu(user);
                        break;
                    case 2:
                    case -1:
                        return;
                }
            }
        }

        private static void EditMenu(User user)
        {
            bool isAdmin = Security.CheckAdminPrivileges(user);
            List<string> userMenuText = new();
            userMenuText.Add("Create New User");
            if (isAdmin)
            {
                userMenuText.Add("Edit Existing Users");
                userMenuText.Add("Remove Existing Users");
                userMenuText.Add("Create New Room");
                userMenuText.Add("Edit Existing Rooms");
                userMenuText.Add("Remove Existing Rooms");
                userMenuText.Add("Remove Reservation");
                userMenuText.Add("Queries");
            }
            else
            {
                userMenuText.Add("Edit Your UserProfile");
                userMenuText.Add("Remove Your Reservation");
            }
            userMenuText.Add("Go Back");

            while (true)
            {
                Console.Clear();
                int index = MenuList("Edit Menu", userMenuText);
                if (!isAdmin)
                {
                    if (index == 2) index = 6;
                    else if (index == 3) index = -1;
                }

                switch (index)
                {
                    case 0:
                        User.CreateUser(isAdmin);
                        break;
                    case 1:
                        User.UpdateUser(user, isAdmin);
                        break;
                    case 2:
                        User.RemoveUser();
                        break;
                    case 3:
                        Room.CreateRoom();
                        break;
                    case 4:
                        Room.UpdateRoom();
                        break;
                    case 5:
                        Room.RemoveRoom();
                        break;
                    case 6:
                        BookedRoom.RemoveUserBooking(user, isAdmin);
                        break;
                    case 7:
                        QueriesMenu();
                        break;
                    case 8:
                    case -1:
                        return;
                }
            }
        }

        internal static void QueriesMenu()
        {
            List<string> userMenuText = new()
            {
                "Top Five Currently Popular Rooms",
                "Chairs In Rooms",
                "Courses Order By Students",
                "Top Three Popular Names",
                "Go Back"
            };

            while (true)
            {
                Console.Clear();
                int index = MenuList("Queries Menu", userMenuText);
                switch (index)
                {
                    case 0:
                        Queries.TopFiveCurrentlyPopularRooms();
                        break;
                    case 1:
                        Queries.RoomChairs();
                        break;
                    case 2:
                        Queries.CoursesListOrderByPopular();
                        break;
                    case 3:
                        Queries.TopThreePopularFirstNames();
                        break;
                    case 4:
                    case -1:
                        return;
                }
            }
        }

        internal static int MenuList(string menuName, List<string> options)
        {
            int index = 0;

            ConsoleKeyInfo keyPressed;

            do
            {
                Console.SetCursorPosition(0,0);
                GUI.PrintVerticalMenu(index, menuName, options);
                Console.CursorVisible = false;

                keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.DownArrow && index != options.Count - 1) index++;
                else if (keyPressed.Key == ConsoleKey.UpArrow && index >= 1) index--;
                else if (keyPressed.Key == ConsoleKey.Escape) return -1;
            } while (keyPressed.Key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            Console.Clear();

            return index;
        }

        internal static int DayMenuList(string menuName, List<DateTime> days, List<DateTime> bookedRoomDates)
        {
            int index = 0;

            ConsoleKeyInfo keyPressed;

            do
            {
                Console.SetCursorPosition(7, 1);
                GUI.PrintDayMenu(index, menuName, days, bookedRoomDates);
                Console.CursorVisible = false;

                keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.DownArrow && index != days.Count - 1) index++;
                else if (keyPressed.Key == ConsoleKey.UpArrow && index >= 1) index--;
                else if (keyPressed.Key == ConsoleKey.Escape) return -1;
                else if (keyPressed.Key == ConsoleKey.N) return -2;
                else if (keyPressed.Key == ConsoleKey.P) return -3;
            } while (keyPressed.Key != ConsoleKey.Enter);

            Console.CursorVisible = true;
            Console.Clear();

            return index;
        }
    }
}
