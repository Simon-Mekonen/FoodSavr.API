using FoodSavr.API.Models.Recipe;

namespace FoodSavr.API.Services
{
    public interface IIngredientConverterServices
    {
        Task<IEnumerable<IngredientConverterDto>> GetRecipeIngredientConverterAsync(int recipeId, List<int> ingredients);
    }
}
