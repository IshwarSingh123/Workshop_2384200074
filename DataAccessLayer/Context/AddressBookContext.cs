using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class AddressBookContext : DbContext
    {
        // Your existing constructor
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {
        }
        public virtual DbSet<Entity.AddressBookEntity> AddressBook { get; set; }
        public virtual DbSet<Entity.UserEntity> UserData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API for defining the One-to-Many relationship
            modelBuilder.Entity<AddressBookEntity>()
                .HasOne(g => g.User)
                .WithMany(u => u.AddressBook)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: Delete greetings when user is deleted
        }
    }
}