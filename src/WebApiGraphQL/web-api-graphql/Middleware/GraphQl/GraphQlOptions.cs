namespace WebApiGraphQL.Middleware.GraphQl
{
    using GraphQL.Types;

    public sealed class GraphQlOptions
    {
        public const string DefaultPath = "/graphql";

        public GraphQlOptions()
        {
            this.GraphQlPath = DefaultPath;
        }

        public GraphQlOptions(string path, ISchema schema)
        {
            this.GraphQlPath = path;
            this.Schema = schema;
        }

        public string GraphQlPath { get; set; }

        public ISchema Schema { get; set; }
    }
}