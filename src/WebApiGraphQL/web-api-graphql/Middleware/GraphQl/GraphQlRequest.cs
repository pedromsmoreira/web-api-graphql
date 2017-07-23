namespace WebApiGraphQL.Middleware.GraphQl
{
    internal sealed class GraphQlRequest
    {
        public string OperationName { get; set; }

        public string Query { get; set; }

        public string Variables { get; set; }
    }
}