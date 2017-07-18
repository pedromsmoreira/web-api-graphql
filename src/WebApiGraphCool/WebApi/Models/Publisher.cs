namespace WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public Book[] Books { get; set; } = new Book[] { };

        public Author[] Authors { get; set; } = new Author[] { };
    }
}