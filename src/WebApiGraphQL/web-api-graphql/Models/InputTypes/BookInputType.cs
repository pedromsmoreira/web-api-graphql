namespace WebApiGraphQL.Models.InputTypes
{
    using GraphQL.Types;

    public class BookInputType : InputObjectGraphType
    {
        public BookInputType()
        {
            this.Name = "BookInput";

            //this.Field<NonNullGraphType<StringGraphType>>("isbn");
            this.Field<NonNullGraphType<StringGraphType>>("name");
        }
    }
}