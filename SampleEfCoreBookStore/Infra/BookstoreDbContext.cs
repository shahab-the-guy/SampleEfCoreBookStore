using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Configurations;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthorConfig).Assembly);

            modelBuilder.AddIsDeletedQueryFilter();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author> Authors { get; set; }
    }
}