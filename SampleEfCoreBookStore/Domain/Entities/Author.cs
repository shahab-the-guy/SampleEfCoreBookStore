using System;
using System.Collections.Generic;
using System.Text;
using static SampleEfCoreBookStore.Domain.Entities.Book;

namespace SampleEfCoreBookStore.Domain.Entities
{
    public class Author : ICanBeSoftDeleted
    {
        private Author()
        {
            _someBooks = new List<Book>();
        }

        public static Author Create(string firstname, string lastname, DateTime birthdate)
        {
            if (birthdate > DateTime.Now)
                throw new ArgumentOutOfRangeException($"The {nameof(birthdate)} should not be for future");

            var author = new Author
            {
                Id = Guid.NewGuid(),

                FirstName = firstname ?? throw new ArgumentNullException(nameof(firstname)),
                LastName = lastname ?? throw new ArgumentNullException(nameof(lastname)),

                DateOfBirth = birthdate,

                IsDeleted = false
            };

            return author;
        }

        public void Deactivate() => IsDeleted = true;
        public void ArchiveBook(Book b) => b.Archive();

        private readonly List<Book> _someBooks;
        public IEnumerable<Book> Books => _someBooks;

        public void AddBook(string title, string isbn)
            => _someBooks.Add(ForAuthor(this.Id, title, isbn));

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public bool IsDeleted { get; private set; }
    }
}