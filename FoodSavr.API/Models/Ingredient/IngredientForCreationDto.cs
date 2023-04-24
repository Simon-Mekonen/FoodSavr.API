using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Models
{
    public class IngredientForCreationDto
    {
        [Required(ErrorMessage = "You should provide a ingredient name value.")]
        [MaxLength(20)]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "You should provide a ingredient category name value.")]
        [MaxLength(20)]
        [MinLength(3)]
        public string Category { get; set;  } = string.Empty;
    }
}
