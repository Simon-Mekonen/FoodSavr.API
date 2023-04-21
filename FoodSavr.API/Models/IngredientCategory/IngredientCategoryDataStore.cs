namespace FoodSavr.API.Models
{
    public class IngredientCategoryDataStore
    {
        public ICollection<IngredientCategoryDto> IngredientCategories { get; set; }
            = new List<IngredientCategoryDto>();
        public static IngredientCategoryDataStore Current { get; } = new IngredientCategoryDataStore();
        public IngredientCategoryDataStore()
        {
            IngredientCategories = new List<IngredientCategoryDto>()
            {
                new IngredientCategoryDto()
                {
                    Id = 1,
                    Name = "Grönsak",
                },
                new IngredientCategoryDto()
                {
                    Id = 2,
                    Name = "Protein",
                },
                new IngredientCategoryDto()
                {
                    Id = 3,
                    Name = "Test",
                }
            };
        }
    }
}
