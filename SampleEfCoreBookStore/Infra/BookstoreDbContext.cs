using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Domain.Entities;

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
            modelBuilder.Entity<Author>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id)
                    .IsRequired()
                    .ValueGeneratedNever();

                var navigation = builder.Metadata.FindNavigation(nameof(Author.Books));
                // DDD Patterns comment:
                //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
                navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Book>(builder =>
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).IsRequired();

                //builder.Property(o => o.Id)
                //    .UseHiLo("booksseq");

                builder.Property<Guid>("_authorId")
                    .IsRequired()
                    .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            AddIsDeletedQueryFilter(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void AddIsDeletedQueryFilter(ModelBuilder modelBuilder)
        {
            var softDeleteEntities = typeof(ICanBeSoftDeleted).Assembly.GetTypes()
                .Where(type => typeof(ICanBeSoftDeleted).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var softDeleteEntity in softDeleteEntities)
            {
                modelBuilder.Entity(softDeleteEntity).HasQueryFilter(GenerateQueryFilterLambda(softDeleteEntity));
            }
        }

        private LambdaExpression GenerateQueryFilterLambda(Type type)
        {
            // we should generate:  e => e.IsDeleted == false
            // or: e => !e.IsDeleted

            // e =>
            var parameter = Expression.Parameter(type, "e"); 
            
            // e.IsDeleted
            var propertyAccess = Expression.PropertyOrField(parameter, nameof(ICanBeSoftDeleted.IsDeleted));

            // !e.IsDeleted
            var notExpression = Expression.Not(propertyAccess);

            // e => e.IsDeleted == false
            var lambda = Expression.Lambda(notExpression, parameter); 

            return lambda;
        }

        public DbSet<Author> Authors { get; set; }
    }
}