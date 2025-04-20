namespace HotelManagement.WebApi.Data
{
    using HotelManagement.WebApi.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Tbl_OrderMaster> Tbl_OrderMaster { get; set; }
        public DbSet<Tbl_MenuItem> Tbl_MenuItem { get; set; }
        public DbSet<Tbl_MenuItemTypes> Tbl_MenuItemTypes { get; set; }
        public DbSet<Tbl_User> Tbl_User { get; set; }
        public DbSet<Tbl_UserType> Tbl_UserType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly set primary key for Tbl_MenuItem
            modelBuilder.Entity<Tbl_MenuItem>()
                .HasKey(m => m.MenuItemId);

            modelBuilder.Entity<Tbl_OrderMaster>()
                .HasKey(o => o.OrderId);

            // Add more model configurations if needed...
        }
    }

}
