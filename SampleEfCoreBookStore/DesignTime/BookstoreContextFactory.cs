using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using SampleEfCoreBookStore.Infra;
using static SampleEfCoreBookStore.Constants;

namespace SampleEfCoreBookStore.DesignTime
{
    public class BookstoreContextFactory : IDesignTimeDbContextFactory<BookstoreDbContext>
    {
        public BookstoreDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BookstoreDbContext>();
            optionsBuilder.UseSqlServer(ConnectionString);

            return new BookstoreDbContext(optionsBuilder.Options);
        }
    }
}
