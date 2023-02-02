using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen.Models
{
    internal class BookedRoom
    {
        public int Id { get; set; }
        public DateTime BookedDay { get; set; }
        //public TimeSpan Duration { get; set; }
        //public DateTime EndTime { get { return StartTime + Duration; } }
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        internal static List<DateTime> GetBookedRoomDatesFromDb(int roomId)
        {
            List<DateTime> bookedRoomDates = new List<DateTime>();
            using (var db = new Context())
            {
                bookedRoomDates = db.BookedRooms.Where(b => b.RoomId == roomId)
                    .Select(b => b.BookedDay).ToList();
            }
            return bookedRoomDates;
        }

        internal static bool RemoveBooking(int bookedRoomId)
        {
            bool saved = false;
            using (var db = new Context())
            {
                var deleteBooking = (from b in db.BookedRooms
                                  where b.Id == bookedRoomId
                                  select b).SingleOrDefault();
                if (deleteBooking != null)
                {
                    db.BookedRooms.Remove((BookedRoom)deleteBooking);
                    try
                    {
                        db.SaveChanges();
                        saved = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("An error has occured, please try again!" + e);
                        Console.ReadLine();
                    }
                }
            }
            return saved;
        }

        internal static void RemoveUserBooking(User user, bool isAdmin)
        {
            bool removed = false;
            string output = "";
            if (isAdmin)
            {
                List<BookedRoom> bookedRooms = GetBookedRoomsFromDb();
                int index = GetBookedRoomChoice(bookedRooms);
                if (index != -1)
                {
                    removed = RemoveBooking(bookedRooms[index].Id);
                }
            }
            else removed = RemoveBooking(GetSelectedBookedRoom(user.Id).Id);
            if (removed) output = "Removed reservation!";
            else output = "Could not remove reservation!";
            Console.WriteLine(output);
            Console.ReadLine();
            Console.Clear();
        }

        private static int GetBookedRoomChoice(List<BookedRoom> bookedRooms)
        {
            if (bookedRooms.Count < 1)
            {
                Console.WriteLine("There is no reservation");
                Console.ReadKey();
                return -1;
            }
            return Menu.MenuList("Select reservation", Helpers.ConvertBokkedRoomsListToStringList(bookedRooms));
        }

        private static List<BookedRoom> GetBookedRoomsFromDb()
        {
            using var db = new Context();
            var bookedRoom = db.BookedRooms.Select(b => b).ToList();
            return bookedRoom;
        }

        internal static BookedRoom GetSelectedBookedRoom(int userId)
        {
            using var db = new Context();
            var bookedRoom = db.BookedRooms.FirstOrDefault(b => b.UserId == userId);
            return bookedRoom;
        }

        internal static bool CheckIfUserHasBookedRoom(User user)
        {
            BookedRoom bookedRoom = GetSelectedBookedRoom(user.Id);

            if (bookedRoom == null) ;
            else if (bookedRoom.BookedDay.AddDays(1) < DateTime.Now) ;
            else return true;
            return false;
        }

        internal static bool PromptToRemoveBooking(User user)
        {
            List<string> question = new() { "No", "Yes" };
            BookedRoom bookedRoom = GetSelectedBookedRoom(user.Id);
            Room room = Room.GetSelectedRoom(bookedRoom.RoomId);
            if (Menu.MenuList("You already have a reservation! Do you want to remove your reservation? " + room.RoomName.ToString().PadRight(13) + bookedRoom.BookedDay.ToString("dd/MM/yy"), question) == 1)
            {
                return RemoveBooking(bookedRoom.Id);
            }
            return false;
        }

        internal static bool SaveBookedRoomToDb(Room room, DateTime dateTime, User user)
        {
            bool bookedRoomBool = false;
            string saveOutput = "";
            BookedRoom bookedRoom = new BookedRoom();

            using (var db = new Context())
            {
                bookedRoom.BookedDay = dateTime;
                try
                {
                    var dbUsers = db.Users;
                    User dbUser = dbUsers.ToList().SingleOrDefault(u => u.Id == user.Id);
                    bookedRoom.User = dbUser;

                    var dbRooms = db.Rooms;
                    Room dbRoom = dbRooms.ToList().SingleOrDefault(r => r.Id == room.Id);
                    bookedRoom.Room = dbRoom;

                    var dbBookedRoom = db.BookedRooms;
                    BookedRoom dbBookRoom = dbBookedRoom.ToList().SingleOrDefault(b => b.Id == bookedRoom.Id);
                    if (dbBookRoom != null)
                    {
                        throw new Exception("BookedRoom ID already exist");
                    }
                    dbBookedRoom.Add(bookedRoom);

                    db.SaveChanges();
                    saveOutput = "Your reservation has been made!";
                    bookedRoomBool = true;
                }
                catch (Exception ex)
                {
                    saveOutput = "Your reservation could not be completed, please contact Admin!\n\n" + ex;
                }
            }
            Console.WriteLine("\n\n" + saveOutput);
            Console.ReadLine();
            return bookedRoomBool;
        }
    }
}
