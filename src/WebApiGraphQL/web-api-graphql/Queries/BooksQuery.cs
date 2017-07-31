namespace WebApiGraphQL.Queries
{
    using GraphQL.Types;
    using Models;
    using Repositories;

    public class BooksQuery : ObjectGraphType
    {
        public BooksQuery(IBookRepository bookRepository)
        {
            //{
            //    book(name: "The law suit"){
            //        isbn,
            //        name,
            //        author{
            //            name
            //        }
            //    }
            //}
            this.Field<BookType>(
                "book",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "name" }),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    return bookRepository.BookBy(b => b.Name.Equals(name));
                });

            //{
            //    books{
            //        name,
            //        isbn,
            //        author{
            //            name
            //        }
            //    }
            //}

            this.Field<ListGraphType<BookType>>(
                "books",
                resolve: context => bookRepository.AllBooks());

            //{
            //    authors{
            //        id,
            //        name,
            //        books{
            //            isbn
            //                name
            //        }
            //    }
            //}
            this.Field<ListGraphType<AuthorType>>(
                "authors",
                resolve: context => bookRepository.AllAuthors());

            
            this.Field<ListGraphType<AuthorType>>(
                "author",
                arguments: new QueryArguments(new QueryArgument<IntGraphType>{ Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return bookRepository.AuthorById(id);
                });
        }
    }
}