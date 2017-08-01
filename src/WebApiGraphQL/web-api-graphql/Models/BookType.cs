namespace WebApiGraphQL.Models
{
    using GraphQL.Types;

    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            this.Field(x => x.Isbn).Description("The isbn of the book.");
            this.Field(x => x.Name).Description("The name of the book.");
            this.Field<AuthorType>("author");
        }
    }
}