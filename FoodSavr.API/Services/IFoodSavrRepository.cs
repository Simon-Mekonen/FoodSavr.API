using FoodSavr.API.Entities;

namespace FoodSavr.API.Services
{
    public interface IFoodSavrRepository
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<Ingredient> GetIngredientAsync(int id);
        Task<IEnumerable<Recipe>> GetRecipesAsync();
    }
}
