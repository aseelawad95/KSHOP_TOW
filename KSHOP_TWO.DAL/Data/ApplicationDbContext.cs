using KSHOP_TWO.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Category> Categories { get; set; }

        public DbSet<CategoryTranslation> CategoryTranslation { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductTranslation> ProductTranslation { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor  
            ) : base(options)
        {
           _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // fluent api
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<Category>()
    .HasOne(p => p.CreatedBy)
    .WithMany()
    .HasForeignKey(p => p.CreatedById)
    .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }


        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker.Entries<AuditableEntity>();
        //    var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    foreach (var entity in entries)
        //    {
        //       if(entity.State == EntityState.Added)
        //        {
        //            entity.Property(x => x.CreatedById).CurrentValue = currentUserId;
        //            entity.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
        //        }

        //        if (entity.State == EntityState.Modified)
        //        {
        //            entity.Property(x => x.UpdatedById).CurrentValue = currentUserId;
        //            entity.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();

            var currentUserId = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entity in entries)
            {
                if (entity.State == EntityState.Added)
                {
                    entity.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;

                    if (currentUserId != null)
                        entity.Property(x => x.CreatedById).CurrentValue = currentUserId;
                }

                if (entity.State == EntityState.Modified)
                {
                    entity.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;

                    if (currentUserId != null)
                        entity.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
