using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Configurations;
using SampleEfCoreBookStore.Domain.Abstractions;
using SampleEfCoreBookStore.Domain.Entities;
using SampleEfCoreBookStore.Extensions;

namespace SampleEfCoreBookStore.Infra
{
    public class BookstoreDbContext : DbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options)
            : base(options)
        {
        }


        public override int SaveChanges()
        {
            this.UpdateTrackingDates();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            this.UpdateTrackingDates();
            
            return base.SaveChangesAsync(cancellationToken);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfig).Assembly);

            modelBuilder.AddIsDeletedQueryFilter();
            
            modelBuilder.AddTrackingDatesShadowProperties();

            base.OnModelCreating(modelBuilder);
        }
        private void UpdateTrackingDates()
        {
            var trackingDatesChangedEntries = this.ChangeTracker.Entries()
                .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified
                    && entry.Entity is IHaveTrackingDates
                );

            foreach (var entry in trackingDatesChangedEntries)
            {
                if (entry.State == EntityState.Modified)
                    this.Entry(entry.Entity).Property("LastUpdatedOn").CurrentValue = DateTimeOffset.UtcNow;
                else if (entry.State == EntityState.Added)
                    this.Entry(entry.Entity).Property("CreatedOn").CurrentValue = DateTimeOffset.UtcNow;
            }
        }

        public DbSet<Author> Authors { get; set; }
    }
}
