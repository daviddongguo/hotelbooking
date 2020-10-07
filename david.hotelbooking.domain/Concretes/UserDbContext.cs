using david.hotelbooking.domain.Entities.RBAC;
using Microsoft.EntityFrameworkCore;

namespace david.hotelbooking.domain.Concretes
{
    public class UserDbContext : DbContext
    {
        public UserDbContext() : base()
        {

        }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<RolePermission> RolePermissions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // composite key
            modelBuilder.Entity<UserRole>()
                .HasKey(u => new { u.UserId, u.RoleId });
            modelBuilder.Entity<RolePermission>()
                .HasKey(r => new { r.RoleId, r.PermissionId });

            // Seeding
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "Admin@hotel.com", Password = "aaa" },
                new User { Id = 2, Email = "Sis@hotel.com", Password = "aaa" }
                );
            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { UserId = 1, RoleId = 1 },
                new UserRole { UserId = 2, RoleId = 4 });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "admin", Description = "administrator" },
                new Role { Id = 2, Name = "marketing", Description = "" },
                new Role { Id = 3, Name = "receptionist", Description = "" },
                new Role { Id = 4, Name = "customer", Description = "" }
                );
            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = 1, PermissionId = 1 },
                new RolePermission { RoleId = 1, PermissionId = 2 },
                new RolePermission { RoleId = 1, PermissionId = 3 },
                new RolePermission { RoleId = 1, PermissionId = 4 },
                new RolePermission { RoleId = 4, PermissionId = 1 },
                new RolePermission { RoleId = 4, PermissionId = 3 }
                );

            modelBuilder.Entity<Permission>().HasData(
                new Role { Id = 1, Name = "readOrder", Description = "" },
                new Role { Id = 2, Name = "writeOrder", Description = "" },
                new Role { Id = 3, Name = "readUser", Description = "" },
                new Role { Id = 4, Name = "writeUser", Description = "" }
                );

        }
    }
}
