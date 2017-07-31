namespace WebApiGraphQL.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}