namespace FoodSavr.API.Models
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImgLink { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }
        public List<RecipeStepsDto>? Steps { get; set; }
        public List<RecipeIngredientDto>? Ingredients { get; set;}
    }

    public class RecipeStepsDto
    {
        public int Row { get; set; }
        public string? Text { get; set; }
    }
    public class RecipeIngredientDto
    {
        public int Quantity { get; set; }
        public string? Measurement { get; set; }
        public string? OriginalIngredient { get; set; }
        public string? Replacement { get; set; }
    }
}