using Bokningsappen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen
{
    internal class Queries
    {
        internal static void TopFiveCurrentlyPopularRooms()
        {
            using var db = new Context();

            var roomsBooked = db.BookedRooms
                .GroupBy(r => r.RoomId)
                .OrderByDescending(g => g.Count())
                .Select(g => new {Id = g.Key, Count = g.Count(),}).Take(5).ToList();

            Console.WriteLine();
            foreach (var room in roomsBooked)
            {
                Console.WriteLine($"  {Room.GetSelectedRoom(room.Id).RoomName.PadRight(15)} Current active bookings: {room.Count}");
            }
            Console.ReadLine();
        }

        internal static void CoursesListOrderByPopular()
        {
            using var db = new Context();

            var mostPopularCourses = db.Users
                .Where(c => c.Course != "Admin" && c.Course != "Teacher")
                .GroupBy(c => c.Course)
                .OrderByDescending(g => g.Count())
                .Select(g => new {Name = g.Key, Count = g.Count()}).ToList();

            Console.WriteLine();
            foreach (var course in mostPopularCourses)
            {
                Console.WriteLine($"  Course: {course.Name.PadRight(25)} Number of students: {course.Count}");
            }
            Console.ReadLine();
        }

        internal static void TopThreePopularFirstNames()
        {
            using var db = new Context();

            var names = db.Users
                .GroupBy(u => u.FirstName)
                .OrderByDescending(g => g.Count())
                .Select(g => new { Name = g.Key, Count = g.Count(), }).Take(3).ToList();

            Console.WriteLine();
            foreach (var name in names)
            {
                Console.WriteLine($"  Name: {name.Name.PadRight(10)} Count: {name.Count}");
            }
            Console.ReadLine();
        }

        internal static void RoomChairs()
        {
            using var db = new Context();

            Room roomMostChairs = db.Rooms.Where(r => r.Chairs == db.Rooms.Max(r => r.Chairs)).FirstOrDefault();
            Room roomLeastChairs = db.Rooms.Where(r => r.Chairs == db.Rooms.Min(r => r.Chairs)).FirstOrDefault();
            Double AvgNumberofChairs = db.Rooms.Average(r => r.Chairs);

            Console.WriteLine($"\n  The room with the most amount of chairs: {roomMostChairs.RoomName} with {roomMostChairs.Chairs} chairs.");
            Console.WriteLine($"  The room with the least amount of chairs: {roomLeastChairs.RoomName} with {roomLeastChairs.Chairs} chairs.");
            Console.WriteLine($"  The average amount of chairs is: {AvgNumberofChairs.ToString("0.00")}.");
            Console.ReadLine();
        }
    }
}
