using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
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

        public async Task<bool> IngredientExist(int id)
        {
            return await _context.Ingredient.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> IngredientExist(string name)
        {
            return await _context.Ingredient.AnyAsync(i => i.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await _context.Recipe.OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            return await _context.Recipe.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> RecipeExist(int id)
        {
            return await _context.Recipe.AnyAsync(i => i.Id == id);
        }

        public async Task AddIngredientAsync(IngredientForCreationDto ingredient) 
        {
            var ingredientToSave = new Ingredient(ingredient.Category, ingredient.Name);
            await _context.Ingredient.AddAsync(ingredientToSave);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await  _context.SaveChangesAsync() >= 0);
        }

    }
}
