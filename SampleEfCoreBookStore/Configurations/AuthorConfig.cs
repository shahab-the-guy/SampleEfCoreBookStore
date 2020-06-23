using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleEfCoreBookStore.Domain.Entities;

namespace SampleEfCoreBookStore.Configurations
{
    class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedNever();

            var navigation = builder.Metadata.FindNavigation(nameof(Author.Books));
            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
            navigation.SetField("_someBooks"); // if you have not used the naming conventions
            // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
