using Infinion.Domain.Constants;
using Infinion.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infinion.Data.Context
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                 new Product
                 {
                     Id = "f60f6e13-c19a-4cf5-a4a6-5f373ad9af04",
                     Name = "75 INCHES SAMSUNG 4K TV",
                     Category = "Electronic Appliances",
                     Description = "This TV features a slim bezel design, providing a sleek and modern look that will complement any living space.",
                     Brand = "Samsung",
                     Price = 250000.00,
                     AddedBy="Mary Silver"


                 },
                 new Product
                 {
                     Id = "88c0e985-5b44-4cfd-8e5c-a7baaf123b0e",
                     Name = "18-inches Standing Fan, Heavy Breeze",
                     Category = "Electronic Appliances",
                     Description = "Super standing Fan is unique and durable  with a black blade and a black body",
                     Brand = "Binatone",
                     Price = 15000.00,
                     AddedBy = "Mary Silver"


                 },
                 new Product
                 {
                     Id = "529701ca-c4a2-48c4-bad4-e2cd87b53866",
                     Name = "Infinix Smart 8 6.6 inches 3GB RAM/64GB ROM Android T Go - Gold",
                     Category = "Phone",
                     Description = "With the large 5000mAh battery and intelligent power-saving technology, you can enjoy your entertainment all day long without power worries.",
                     Brand = "Infinix",
                     Price = 217000.00,
                     AddedBy = "Mary Gold"


                 }

                 );

            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole { Id = "1", Name = RoleConstant.Admin, NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = RoleConstant.User, NormalizedName = "USER" }
                );

            var adminUser = new AppUser
            {
                Id = "1accc843-1146-4970-b709-5307f31615ff",
                UserName = "Admin@yopmail.com",
                NormalizedUserName = "ADMIN@YOPMAIL.COM",
                Email = "admin@yopmail.com",
                NormalizedEmail = "ADMIN@YOPMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = "166b52c6-a877-4a4b-95cf-44e7cc4ec4aa",
                FirstName= "Admin",
                MiddleName="Admin",
                LastName="Admin",
                PhoneNumber="08139366177",
                Address="Lagos Nigeria",
                CreatedAt=DateTimeOffset.Now,
                UpdatedAt=DateTimeOffset.Now,

                
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin@123"); 

            modelBuilder.Entity<AppUser>().HasData(adminUser);

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = "1", 
                UserId = "1accc843-1146-4970-b709-5307f31615ff",
            });
        }
    }
}
