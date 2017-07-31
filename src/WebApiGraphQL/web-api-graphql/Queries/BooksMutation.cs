namespace WebApiGraphQL.Queries
{
    using GraphQL.Types;
    using Models;
    using Models.InputTypes;
    using Repositories;

    public class BooksMutation : ObjectGraphType<object>
    {
        private readonly IBookRepository BookRepository;

        public BooksMutation(IBookRepository bookRepository)
        {
            this.BookRepository = bookRepository;
            Name = "Mutation";

            this.Field<BookType>(
                "addBook",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<BookInputType>>
                    {
                        Name = "book"
                    }),
                resolve: context =>
                {
                    var book = context.GetArgument<Book>("book");
                    return this.BookRepository.AddBook(book);
                });
        }
    }
}