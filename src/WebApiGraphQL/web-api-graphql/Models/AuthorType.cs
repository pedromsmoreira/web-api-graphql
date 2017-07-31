namespace WebApiGraphQL.Models
{
    using GraphQL.Types;

    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Field(x => x.Id).Description("The id of the author.");
            Field(x => x.Name).Description("The name of the author.");
            Field<ListGraphType<BookType>>("books");
        }
    }
}