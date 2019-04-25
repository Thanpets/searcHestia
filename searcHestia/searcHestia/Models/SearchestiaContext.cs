using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace searcHestia.Models
{
    /// <summary>
	/// Used to access and persist data to the database
	/// </summary>
	public class SearchestiaContext : IdentityDbContext<ApplicationUser>
    {
        // We need to replace this value with the appropriate value that points to the local database
        const string connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=searcHestia;Integrated Security=True";

        public SearchestiaContext() : base(connectionString) { }

        /// <summary>
        /// Collection managing regions
        /// </summary>
        public DbSet<Region> Regions { get; set; }

        /// <summary>
        /// Collection managing locations
        /// </summary>
        public DbSet<Location> Locations { get; set; }

        /// <summary>
        /// Collection managing amentities
        /// </summary>
        public DbSet<Amentity> Amentities { get; set; }

        /// <summary>
        /// Collection managing vacation properties
        /// </summary>
        public DbSet<VacProperty> VacProperties { get; set; }

        /// <summary>
        /// Collection managing availabilities
        /// </summary>
        public DbSet<Availability> Availabilities { get; set; }

        /// <summary>
        /// Collection managing ratings
        /// </summary>
        public DbSet<Rating> Ratings { get; set; }

        /// <summary>
        /// Collection managing rating's categories
        /// </summary>
        public DbSet<RatCategory> RatCategories { get; set; }

        /// <summary>
        /// Collection managing reservations
        /// </summary>
        public DbSet<Reservation> Reservations { get; set; }

        /// <summary>
        /// Collection managing pricing
        /// </summary>
        public DbSet<Pricing> Pricings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VacProperty>()
                .HasMany(c => c.Amentities).WithMany(i => i.VacProperties)
                .Map(t => t.MapLeftKey("VacPropertyId")
                    .MapRightKey("AmentityId")
                    .ToTable("PropertyAmentity"));

            modelBuilder.Entity<VacProperty>()
                 .HasRequired(c => c.ApplicationUser)
                 .WithMany(t => t.VacProperties)
                 .Map(m => m.MapKey("UserId"))
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reservation>()
                 .HasRequired(c => c.ApplicationUser)
                 .WithMany(t => t.Reservations)
                 .Map(m => m.MapKey("UserId"))
                 .WillCascadeOnDelete(false);

            /* modelBuilder.Entity<Rating>()
            .HasIndex(r => r.ReservationId).IsUnique(); */

            modelBuilder.Entity<Reservation>()
                 .HasOptional(r => r.Rating)
                 .WithRequired(ra => ra.Reservation)
                 .Map(c => c.MapKey("ReservationId"));
                 //using different FK in the dependent entity than the PK
                 //other approach: Shared PK Association

            base.OnModelCreating(modelBuilder);
        }
    }
}