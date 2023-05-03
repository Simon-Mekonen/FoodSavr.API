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
        Task<IEnumerable<RecipeBlobDto>> GetRecipesAsync(List<int> ingredientId);
        Task<Ingredient>CreateIngredientAsync(Ingredient ingredient);
        Task<bool> SaveChangesAsync();

        Task<IEnumerable<RecipeSteps>> GetRecipeStepsAsync(int id);

        // can be removed later
        Task<Recipe> GetRecipeAsync(int id);
        Task<bool> IngredientExist(int id);
        Task<bool> IngredientExist(string name);
        Task<bool> RecipeExist(int id);
        Task<bool> CategoryExist(int id);
    }
}
