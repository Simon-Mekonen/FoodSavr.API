namespace FoodSavr.API.Models
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public int IngredientCategoryId { get; set; }
        public string Name { get; set; }
    }
}