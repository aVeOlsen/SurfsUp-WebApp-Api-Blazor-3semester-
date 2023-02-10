using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProtoTypeV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoTypeV1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Spot> Spot { get; set; }
        public DbSet<Rental> Rental { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductEquipment> ProductEquipments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<Location> Locations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Address>().HasKey(c => c.ID);
            builder.Entity<User>().HasOne(c => c.Address);
            builder.Entity<Spot>().HasKey(c => c.SpotID);
            builder.Entity<User>().HasMany(c => c.Spots);
            builder.Entity<Rental>().HasKey(c => c.RentalID);
            builder.Entity<Product>().HasKey(c => c.ProductID);
            builder.Entity<Product>().HasOne(c => c.ByUser);
            builder.Entity<ProductEquipment>().HasKey(c => c.EquipmentID);
            builder.Entity<Rental>().HasMany(c => c.Equipments);
            builder.Entity<Rental>().HasMany(c => c.Product).WithMany(c=>c.Rentals);
            builder.Entity<Utility>().HasKey(c => c.UtilityID);
            builder.Entity<Utility>().HasOne(c => c.Spot);
            builder.Entity<Review>().HasKey(c => c.ReviewID);
            builder.Entity<Review>().HasOne(c => c.ByUser);
            builder.Entity<Location>().HasKey(c => c.LocationID);
            builder.Entity<Spot>().HasMany(c => c.Reviews);
            builder.Entity<Rental>().HasOne(c => c.RentedOutBy);
            builder.Entity<Rental>().HasMany(c => c.Reviews);

        }

    }
}
