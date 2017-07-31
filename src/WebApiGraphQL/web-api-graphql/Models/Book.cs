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

        private static IList<string> names = new List<string> { "Freddy", "James", "David", "John", "Peter", "Paul" };
        private static IList<string> books = new List<string> { "A tale of two cities", "Peter's Hand", "The Cryptic Message", "The Messenger", "The law suit", "Music for Kids" };
        private static string[] publishers = new string[] { "Macmillan", "Kevins", "Holy Prints" };

        public static IEnumerable<Book> GetBooks(int count)
        {
            foreach (int id in Enumerable.Range(1, count))
            {
                var isbn = Guid.NewGuid().ToString();
                var bookPos = Convert.ToInt32(Math.Floor(Number.Rnd() * books.Count));

                var authorFirstname = Convert.ToInt32(Math.Floor(Number.Rnd() * names.Count));
                var authorLastname = Convert.ToInt32(Math.Floor(Number.Rnd() * names.Count));

                var publisherName = Convert.ToInt32(Math.Floor(Number.Rnd() * publishers.Length));

                yield return new Book
                {
                    Author = new Author()
                    {
                        Id = id,
                        Name = names[authorFirstname] + " " + names[authorLastname],
                        Birthdate = DateTime.Now.AddYears(Convert.ToInt32(Math.Floor(Number.Rnd() * 20))),
                        Books = new List<Book> { new Book
                            {
                                Isbn = isbn,
                                Name = books.ElementAt(bookPos)
                            }
                        }
                    },
                    Isbn = isbn,
                    Name = books.ElementAt(bookPos),
                    Publisher = new Publisher()
                    {
                        Id = id,
                        Name = publishers[publisherName],
                        Books = new List<Book> { new Book
                            {
                                Isbn = isbn,
                                Name = books.ElementAt(bookPos)
                            },
                            new Book
                                {
                                    Isbn = isbn,
                                    Name = books.ElementAt(bookPos + 1)
                                }
                            },
                        Authors = new List<Author>
                        {
                            new Author
                            {
                                Name = names[authorFirstname] + " " + names[authorLastname],
                                Id = id
                            }
                        }
                    }
                };
            }
        }
    }
}