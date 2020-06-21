﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SampleEfCoreBookStore.Domain.Entities;
using SampleEfCoreBookStore.Infra;

namespace SampleEfCoreBookStore
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookstoreDbContext>();
            optionsBuilder.UseInMemoryDatabase("Bookstore");

            var context = new BookstoreDbContext(optionsBuilder.Options);

            var me = Author.Create("Saeed", "Ganji", DateTime.Now.AddDays(-1));
            me.AddBook("EF Core" , "132465");

            var other = Author.Create("John", "Doe", DateTime.Now.AddDays(-1));
            other.AddBook("No Idea", "132465");

            context.Authors.AddRange(me,other);

            context.SaveChanges();

            me = null;

            me = context.Authors.FirstOrDefault(x => x.FirstName == "Saeed");

            Console.WriteLine(me.LastName);

        }
    }
}
