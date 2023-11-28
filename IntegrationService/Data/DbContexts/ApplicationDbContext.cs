using IntegrationService.Models.Channels;
using IntegrationService.Models.Fields;
using IntegrationService.Models.Imports;
using IntegrationService.Models.Product;
using IntegrationService.Models.Stores;
using IntegrationService.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using IntegrationService.ConcreteServices;
using System.Security.Claims;
using IntegrationService.Models.Categories;
using IntegrationService.ViewModels.ImportViewModels;
using IntegrationService.Models.Collection;
using IntegrationService.Models.Errors;
using IntegrationService.Models.Nutritions;
using IntegrationService.Models.Exports;
using IntegrationService.Models.ChannelDifferences;

namespace IntegrationService.Data.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>().HasKey(p => new { p.Sku, p.StoreCode , p.ChannelId});
            modelBuilder.Entity<ChannelProductStore>().HasKey(p => p.ProductStoresId);
            modelBuilder.Entity<ProductCharacteristicStores>().HasKey(p => new { p.ProductCharacteristicId, p.StoreId });
            //modelBuilder.Entity<Products>().HasKey(p => p.Sku);
            //modelBuilder.Entity<ProductCharacteristicStores>().HasOne(pt => pt.ProductChars).WithMany(p => p.ProductCharacteristicStore).HasForeignKey(pt=>pt.ProductCharacteristicId);
            //modelBuilder.Entity<ProductCharacteristicStores>().HasOne(pt => pt.Store).WithMany(p => p.ProductCharacteristicStores).HasForeignKey(pt => pt.StoreId);
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(p => new { p.UserId, p.RoleId });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasKey(p => new { p.UserId });
            modelBuilder.Entity<ApplicationUser>().HasData(new
            {
                Id = "386d7e90-eccd-41ee-bd28-a7d5b0ce4209",
                Name = "Panos",
                LastName = "Kossiaras",
                UserName = "pkossiaras@sklavenitis.com",
                NormalizedUserName = "PKOSSIARAS@SKLAVENITIS.COM",
                Email = "pkossiaras@sklavenitis.com",
                NormalizedEmail = "PKOSSIARAS@SKLAVENITIS.COM",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==",
                SecurityStamp = "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF",
                ConcurrencyStamp = "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8",
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new
            {
                UserId = "386d7e90-eccd-41ee-bd28-a7d5b0ce4209",
                Id = 1,
                ClaimType = "IsAdmin",
                ClaimValue = "true"
            });
            var InitUser = modelBuilder.Entity<ApplicationUser>().HasData(new
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Kostas",
                LastName = "priftis",
                UserName = "kpriftis@sklavenitis.com",
                NormalizedUserName = "kpriftis@sklavenitis.com",
                Email = "kpriftis@sklavenitis.com",
                NormalizedEmail = "kpriftis@sklavenitis.com",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEED6s4HmJ7vVDjUDpxALd3n2zZ2JMhvOCqltxbdtJ8FvWf+Md8U520XV/+QB4OPuAA==",
                SecurityStamp = "3WY3LMI4MIAWSMELRQVGE6OCQVYNC5IF",
                ConcurrencyStamp = "6fed6a16-aaf4-4eee-8e0e-c2e657976fc8",
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = false,
                LockoutEnabled = false,
                AccessFailedCount = 0
            });
            modelBuilder.Entity<IdentityUserClaim<string>>().HasData(new
            {
                UserId = Guid.NewGuid().ToString(),
                Id = 1,
                ClaimType = "IsAdmin",
                ClaimValue = "true"
            });
            modelBuilder.Entity<ImportDiffsViewModel>().HasNoKey()
                .ToTable(nameof(ImportDiffsViewModel), t => t.ExcludeFromMigrations());
        }

        public DbSet<Import> Imports { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<ProductCharacteristic> ProductCharacteristics { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<IdentityUserClaim<string>> Claims { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<FieldProducts> FieldProducts { get; set; }
        public DbSet<ProductCharacteristicStores> ProductCharacteristicStores { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Nutrition> Nutritions { get; set; }
        public DbSet<ExportRequest> ExportRequests { get; set; }
        public DbSet<ChannelDifference> ChannelDifferences { get; set; }
        public DbSet<FieldProductStore> FieldProductStores { get; set; }
        public DbSet<ChannelProductStore> ChannelProductStores { get; set; }
        public DbSet<Collection>  Collections { get; set; }
        public DbSet<NutritionProduct> NutritionProducts { get; set; }
    }
}
