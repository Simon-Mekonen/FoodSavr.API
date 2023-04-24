namespace FoodSavr.API.Models
{
    public class IngredientDataStore
    {
        public ICollection<IngredientDto> Ingredients { get; set; }
            = new List<IngredientDto>();

        public int MaxIngredientId { get { return Ingredients.Max(i => i.Id); } }
        public static IngredientDataStore Current { get; } = new IngredientDataStore();
        public int NumberOfIngredients { get { return Ingredients.Count(); } }

        public IngredientDataStore()
        {
            // init dummy data
            Ingredients = new List<IngredientDto>()
            {
                new IngredientDto()
                {
                    Id = 1,
                    IngredientCategoryId = 1,
                    Name = "Tomat"
                },
                new IngredientDto()
                {
                    Id = 2,
                    IngredientCategoryId = 2,
                    Name = "Fisk"
                },
                new IngredientDto()
                {
                    Id = 3,
                    IngredientCategoryId = 2,
                    Name = "Nötkött"
                },
                new IngredientDto()
                {
                    Id = 4,
                    IngredientCategoryId = 1,
                    Name = "Sallad"
                },
                new IngredientDto()
                {
                    Id = 5,
                    IngredientCategoryId = 3,
                    Name = "Persilja"
                },
                new IngredientDto()
                {
                    Id = 6,
                    IngredientCategoryId = 1,
                    Name = "Avokado"
                },
            };
        }
        public static bool IngredientExists(string ingredientName)
        {
            var firstOrDefaultIngredient = IngredientDataStore.Current.Ingredients.FirstOrDefault(
                i => i.Name.ToLower() == ingredientName.ToLower());
            
            bool ingredientExists = true;

            if (firstOrDefaultIngredient == null)
            {
                ingredientExists = false;
            }
            return ingredientExists;
        }
    }

}
