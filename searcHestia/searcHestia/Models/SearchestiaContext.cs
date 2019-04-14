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
	public class SearchestiaContext : DbContext
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VacProperty>()
                .HasMany(c => c.Amentities).WithMany(i => i.VacProperties)
                .Map(t => t.MapLeftKey("VacPropertyId")
                    .MapRightKey("AmentityId")
                    .ToTable("PropertyAmentity"));
        }
    }
}