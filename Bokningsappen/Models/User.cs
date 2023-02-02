using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bokningsappen.Models
{
    internal class User
    {
        public int Id { get; set; }
        public bool Admin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return FirstName + " " + LastName; } }
        public string Course { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public virtual BookedRoom BookedRoom { get; set; }

        internal static void CreateUser(bool admin)
        {
            EditUser(new User(), admin);
        }

        internal static void EditUser(User user, bool admin)
        {
            List<string> question = new() { "No", "Yes" };
            int index = 1;

            user.Admin = false;
            if (admin)
            {
                if ((Menu.MenuList("Set user as admin?", question) == 1)) user.Admin = true;
            }
            while (index != 0)
            {
                index = Menu.MenuList("Edit users FirstName: " + (user.FirstName == null ? "Empty" : user.FirstName), question);
                if (index == 1) user.FirstName = Console.ReadLine();
            }
            index = 1;
            while (index != 0)
            {
                index = Menu.MenuList("Edit users LastName: " + (user.LastName == null ? "Empty" : user.LastName), question);
                if (index == 1) user.LastName = Console.ReadLine();
            }
            index = 1;
            while (index != 0)
            {
                index = Menu.MenuList("Edit users Course: " + (user.Course == null ? "Empty" : user.Course), question);
                if (index == 1) user.Course = Console.ReadLine();
            }
            index = 1;
            while (index != 0)
            {
                index = Menu.MenuList("Edit users Password: " + (user.Password == null ? "Empty" : user.Password), question);
                if (index == 1) user.Password = Console.ReadLine();
            }
            index = 1;
            while (index != 0)
            {
                index = Menu.MenuList("Edit users Email: " + (user.Email == null ? "Empty" : user.Email), question);
                if (index == 1) user.Email = Console.ReadLine();
            }

            index = Menu.MenuList(("Save User: " + user.FullName), question);
            if (index == 1) SaveUser(user);
        }

        internal static void UpdateUser(User user, bool isAdmin)
        {
            if (isAdmin)
            {
                List<User> users = GetUsersFromDb();
                int index = GetUserChoice(users);
                if (index != -1)
                {
                    EditUser(users[index], isAdmin);
                }
            }
            else EditUser(GetSelectedUser(user.Id), isAdmin);
        }

        internal static void RemoveUser()
        {
            string output = "";
            List<User> users = GetUsersFromDb();
            int index = GetUserChoice(users);
            if (index != -1)
            {
                using (var db = new Context())
                {
                    var deleteUser = GetSelectedUser(users[index].Id);
                    if (deleteUser != null)
                    {
                        db.Users.Remove((User)deleteUser);
                        try
                        {
                            db.SaveChanges();
                            output = "Removed user succesfully!";
                        }
                        catch (Exception e)
                        {
                            output = "Could not remove selected user!" + e;
                        }
                    }
                }
                Console.WriteLine(output);
                Console.ReadLine();
                Console.Clear();
            }
        }

        internal static int GetUserChoice(List<User> users)
        {
            if (users.Count < 1)
            {
                Console.WriteLine("There is no Users");
                Console.ReadKey();
                return -1;
            }
            return Menu.MenuList("Select user", Helpers.ConvertUserListToStringList(users));
        }

        internal static List<User> GetUsersFromDb()
        {
            List<User> users = new List<User>();
            using (var db = new Context())
            {
                users = db.Users.ToList();
            }
            return users;
        }

        internal static User GetSelectedUser(int userId)
        {
            using var db = new Context();
            var user = db.Users.FirstOrDefault(c => c.Id == userId);
            return user;
        }

        internal static void SaveUser(User user)
        {
            string saveOutput = "";
            using (var db = new Context())
            {
                var dbUsers = db.Users;
                User dbUser = dbUsers.ToList().SingleOrDefault(a => a.Id == user.Id);
                if (dbUser == null)
                {
                    dbUsers.Add(user);
                }
                else
                {
                    dbUser.Admin = user.Admin;
                    dbUser.FirstName = user.FirstName;
                    dbUser.LastName = user.LastName;
                    dbUser.Course = user.Course;
                    dbUser.Password = user.Password;
                    dbUser.Email = user.Email;
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
