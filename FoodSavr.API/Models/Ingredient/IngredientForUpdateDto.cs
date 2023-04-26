using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Models
{
    public class IngredientForUpdateDto
    {
        public string? Name { get; set; } = string.Empty;
        public string? Category { get; set; } = string.Empty;
    }

}
