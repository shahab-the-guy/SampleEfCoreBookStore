using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Domain.Abstractions;
using SampleEfCoreBookStore.Domain.Entities;

namespace SampleEfCoreBookStore.Extensions
{
    static class ModelBuilderExtensions
    {
        public static void AddIsDeletedQueryFilter(this ModelBuilder modelBuilder)
        {
            var softDeleteEntities = typeof(ICanBeSoftDeleted).Assembly.GetTypes()
                .Where(type => typeof(ICanBeSoftDeleted)
                    .IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var softDeleteEntity in softDeleteEntities)
            {
                modelBuilder.Entity(softDeleteEntity).HasQueryFilter(GenerateQueryFilterLambda(softDeleteEntity));
            }
        }

        public static void AddTrackingDatesShadowProperties(this ModelBuilder modelBuilder)
        {
            var trackingDatesEntities = typeof(IHaveTrackingDates).Assembly.GetTypes()
                .Where(type => typeof(IHaveTrackingDates)
                    .IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var entity in trackingDatesEntities)
            {
                modelBuilder.Entity(entity).Property<DateTimeOffset>("CreatedOn").IsRequired();
                modelBuilder.Entity(entity).Property<DateTimeOffset?>("LastUpdatedOn")
                    .IsRequired(false)
                    .HasColumnName("LastUpdatedAt");
            }
        }

        private static LambdaExpression GenerateQueryFilterLambda(Type type)
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
    }
}
