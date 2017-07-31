namespace WebApiGraphQL.Schema
{
    using System;
    using GraphQL.Types;
    using Queries;

    public class LibrarySchema : Schema
    {
        public LibrarySchema(Func<Type, GraphType> resolveType)
            : base(resolveType)
        {
            Query = (BooksQuery) resolveType(typeof(BooksQuery));
            Mutation = (BooksMutation) resolveType(typeof(BooksMutation));
        }
    }
}