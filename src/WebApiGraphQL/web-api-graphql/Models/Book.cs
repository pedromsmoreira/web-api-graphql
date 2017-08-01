namespace WebApiGraphQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Extensions;

    public class Book
    {
        public string Isbn { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Author Author { get; set; } = new Author { };

        [Required]
        public Publisher Publisher { get; set; } = new Publisher { };

        private static readonly IList<string> names = new List<string> { "Freddy", "James", "David", "John", "Peter", "Paul" };
        private static readonly IList<string> books = new List<string> { "A tale of two cities", "Peter's Hand", "The Cryptic Message", "The Messenger", "The law suit", "Music for Kids" };
        private static readonly IList<string> publishers = new List<string> { "Macmillan", "Kevins", "Holy Prints" };

        public static IEnumerable<Book> GetBooks(int count)
        {
            foreach (var id in Enumerable.Range(1, count))
            {
                int bookPos;
                int authorFirstname;
                int authorLastname;
                int publisherName;
                DateTime birthdate;
                List<Book> authorbooks;
                List<Book> publisherBooks;
                var isbn = DataSetup(out bookPos, out authorFirstname, out authorLastname, out publisherName, out birthdate, out authorbooks, out publisherBooks);

                yield return new Book
                {
                    Author = new Author()
                    {
                        Id = id,
                        Name = names.ElementAt(authorFirstname) + " " + names.ElementAt(authorLastname),
                        Birthdate = birthdate,
                        Books = authorbooks
                    },
                    Isbn = isbn,
                    Name = books.ElementAt(bookPos),
                    Publisher = new Publisher()
                    {
                        Id = id,
                        Name = publishers.ElementAt(publisherName),
                        Books = publisherBooks,
                        Authors = new List<Author>
                        {
                            new Author
                            {
                                Name = names.ElementAt(authorFirstname) + " " + names.ElementAt(authorLastname),
                                Id = id,
                                Birthdate = birthdate,
                                Books = authorbooks
                            }
                        }
                    }
                };
            }
        }

        private static string DataSetup(out int bookPos, out int authorFirstname, out int authorLastname, out int publisherName,
            out DateTime birthdate, out List<Book> authorbooks, out List<Book> publisherBooks)
        {
            var isbn = Guid.NewGuid().ToString();
            bookPos = Convert.ToInt32(Math.Floor(Number.Rnd() * books.Count));

            authorFirstname = Convert.ToInt32(Math.Floor(Number.Rnd() * names.Count));
            authorLastname = Convert.ToInt32(Math.Floor(Number.Rnd() * names.Count));

            publisherName = Convert.ToInt32(Math.Floor(Number.Rnd() * publishers.Count));

            birthdate = DateTime.Now.AddYears(Convert.ToInt32(Math.Floor(Number.Rnd() * 20)));

            authorbooks = new List<Book>
            {
                new Book
                {
                    Isbn = isbn,
                    Name = books.ElementAt(bookPos)
                }
            };

            publisherBooks = new List<Book>
            {
                new Book
                {
                    Isbn = isbn,
                    Name = books.ElementAt(bookPos)
                },
                new Book
                {
                    Isbn = isbn,
                    Name = books.ElementAt(bookPos == 0 ? bookPos + 2 : bookPos - 1)
                }
            };
            return isbn;
        }
    }
}