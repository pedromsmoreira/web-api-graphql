namespace WebApiGraphQL.Middleware.GraphiQl
{
    public sealed class GraphiQlOptions
    {
        // TODO: REMOVE THIS CONSTS THEY CAUSE PROBLEMS!!!!
        public const string DefaultGraphiQLPath = "/graphiql";
        public const string DefaultGraphQLPath = "/graphql";

        public GraphiQlOptions()
        {
            this.GraphiQLPath = DefaultGraphiQLPath;
            this.GraphQLPath = DefaultGraphQLPath;
        }

        public string GraphiQLPath { get; set; }

        public string GraphQLPath { get; set; }
    }
}