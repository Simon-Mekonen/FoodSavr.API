namespace FoodSavr.API.Models
{
    public class IngredientForCreationDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int IngredientCategoryId { get; set; }
    }
}
