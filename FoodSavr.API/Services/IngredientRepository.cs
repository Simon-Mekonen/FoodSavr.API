using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodSavr.API.Services
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly FoodSavrContext _context;

        public IngredientRepository(FoodSavrContext context) 
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
    }
}
