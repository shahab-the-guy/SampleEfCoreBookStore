using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Domain.Entities;

namespace SampleEfCoreBookStore.Extensions
{
    static class ModelBuilderExtensions
    {
        public static void AddIsDeletedQueryFilter(this ModelBuilder modelBuilder)
        {
            var softDeleteEntities = typeof(ICanBeSoftDeleted).Assembly.GetTypes()
                .Where(type => typeof(ICanBeSoftDeleted).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);

            foreach (var softDeleteEntity in softDeleteEntities)
            {
                modelBuilder.Entity(softDeleteEntity).HasQueryFilter(GenerateQueryFilterLambda(softDeleteEntity));
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
