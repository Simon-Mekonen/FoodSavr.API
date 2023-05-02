using FoodSavr.API.Entities;
using FoodSavr.API.Models;

namespace FoodSavr.API.Services
{
    public interface IFoodSavrRepository
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<(IEnumerable<Ingredient>, PaginationMetaData)> GetIngredientsAsync(
            string? searchQuery, int pageNumber, int pageSize);
        Task<Ingredient> GetIngredientAsync(int id);
        Task<IEnumerable<Recipe>> GetRecipesAsync();
        Task<Recipe> GetRecipeAsync(int id);
        Task<bool> IngredientExist(int id);
        Task<bool> IngredientExist(string name);
        Task<bool> RecipeExist(int id);
        Task<bool> CategoryExist(int id);
        Task <Ingredient>CreateIngredientAsync(Ingredient ingredient);
        Task<bool> SaveChangesAsync();
    }
}
