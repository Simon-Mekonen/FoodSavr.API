namespace FoodSavr.API.Models.Recipe
{
    public class IngredientConverterDto
    {
        public int IngredientId { get; set; }
        public string? OriginalIngredient { get; set; }
        public string? ReplacementIngredient { get; set; }
    }
}