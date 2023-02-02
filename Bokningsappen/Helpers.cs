using Bokningsappen.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen
{
    internal class Helpers
    {
        internal static List<DateTime> GetDatesWithoutWeekends(List<DateTime> dates)
        {
            return dates.Where(date => (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)).ToList();
        }

        internal static List<DateTime> GetDates(int year, int month)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                             .Select(day => new DateTime(year, month, day))
                             .ToList();
        }

        internal static List<DateTime> GetDaysOfMonth(int year, int month)
        {
            List<DateTime> daysOfMonth = GetDates(year, month);
            List<DateTime> daysFromCurrentDay = new List<DateTime>();
            int fromCurrentDay = 0;

            if (DateTime.Now.Month == month && DateTime.Now.Year == year)
            {
                fromCurrentDay = DateTime.Now.Day - 1;
            }

            for (int i = fromCurrentDay; i < daysOfMonth.Count; i++)
            {
                daysFromCurrentDay.Add(daysOfMonth[i]);
            }

            List<DateTime> daysOfMonthExeptWeekends = GetDatesWithoutWeekends(daysFromCurrentDay);

            return daysOfMonthExeptWeekends;
        }

        internal static int GetRandomNumber(int low, int high)
        {
            Random random = new();
            return random.Next(low, high);
        }

        internal static List<string> ConvertRoomListToStringList(List<Room> rooms)
        {
            List<string> roomsStringMenuList = new List<string>();
            foreach (var room in rooms)
            {
                roomsStringMenuList.Add(room.RoomName.PadRight(13) + "Chairs: " + room.Chairs.ToString().PadRight(3) + room.Description);
            }
            return roomsStringMenuList;
        }

        internal static List<string> ConvertUserListToStringList(List<User> users)
        {
            List<string> usersStringMenuList = new List<string>();
            foreach (var user in users)
            {
                usersStringMenuList.Add(user.FullName.PadRight(20) + "Admin: " + user.Admin.ToString().PadRight(6) + user.Course.ToString().PadRight(12) + user.Email);
            }
            return usersStringMenuList;
        }

        internal static List<string> ConvertBokkedRoomsListToStringList(List<BookedRoom> bookedRooms)
        {
            List<string> bookedRoomsStringMenuList = new List<string>();
            List<User> users = User.GetUsersFromDb();
            foreach (var bookedRoom in bookedRooms)
            {
                bookedRoomsStringMenuList.Add("Room: " + Room.GetSelectedRoom(bookedRoom.RoomId).RoomName.ToString().PadRight(13) + "Date: " + bookedRoom.BookedDay.ToString("yy.MM.dd").PadRight(10) + "User: " + User.GetSelectedUser(bookedRoom.UserId).FullName);
            }
            return bookedRoomsStringMenuList;
        }

        internal static int GetDigitInput(int lowerValue, int higherValue)
        {
            int digit = 0;
            string prompt = "Input a number between " + lowerValue + " and " + higherValue + ": ";
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out digit) || digit < lowerValue || digit > higherValue) ;
            return digit;
        }
    }
}
