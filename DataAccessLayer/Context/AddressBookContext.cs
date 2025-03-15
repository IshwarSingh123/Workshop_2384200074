using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context
{
    public class AddressBookContext : DbContext
    {
        // Your existing constructor
        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options)
        {
        }

        // Add this constructor for migrations ONLY
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-LVBKDOQ\\SQLEXPRESS;Database=AddressBookDatabase;Integrated Security=True;TrustServerCertificate=True");
            }
        }

        public virtual DbSet<Entity.AddressBookEntity> AddressBook { get; set; }
        public virtual DbSet<Entity.UserEntity> UserData { get; set; }
    }
}