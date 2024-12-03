using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Doantour.Models
{
    public class Hachutravelcontext : IdentityDbContext<AppUser>
    {
        public DbSet<Tour> Tour { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Booking> Booking { get; set; }

        public DbSet<Country> Country { get; set; }
        public DbSet<Hotel> Hotel { get; set; }


        public DbSet<TourHotel> TourHotel { get; set; }

        public Hachutravelcontext(DbContextOptions<Hachutravelcontext> options) : base(options)
        {

        }

        public Hachutravelcontext()
        {
        }
        
    }
}
