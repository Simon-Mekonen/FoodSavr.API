using FoodSavr.API.Entities;

namespace FoodSavr.API.Services
{
    public interface IIngredientRepository
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync();
        Task<Ingredient> GetIngredientAsync(int id);
    }
}
