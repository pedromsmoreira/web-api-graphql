namespace WebApiGraphQL.Middleware.GraphiQl
{
    public sealed class GraphiQlOptionsV2
    {
        public const string DefaultGraphiQlPath = "/graphiql";
        public const string DefaultGraphQlPath = "/graphql";

        public GraphiQlOptionsV2()
        {
            this.GraphiQlPath = DefaultGraphiQlPath;
            this.GraphQlPath = DefaultGraphQlPath;
        }

        public string GraphiQlPath { get; set; }

        public string GraphQlPath { get; set; }
    }
}