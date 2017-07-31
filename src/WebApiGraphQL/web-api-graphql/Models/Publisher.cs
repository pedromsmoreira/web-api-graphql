namespace WebApiGraphQL.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Publisher
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();

        public ICollection<Author> Authors { get; set; } = new List<Author>();
    }
}