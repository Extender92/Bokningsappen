using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Bokningsappen.Models;
using Microsoft.EntityFrameworkCore;

namespace Bokningsappen
{
    internal class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=BookingDB;Trusted_Connection=True;");
        }

        //Packet Manager
        // add-migration first
        // update-database

        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookedRoom> BookedRooms { get; set; }
    }
}
