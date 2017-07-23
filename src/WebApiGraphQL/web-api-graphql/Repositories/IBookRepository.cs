namespace WebApiGraphQL.Repositories
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IBookRepository
    {
        Book BookByIsbn(string isbn);

        Book BookBy(Func<Book, bool> predicate);

        IEnumerable<Book> AllBooks();

        Author AuthorById(int id);

        IEnumerable<Author> AllAuthors();

        Publisher PublisherById(int id);

        IEnumerable<Publisher> AllPublishers();
    }
}