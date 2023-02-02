using Bokningsappen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Bokningsappen
{
    internal class Security
    {
        internal static User UserLogin()
        {
            User user = new User();
            do
            {
                user = Login();
            } while (user == null);

            return user;
        }

        internal static bool CheckAdminPrivileges(User user)
        {
            return user.Admin;
        }

        private static User Login()
        {
            Console.WriteLine("Input Firstname");
            string firstName = Console.ReadLine();
            Console.WriteLine("Input Last name");
            string lastName = Console.ReadLine();
            Console.WriteLine("Input Password");
            string password = Console.ReadLine();

            string output = "Name and password has no match!";
            User user = new User();
            try
            {
                user = GetSelectedUserFromPassword(firstName, lastName, password);
            }
            catch (Exception ex)
            {
                output += ex.Message;
            }
            if (user == null || user.Id == 0)
            {
                Console.WriteLine(output);
                Console.ReadLine();
            }
            Console.Clear();
            return user;
        }
        internal static User GetSelectedUserFromPassword(string firstName, string lastName, string password)
        {
            using var db = new Context();
            var user = db.Users.FirstOrDefault(u => u.FirstName == firstName && u.LastName == lastName && u.Password == password);
            return user;
        }
    }
}