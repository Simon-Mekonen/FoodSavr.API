using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodSavr.API.Services
{
    public class FoodSavrRepository : IFoodSavrRepository
    {
        private readonly FoodSavrContext _context;

        public FoodSavrRepository(FoodSavrContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<Ingredient> GetIngredientAsync(int id)
        {
            return await _context.Ingredient.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
        {
            return await _context.Ingredient.OrderBy(i => i.Name).ToListAsync();
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await _context.Recipe.OrderBy(r => r.Id).ToListAsync();
        }
    }
}
