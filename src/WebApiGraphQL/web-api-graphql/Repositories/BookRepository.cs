﻿namespace WebApiGraphQL.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Distributed;
    using Models;

    public class BookRepository : IBookRepository
    {
        private static ICollection<Book> books = new List<Book>();
        private static IEnumerable<Author> authors = new List<Author>();
        private static IEnumerable<Publisher> publisher = new List<Publisher>();

        //private readonly IDistributedCache DistributedCache;

        public BookRepository(/*IDistributedCache distributedCache*/)
        {
            //this.DistributedCache = distributedCache;

            if (books.Any())
            {
                return;
            }

            books = Book.GetBooks(10).ToList();
            authors = books.Select(book => book.Author).ToList();
            publisher = books.Select(book => book.Publisher).ToList();
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

        public Book AddBook(Book book)
        {
            book.Isbn = Guid.NewGuid().ToString();

            book.Author = authors.First();
            book.Publisher = publisher.First();

            books.Add(book);

            return book;
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