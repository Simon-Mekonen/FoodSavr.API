using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using FoodSavr.API.Models.Recipe;

namespace FoodSavr.API.Services
{
    public interface IFoodSavrRepository
    {
        Task<bool> SaveChangesAsync();

        // Ingredient
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<(IEnumerable<Ingredient>, PaginationMetaData)> GetIngredientsAsync(
            string? searchQuery, int pageNumber, int pageSize);
        Task<bool> IngredientExist(int id);
        Task<bool> IngredientExist(string name);
        Task<Ingredient> GetIngredientAsync(int id);
        Task<Ingredient>CreateIngredientAsync(Ingredient ingredient);
        
        // Category
        Task<bool> CategoryExist(int id);

        // Recipe
        Task<bool> RecipeExist(int id);
        Task<IEnumerable<RecipeBlobDto>> GetRecipesAsync(List<int> ingredientId);
        Task<(
            Recipe, 
            IEnumerable<RecipeSteps>, 
            IEnumerable<RecipeIngredient>, 
            IEnumerable<IngredientConverterDto>)> 
            GetRecipeAsync(int recipeId, List<int> ingredients);
        Task<IEnumerable<RecipeSteps>> GetRecipeStepsAsync(int id);
    }
}
