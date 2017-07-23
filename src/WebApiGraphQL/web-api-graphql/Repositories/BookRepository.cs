namespace WebApiGraphQL.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;

    public class BookRepository : IBookRepository
    {
        private static IEnumerable<Book> books = new List<Book>();
        private static IEnumerable<Author> authors = new List<Author>();
        private static IEnumerable<Publisher> publisher = new List<Publisher>();

        public BookRepository()
        {
            if (books.Count() == 0)
            {
                books = Book.GetBooks(10).ToList();
                authors = books.Select(book => book.Author).ToList();
                publisher = books.Select(book => book.Publisher).ToList();
            }
        }

        public IEnumerable<Author> AllAuthors()
        {
            return authors;
        }

        public Book BookBy(Func<Book, bool> predicate)
        {
            return books.First(predicate);
        }

        public IEnumerable<Book> AllBooks()
        {
            return books;
        }

        public IEnumerable<Publisher> AllPublishers()
        {
            return publisher;
        }

        public Author AuthorById(int id)
        {
            return authors.First(_ => _.Id == id);
        }

        public Book BookByIsbn(string isbn)
        {
            return books.First(_ => _.Isbn == isbn);
        }

        public Publisher PublisherById(int id)
        {
            return publisher.First(_ => _.Id == id);
        }
    }
}