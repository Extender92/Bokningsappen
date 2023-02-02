using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen.Models
{
    internal class Room
    {
        public int Id { get; set; }
        public string RoomName { get; set; }
        public int Chairs { get; set; }
        public string Description { get; set; }
        public List<BookedRoom> BookedRooms { get; set; }

        internal static List<Room> GetAllRoomsFromDb()
        {
            List<Room> rooms = new List<Room>();
            using (var db = new Context())
            {
                rooms = db.Rooms.ToList();
            }
            return rooms;
        }

        internal static Room GetSelectedRoom(int roomId)
        {
            using var db = new Context();
            var room = db.Rooms.FirstOrDefault(r => r.Id == roomId);
            return room;
        }

        internal static void BookRoom(User user)
        {
            if (BookedRoom.CheckIfUserHasBookedRoom(user))
                if (!BookedRoom.PromptToRemoveBooking(user)) return;

            List<Room> roomsList = GetAllRoomsFromDb();

            int month = DateTime.Now.Month;
            List<DateTime> dayList = Helpers.GetDaysOfMonth(DateTime.Now.Year, month);

            bool bookedRoom = false;

            while (true)
            {
                month = DateTime.Now.Month;
                Console.Clear();

                int roomIndex = GetRoomChoice(roomsList);
                if (roomIndex == -1)
                {
                    Console.Clear();
                    return;
                }

                while (true)
                {
                    List<DateTime> bookedRoomDates = BookedRoom.GetBookedRoomDatesFromDb(roomsList[roomIndex].Id);
                    Console.Clear();
                    int dayIndex = Menu.DayMenuList("Available days of " + DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(month), dayList, bookedRoomDates);

                    if (dayIndex == -2 && month < 12) month++;
                    else if (dayIndex == -3 && month > 1 && month > DateTime.Now.Month) month--;
                    else if (dayIndex == -1) break;
                    else if (dayIndex >= 0)
                    {
                        if (!bookedRoomDates.Contains(dayList[dayIndex])) bookedRoom = BookedRoom.SaveBookedRoomToDb(roomsList[roomIndex], dayList[dayIndex], user);
                    }
                    if (bookedRoom)
                    {
                        Console.Clear();
                        return;
                    }

                    dayList = Helpers.GetDaysOfMonth(DateTime.Now.Year, month);
                }
            }
        }

        internal static void CreateRoom()
        {
            EditRoom(new Room());
        }

        private static void EditRoom(Room room)
        {
            List<string> question = new() { "No", "Yes" };
            int index = 1;

            while (index != 0)
            {
                index = Menu.MenuList("Edit Room Name: " + (room.RoomName == null ? "Empty" : room.RoomName), question);
                if (index == 1) room.RoomName = Console.ReadLine();
            }
            index = 1;
            while (index != 0)
            {
                index = Menu.MenuList("Edit Room Chairs: " + (room.Chairs == null ? "Empty" : room.Chairs), question);
                if (index == 1) room.Chairs = Helpers.GetDigitInput(1, 20);
            }
            index = 1;
            while (index != 0)
            {
                index = Menu.MenuList("Edit Room Description: " + (room.Description == null ? "Empty" : room.Description), question);
                if (index == 1) room.Description = Console.ReadLine();
            }

            index = Menu.MenuList(("Save Room: " + room.RoomName), question);
            if (index == 1) SaveRoom(room);
        }

        internal static void UpdateRoom()
        {
            List<Room> roomsList = GetAllRoomsFromDb();
            int index = GetRoomChoice(roomsList);
            if (index != -1)
            {
                EditRoom(roomsList[index]);
            }

        }

        internal static void RemoveRoom()
        {
            string output = "";
            List<Room> roomsList = GetAllRoomsFromDb();
            int index = GetRoomChoice(roomsList);
            if (index != -1)
            {
                using (var db = new Context())
                {
                    var deleteRoom = GetSelectedRoom(roomsList[index].Id);
                    if (deleteRoom != null)
                    {
                        db.Rooms.Remove((Room)deleteRoom);
                        try
                        {
                            db.SaveChanges();
                            output = "Removed room succesfully!";
                        }
                        catch (Exception e)
                        {
                            output = "Could not remove selected room!" + e;
                        }
                    }
                }
                Console.WriteLine(output);
                Console.ReadLine();
                Console.Clear();
            }
        }

        private static int GetRoomChoice(List<Room> roomsList)
        {
            if (roomsList.Count < 1)
            {
                Console.WriteLine("There is no Rooms");
                Console.ReadKey();
                return -1;
            }
            return Menu.MenuList("Select Room", Helpers.ConvertRoomListToStringList(roomsList));
        }

        private static void SaveRoom(Room room)
        {
            string saveOutput = "";
            using (var db = new Context())
            {
                var dbRooms = db.Rooms;
                Room dbRoom = dbRooms.ToList().SingleOrDefault(r => r.Id == room.Id);
                if (dbRoom == null)
                {
                    dbRooms.Add(room);
                }
                else
                {
                    dbRoom.RoomName = room.RoomName;
                    dbRoom.Chairs = room.Chairs;
                    dbRoom.Description = room.Description;
                }
                try
                {
                    db.SaveChanges();
                    saveOutput = "Save success";
                }
                catch (Exception ex)
                {
                    saveOutput = "Could not save values to database" + ex;
                }
            }
            Console.WriteLine("\n\n" + saveOutput);
            Console.ReadLine();
            Console.Clear();
        }
    }
}
