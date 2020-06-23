using System;
using System.Collections.Generic;
using System.Text;

namespace SampleEfCoreBookStore.Domain.Entities
{
    public class Book : ICanBeSoftDeleted
    {
        private Guid _authorId;

        private Book()
        {
        }
        internal static Book ForAuthor(Guid authorId, string title, string isbn)
            => new Book
            {
                ISBN = isbn,
                Title = title,
                _authorId = authorId
            };


        public long Id { get; private set; }
        public string ISBN { get; private set; }
        public string Title { get; private set; }
        public bool IsDeleted { get; private set; }
        internal void Archive() => IsDeleted = true;
    }
}