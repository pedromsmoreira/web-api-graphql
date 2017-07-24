namespace WebApiGraphQL.Middleware.GraphiQl
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
    using Microsoft.Extensions.Options;

    public sealed class GraphiQlMiddleware
    {
        private static readonly string Template = ReadTemplate();
        private readonly string graphiqlPath;
        private readonly string graphqlPath;
        private readonly RequestDelegate next;

        public GraphiQlMiddleware(RequestDelegate next, IOptions<GraphiQlOptions> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.next = next ?? throw new ArgumentNullException(nameof(next));

            var graphiOptions = options.Value;

            this.graphiqlPath = string.IsNullOrEmpty(graphiOptions?.GraphiQLPath)
                ? GraphiQlOptions.DefaultGraphiQLPath
                : graphiOptions.GraphiQLPath;

            this.graphqlPath = string.IsNullOrEmpty(graphiOptions?.GraphQLPath)
                ? GraphiQlOptions.DefaultGraphQLPath
                : graphiOptions.GraphQLPath;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var validRequest = string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase);
            var validPath = context.Request.Path.Equals(this.graphiqlPath);

            if (validRequest && validPath)
            {
                await this.WriteResponseAsync(context.Response).ConfigureAwait(true);
            }

            await this.next(context).ConfigureAwait(true);
        }

        private Task WriteResponseAsync(HttpResponse contextResponse)
        {
            contextResponse.ContentType = "text/html";
            contextResponse.StatusCode = 200;

            var builder = new StringBuilder(Template);
            builder.Replace("@{GraphQLPath}", this.graphqlPath);

            var data = Encoding.UTF8.GetBytes(builder.ToString());

            return contextResponse.Body.WriteAsync(data, 0, data.Length);

            //TODO: User RazorView instead of having this filthy string.
        }

        private static string ReadTemplate()
        {
            return @"
                <!--
                 *  Copyright (c) 2015, Facebook, Inc.
                 *  All rights reserved.
                 *
                 *  This source code is licensed under the license found in the
                 *  LICENSE file in the root directory of this source tree.
                 *
                -->
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {
                            height: 100%;
                            margin: 0;
                            width: 100%;
                            overflow: hidden;
                        }
                        #graphiql {
                            height: 100vh;
                        }
                    </style>
                    <title>GraphiQL</title>
                    <link rel='stylesheet' href='https://cdn.jsdelivr.net/graphiql/0.7.5/graphiql.css' />
                    <script src='https://cdn.jsdelivr.net/fetch/latest/fetch.min.js'></script>
                    <script src='https://cdn.jsdelivr.net/react/latest/react.min.js'></script>
                    <script src='https://cdn.jsdelivr.net/react/latest/react-dom.min.js'></script>
                    <script src='https://cdn.jsdelivr.net/graphiql/0.7.5/graphiql.min.js'></script>
                </head>
                <body>
                    <div id='graphiql'>Loading...</div>
                    <script>
                      /**
                       * This GraphiQL example illustrates how to use some of GraphiQL's props
                       * in order to enable reading and updating the URL parameters, making
                       * link sharing of queries a little bit easier.
                       *
                       * This is only one example of this kind of feature, GraphiQL exposes
                       * various React params to enable interesting integrations.
                       */
                      // Parse the search string to get url parameters.
                      var search = window.location.search;
                      var parameters = {};
                      search.substr(1).split('&').forEach(function (entry) {
                        var eq = entry.indexOf('=');
                        if (eq >= 0) {
                          parameters[decodeURIComponent(entry.slice(0, eq))] =
                            decodeURIComponent(entry.slice(eq + 1));
                        }
                      });
                      // if variables was provided, try to format it.
                      if (parameters.variables) {
                        try {
                          parameters.variables =
                            JSON.stringify(JSON.parse(parameters.variables), null, 2);
                        } catch (e) {
                          // Do nothing, we want to display the invalid JSON as a string, rather
                          // than present an error.
                        }
                      }
                      // When the query and variables string is edited, update the URL bar so
                      // that it can be easily shared
                      function onEditQuery(newQuery) {
                        parameters.query = newQuery;
                        updateURL();
                      }
                      function onEditVariables(newVariables) {
                        parameters.variables = newVariables;
                        updateURL();
                      }
                      function onEditOperationName(newOperationName) {
                        parameters.operationName = newOperationName;
                        updateURL();
                      }
                      function updateURL() {
                        var newSearch = '?' + Object.keys(parameters).filter(function (key) {
                          return Boolean(parameters[key]);
                        }).map(function (key) {
                          return encodeURIComponent(key) + '=' +
                            encodeURIComponent(parameters[key]);
                        }).join('&');
                        history.replaceState(null, null, newSearch);
                      }
                      // Defines a GraphQL fetcher using the fetch API.
                      function graphQLFetcher(graphQLParams) {
                        return fetch(window.location.origin + '@{GraphQLPath}', {
                          method: 'post',
                          headers: {
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                          },
                          body: JSON.stringify(graphQLParams),
                          credentials: 'include',
                        }).then(function (response) {
                          return response.text();
                        }).then(function (responseBody) {
                          try {
                            return JSON.parse(responseBody);
                          } catch (error) {
                            return responseBody;
                          }
                        });
                      }
                      // Render <GraphiQL /> into the body.
                      ReactDOM.render(
                        React.createElement(GraphiQL, {
                          fetcher: graphQLFetcher,
                          query: parameters.query,
                          variables: parameters.variables,
                          operationName: parameters.operationName,
                          onEditQuery: onEditQuery,
                          onEditVariables: onEditVariables,
                          onEditOperationName: onEditOperationName
                        }),
                        document.getElementById('graphiql')
                      );
                    </script>
                </body>
                </html>
                ";
            //var assembly = typeof(IApplicationBuilderExtensions).GetTypeInfo().Assembly;
            //var a = assembly.GetManifestResourceNames();
            //using (var stream = assembly.GetManifestResourceStream("index.html"))
            //using (var streamReader = new StreamReader(stream , Encoding.UTF8))
            //{
            //    return streamReader.ReadToEnd();
            //}
        }
    }
}