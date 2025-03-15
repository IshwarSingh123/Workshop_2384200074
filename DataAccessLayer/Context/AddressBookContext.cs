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
    }
}