namespace WebApiGraphQL.Middleware.GraphQl
{
    internal sealed class GraphiQlRequest
    {
        public string OperationName { get; set; }

        public string Query { get; set; }

        public string Variables { get; set; }
    }
}