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

        private static string[] names = new string[] { "Freddy", "James", "David", "John", "Peter", "Paul" };
        private static string[] books = new string[] { "A tale of two cities", "Peter's Hand", "The Cryptic Message", "The Messenger", "The law suit", "Music for Kids" };
        private static string[] publishers = new string[] { "Macmillan", "Kevins", "Holy Prints" };


        public static IEnumerable<Book> GetBooks(int count)
        {
            foreach (int id in Enumerable.Range(1, count))
            {
                yield return new Book
                {
                    Author = new Author()
                    {
                        Id = id,
                        Name = names[Convert.ToInt32(Math.Floor(Number.Rnd() * names.Length))] + " " + names[Convert.ToInt32(Math.Floor(Number.Rnd() * names.Length))],
                        Birthdate = DateTime.Now.AddYears(Convert.ToInt32(Math.Floor(Number.Rnd() * 20)))
                    },
                    Isbn = Guid.NewGuid().ToString(),
                    Name = books[Convert.ToInt32(Math.Floor(Number.Rnd() * books.Length))],
                    Publisher = new Publisher()
                    {
                        //Id = id,
                        Name = publishers[Convert.ToInt32(Math.Floor(Number.Rnd() * publishers.Length))]
                    }
                };
            }
        }
    }
}