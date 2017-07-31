namespace WebApiGraphQL.Models.InputTypes
{
    using GraphQL.Types;

    public class AuthorInputType : InputObjectGraphType
    {
        public AuthorInputType()
        {
            this.Name = "AuthorInput";

            //this.Field<NonNullGraphType<StringGraphType>>("isbn");
            this.Field<NonNullGraphType<StringGraphType>>("name");
        }
    }
}