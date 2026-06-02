using System.ComponentModel.DataAnnotations;

namespace TechRegression.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nazwa kategorii")]
        public string Name { get; set; }

        public List<Article> Articles { get; set; } = [];
    }
}
