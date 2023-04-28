namespace FoodSavr.API.Models
{
    public class RecipeIngredientDto
    {
        public int Quantity { get; set; }
        public string? Measurement { get; set; }
        public string? OriginalIngredient { get; set; }
        public string? Replacement { get; set; }
    }
}