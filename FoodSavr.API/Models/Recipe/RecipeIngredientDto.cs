namespace FoodSavr.API.Models
{
    public class RecipeIngredientDto
    {
        public int IngredientId { get; set; }
        public int Quantity { get; set; }
        public string? Measurement { get; set; }
        public string? OriginalIngredient { get; set; }
    }
}