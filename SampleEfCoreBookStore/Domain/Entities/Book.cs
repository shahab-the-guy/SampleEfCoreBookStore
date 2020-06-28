using System;
using System.Collections.Generic;
using System.Text;
using SampleEfCoreBookStore.Domain.Abstractions;

namespace SampleEfCoreBookStore.Domain.Entities
{
    public class Book : ICanBeSoftDeleted
    {
#pragma warning disable 414
        private Guid _authorId;
#pragma warning restore 414

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
