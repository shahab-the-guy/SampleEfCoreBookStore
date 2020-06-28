using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Domain.Entities;
using SampleEfCoreBookStore.Infra;
using static SampleEfCoreBookStore.Constants;

namespace SampleEfCoreBookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var optionsBuilder = new DbContextOptionsBuilder<BookstoreDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            var context = new BookstoreDbContext(optionsBuilder.Options);
            
            context.Database.Migrate();

            var saeedGanji = Author.Create("Saeed", "Ganji", new DateTime(1985, 1, 1));
            context.Authors.Add(saeedGanji);
            context.SaveChanges();
            
            //
            // context.Database.Migrate();
            // context.Database.EnsureCreated();
            //
            // var me = Author.Create("Saeed", "Ganji", DateTime.Now.AddDays(-1));
            // me.AddBook("EF Core" , "132465");
            //
            // var other = Author.Create("John", "Doe", DateTime.Now.AddDays(-1));
            // other.AddBook("No Idea", "132465");
            //
            // context.Authors.AddRange(me,other);
            //
            // context.SaveChanges();

            Author me = null;

            // mention AsNoTracking in the article
            me = context.Authors.AsNoTracking().FirstOrDefault(x => x.FirstName == "Saeed");
            me.AddBook("SampleBook", "1234567-890");

            context.SaveChanges();

            Console.WriteLine(context.Entry(me).Property("CreatedOn").CurrentValue);

        }
    }
}
