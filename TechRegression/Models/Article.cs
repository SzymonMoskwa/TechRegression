using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechRegression.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tytuł")]
        [StringLength(150, ErrorMessage = "Tytuł artykułu nie może przekraczać 150 znaków.")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Treść artykułu")]
        public string Content { get; set; }

        [Display(Name = "Zdjęcie")]
        public string? ImagePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public List<Comment> Comments { get; set; } = [];
    }
}
