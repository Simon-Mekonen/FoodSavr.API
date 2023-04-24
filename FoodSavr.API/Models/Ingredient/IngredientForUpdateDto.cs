using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Models
{
    public class IngredientForUpdateDto
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

}
