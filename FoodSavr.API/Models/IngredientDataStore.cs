namespace FoodSavr.API.Models
{
    public class IngredientDataStore
    {
        public List<IngredientDto> Ingredients { get; set; }
        public static IngredientDataStore Current { get; } = new IngredientDataStore();
        public IngredientDataStore() 
        {
            // init dummy data
            Ingredients = new List<IngredientDto>()
            {
                new IngredientDto()
                {
                    Id = 1,
                    Name = "Tomat"
                },
                new IngredientDto()
                {
                    Id = 2,
                    Name = "Fisk"
                },
                new IngredientDto()
                {
                    Id = 3,
                    Name = "Nötkött"
                },
                new IngredientDto()
                {
                    Id = 4,
                    Name = "Sallad"
                },
                new IngredientDto()
                {
                    Id = 5,
                    Name = "Persilja"
                },
                new IngredientDto()
                {
                    Id = 6,
                    Name = "Avokado"
                },
            };
        }
    }

}
