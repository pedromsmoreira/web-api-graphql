namespace WebApi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public Book[] Books { get; set; } = new Book[] { };
    }
}