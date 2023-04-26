using System.Xml.Linq;

namespace FoodSavr.API.Models
{
    public class IngredientCategoryDataStore
    {
        public List<IngredientCategoryDto> IngredientCategories { get; set; }
        public int MaxCategoryId { get { return IngredientCategories.Max(c => c.Id);  } }
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

        // Create ingredient category if not exists (refactor)
        public IngredientCategoryDto CreateNewCategory(string name)
        {
            var ingredientCategoryItem = IngredientCategories.FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if (ingredientCategoryItem == null)
            {
                ingredientCategoryItem = new IngredientCategoryDto()
                {
                    Name = name,
                    Id = MaxCategoryId + 1,
                };
            IngredientCategories.Add(ingredientCategoryItem);

            }
            return ingredientCategoryItem;
        }
}
}
