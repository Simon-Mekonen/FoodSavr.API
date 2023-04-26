namespace FoodSavr.API.Models
{
    public class IngredientDataStore
    {
        public List<IngredientDto> Ingredients { get; set; }

        public int MaxIngredientId { get { return Ingredients.Max(i => i.Id); } }
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
                }
            };
        }
        public IngredientDto FindIngredientItem(string name)
        {
            var firstOrDefaultIngredient = Ingredients.FirstOrDefault(i => i.Name.ToLower() == name.ToLower());

            return firstOrDefaultIngredient;
        }
        public IngredientDto FindIngredientItem(int id)
        {
            var firstOrDefaultIngredient = Ingredients.FirstOrDefault(i => i.Id == id);

            return firstOrDefaultIngredient;
        }
        public IngredientDto CreateNewIngredient(string name, int categoryId)
        {
            var newIngredient = new IngredientDto()
            {
                Id = MaxIngredientId + 1,
                Name = name.ToLower().Trim(),
                IngredientCategoryId = categoryId
            };
            Ingredients.Add(newIngredient);

            return newIngredient;

        }
    }

}
