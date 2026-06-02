using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechRegression.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Autor")]
        public string Author { get; set; }

        [Required]
        [Display(Name = "Treść komentarza")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public int ArticleId { get; set; }

        [ForeignKey("ArticleId")]
        public Article? Article { get; set; }
    }
}
