using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodSavr.API.Entities
{
    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required, MinLength(3), MaxLength(20)]
        public string? Name { get; set; } = null!;

        public Ingredient(int categoryId, string name)
        {
            CategoryId = categoryId;
            Name = name;
        }
    }
}
