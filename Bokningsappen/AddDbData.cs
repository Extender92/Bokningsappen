using Bokningsappen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen
{
    internal class AddDbData
    {
        private static void AddUsers()
        {
            using var db = new Context();

            var dbNewUsers = db.Users;

            db.AddRange(
                new User()
                {
                    Admin = true,
                    FirstName = "Robin",
                    LastName = "Forsling",
                    Course = "Admin",
                    Password = "Banan",
                    Email = "Robin_Forsling@skola.com",
                },
                new User()
                {
                    Admin = true,
                    FirstName = "Mikael",
                    LastName = "Engström",
                    Course = "Teacher",
                    Password = "Tomat",
                    Email = "Mikael_Engstrom@skola.com",
                });



            for (int i = 0; i < 38; i++)
            {
                string firstName = DbHelper.GetFirstName();
                string lastName = DbHelper.GetLastName();

                var newUsers = new User()
                {
                    Admin = false,
                    FirstName = firstName,
                    LastName = lastName,
                    Course = DbHelper.GetCourse(),
                    Password = "Pass",
                    Email = firstName + "_" + lastName + "@skola.com"
                };

                if (dbNewUsers.SingleOrDefault(u => u.Email == newUsers.Email) == null)
                {
                    dbNewUsers.Add(newUsers);
                }
            }

            try
            {
                db.SaveChanges();
                Console.WriteLine("Users added");
            }
            catch
            {
                Console.WriteLine("Users not added");
            }
        }

        private static void AddRooms()
        {
            using var db = new Context();

            var dbNewRooms = db.Rooms;

            for (int i = 0; i < 20; i++)
            {
                string name = DbHelper.GetRoomName(i);
                string projector = ".";
                int numberOfChairs = Helpers.GetRandomNumber(3, 9);
                if (Helpers.GetRandomNumber(0, 2) == 1) projector = " and a projector.";

                var newRoom = new Room()
                {
                    RoomName = name,
                    Chairs = numberOfChairs,
                    Description = $"Room {name} have {numberOfChairs} number of chairs, the room have a whiteboard{projector}"
                };

                if (dbNewRooms.SingleOrDefault(a => a.RoomName == newRoom.RoomName) == null)
                {
                    dbNewRooms.Add(newRoom);
                }
            }

            try
            {
                db.SaveChanges();
                Console.WriteLine("Rooms added");
            }
            catch
            {
                Console.WriteLine("Rooms not added");
            }
        }

        internal static void AddReservations()
        {
            using var db = new Context();

            var dbNewBookedRooms = db.BookedRooms;

            for (int i = 0; i < User.GetUsersFromDb().Count; i++)
            {
                var newBookedRoom = new BookedRoom()
                {
                    BookedDay = DateTime.Today.AddDays(Helpers.GetRandomNumber(1, 30)),
                    RoomId = Helpers.GetRandomNumber(1, Room.GetAllRoomsFromDb().Count),
                    UserId = i + 1
                };

                if (dbNewBookedRooms.SingleOrDefault(b => b.Id == newBookedRoom.Id) == null)
                {
                    dbNewBookedRooms.Add(newBookedRoom);
                }
            }

            //var newBookedRoom = new BookedRoom()
            //{
            //    BookedDay = DateTime.Today.AddDays(-1),
            //    RoomId = Helpers.GetRandomNumber(1, Room.GetAllRoomsFromDb().Count),
            //    UserId = 1
            //};

            //if (dbNewBookedRooms.SingleOrDefault(b => b.Id == newBookedRoom.Id) == null)
            //{
            //    dbNewBookedRooms.Add(newBookedRoom);
            //}

            try
            {
                db.SaveChanges();
                Console.WriteLine("Reservations added");
            }
            catch
            {
                Console.WriteLine("Reservations not added");
            }
        }

        internal static void AddWholeDb()
        {
            AddUsers();
            AddRooms();
            AddReservations();
            Console.ReadLine();
            Console.Clear();
        }
    }
}
