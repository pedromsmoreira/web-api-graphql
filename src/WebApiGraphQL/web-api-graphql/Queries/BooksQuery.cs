namespace WebApiGraphQL.Queries
{
    using GraphQL.Types;
    using Models;
    using Repositories;

    public class BooksQuery : ObjectGraphType
    {
        public BooksQuery(IBookRepository bookRepository)
        {
            this.Field<BookType>(
                "book",
                arguments: new QueryArguments(new QueryArgument<StringGraphType>{ Name = "name" }),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    return bookRepository.BookBy(b => b.Name.Equals(name));
                });

            this.Field<ListGraphType<BookType>>(
                "books",
                resolve: context => bookRepository.AllBooks());
        }
    }
}