using System.ComponentModel.DataAnnotations;

namespace TechRegression.Models
{
    public class ArticleViewModel
    {
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Treść artykułu jest wymagana")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Wybierz kategorię")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Wgraj zdjęcie wyróżniające")]
        public IFormFile ImageFile { get; set; }
    }
}
