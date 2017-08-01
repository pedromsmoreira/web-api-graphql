namespace WebApiGraphQL.Models
{
    using GraphQL.Types;

    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            this.Field(x => x.Id).Description("The id of the author.");
            this.Field(x => x.Name).Description("The name of the author.");
            this.Field<ListGraphType<BookType>>("books");
        }
    }
}