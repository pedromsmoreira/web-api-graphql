﻿namespace WebApiGraphQL.Middleware.GraphiQl
{
    using System;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;

    public sealed class GraphiQlMiddlewareV2
    {
        private static readonly string Template = ReadTemplate();
        private readonly string GraphiqlPath;
        private readonly string GraphqlPath;
        private readonly RequestDelegate Next;

        public GraphiQlMiddlewareV2(RequestDelegate next, IOptions<GraphiQlOptionsV2> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            this.Next = next ?? throw new ArgumentNullException(nameof(next));
            var optionsValue = options.Value;
            this.GraphiqlPath = string.IsNullOrEmpty(optionsValue?.GraphiQlPath) ? GraphiQlOptionsV2.DefaultGraphiQlPath : optionsValue.GraphiQlPath;
            this.GraphqlPath = string.IsNullOrEmpty(optionsValue?.GraphQlPath) ? GraphiQlOptionsV2.DefaultGraphQlPath : optionsValue.GraphQlPath;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (this.IsGraphiQlRequest(context.Request))
            {
                await this.WriteResponseAsync(context.Response).ConfigureAwait(true);
                return;
            }

            await this.Next(context).ConfigureAwait(true);
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
        }

        private bool IsGraphiQlRequest(HttpRequest request)
        {
            return string.Equals(request.Method, "GET", StringComparison.OrdinalIgnoreCase) && request.Path.StartsWithSegments(GraphiqlPath);
        }

        private Task WriteResponseAsync(HttpResponse response)
        {
            response.ContentType = "text/html";
            response.StatusCode = (int)HttpStatusCode.OK;

            var builder = new StringBuilder(Template);
            builder.Replace("@{GraphQLPath}", GraphqlPath);

            var data = Encoding.UTF8.GetBytes(builder.ToString());
            return response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}