namespace WebApiGraphQL.Repositories
{
    using System.Collections.Generic;
    using Models;

    public interface IBooksRepository
    {
        Book BookByIsbn(string isbn);

        IEnumerable<Book> AllBooks();

        Author AuthorById(int id);

        IEnumerable<Author> AllAuthors();

        Publisher PublisherById(int id);

        IEnumerable<Publisher> AllPublishers();
    }
}