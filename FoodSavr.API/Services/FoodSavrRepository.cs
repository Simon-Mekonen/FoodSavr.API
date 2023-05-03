using FoodSavr.API.Controllers;
using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSavr.API.Services
{
    public class FoodSavrRepository : IFoodSavrRepository
    {
        private readonly FoodSavrContext _context;
        private readonly GetRecipeListController _recipeConnection;

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

        public async Task<(IEnumerable<Ingredient>, PaginationMetaData)> GetIngredientsAsync(
            string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Ingredient as IQueryable<Ingredient>;

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLower();
                collection = collection.Where(i => i.Name.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetaData(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(i => i.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<bool> IngredientExist(int id)
        {
            return await _context.Ingredient.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> IngredientExist(string name)
        {
            return await _context.Ingredient.AnyAsync(i => i.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CategoryExist(int id)
        {
            return await _context.Category.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Recipe>> GetRecipesAsync()
        {
            return await _context.Recipe.OrderBy(r => r.Id).ToListAsync();
        }

        public async Task<Recipe> GetRecipeAsync(int id)
        {
            return await _context.Recipe.Where(r => r.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Recipe>> TestAsync(List<IngredientDto> ingredients)
        {
            // implement the necessary code to fetch the recipes that has ingredients that matches in category.
            // use Stored Procedure?
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync2(List<int> ingredients)
        {
            return await _recipeConnection.Index(ingredients);
        }



        public async Task<bool> RecipeExist(int id)
        {
            return await _context.Recipe.AnyAsync(i => i.Id == id);
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient) 
        {
            var ingredientToSave = new Ingredient(ingredient.CategoryId, ingredient.Name);
            await _context.Ingredient.AddAsync(ingredientToSave);
            return ingredientToSave;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await  _context.SaveChangesAsync() >= 0);
        }

    }
}
