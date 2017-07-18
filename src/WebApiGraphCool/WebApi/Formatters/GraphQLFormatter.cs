namespace WebApi.Formatters
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using GraphQL;
    using GraphQL.Types;

    public class GraphQLFormatter : BufferedMediaTypeFormatter
    {
        public GraphQLFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/graphql"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(string) || type == typeof(ExecutionResult) || type is IObjectGraphType;
        }

        public override bool CanWriteType(Type type)
        {
            return type == typeof(string) || type == typeof(ExecutionResult) || type is IObjectGraphType;
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger,
            CancellationToken cancellationToken)
        {
            var reader = new StreamReader(readStream);

            var stream = reader.ReadToEnd();

            return stream;
        }
    }
}