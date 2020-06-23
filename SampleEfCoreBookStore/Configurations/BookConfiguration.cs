using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleEfCoreBookStore.Domain.Entities;

namespace SampleEfCoreBookStore.Configurations
{
    class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .UseHiLo("bookseq")
                .IsRequired();

            // ******************************************************
            // if you have not used the conventions and not used the commented approach in Author configuration
            // ##############################
            // var navigation = builder.Metadata.FindNavigation(nameof(Author.Books));
            // navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
            // ##############################
            builder.Property<Guid>("_authorId")
                .IsRequired()
                .HasColumnName("AuthorId")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
            builder.HasOne<Author>()
                .WithMany(b => b.Books)
                .HasForeignKey("_authorId");
            // ******************************************************
        }
    }
}
