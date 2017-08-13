# web-api-graphql

[![CodeFactor](https://www.codefactor.io/repository/github/pedromsmoreira/web-api-graphql/badge)](https://www.codefactor.io/repository/github/pedromsmoreira/web-api-graphql)

## How to use

### For now use Postman

Make a POST Request to http://{your-host}:{port}/graph (in aspnetcore web api) or http://{your-host}:{port}/api/graph (in .NET Framework api)
with the following body:
```
{
  books{
    isbn,
    name,
    author{
      id,
      name
    }
  }
}

```

The result will be:

```
{
    "data": {
        "books": [
            {
                "isbn": "46c7e444-dbe1-4232-adb4-6035cd8649cf",
                "name": "The Cryptic Message",
                "author": {
                    "id": 1,
                    "name": "Freddy John"
                }
            },
            ...
            {
                "isbn": "b8866783-704d-43fd-8a9a-68d5a5d1fa9e",
                "name": "Peter's Hand",
                "author": {
                    "id": 9,
                    "name": "James Paul"
                }
            },
            {
                "isbn": "dec9c922-9d68-457a-9f57-02a40b4a32ac",
                "name": "The Cryptic Message",
                "author": {
                    "id": 10,
                    "name": "David James"
                }
            }
        ]
    }
}

```

Search a Book by name:

```
{
  book(name: "The Cryptic Message"){
    isbn,
    name,
    author{
      id,
      name
    }
  }
}

```

Result:

```
{
    "data": {
        "book": {
            "isbn": "46c7e444-dbe1-4232-adb4-6035cd8649cf",
            "name": "The Cryptic Message",
            "author": {
                "id": 1,
                "name": "Freddy John"
            }
        }
    }
}
```

# TODO
[ ] - Make Middleware Generic
