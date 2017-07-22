namespace WebApi.Queries
{
    using System;
    using System.Collections.Generic;
    using GraphQL.Types;
    using Models;
    using Repositories;

    public class BooksQuery : ObjectGraphType
    {
        public BooksQuery()
        {
            IBooksRepository bookRepository = new BooksRepository();

            this.Field<BookType>(
                "book",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "isbn" }),
                resolve: context =>
                {
                    var id = context.GetArgument<string>("isbn");
                    return bookRepository.BookByIsbn(id);
                });

            this.Field<ListGraphType<BookType>>(
                "books",
                resolve: context => bookRepository.AllBooks());
        }
    }
}