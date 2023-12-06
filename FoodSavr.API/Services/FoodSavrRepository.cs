using FoodSavr.API.Controllers;
using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using FoodSavr.API.Models.Ingredient;
using FoodSavr.API.Models.Recipe;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
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

        public async Task<IEnumerable<RecipeBlobDto>> GetRecipesAsync(List<int> ingredientId)
        {
            var controller = new GetRecipeListController(_context);
            var result = controller.Index(ingredientId) as ViewResult;

            var recipeList = result.Model as List<RecipeBlobDto>;
            return recipeList.AsEnumerable();
        }
        public async Task<(
            RecipeBlobDto, 
            IEnumerable<RecipeStepsDto>, 
            IEnumerable<RecipeIngredientDto>, 
            IEnumerable<IngredientConverterDto>)> 
            GetRecipeAsync(int recipeId, List<int> ingredients)
        {
            var recipeSteps = await GetRecipeStepsAsync(recipeId);
            var recipeIngredient = new NotImplementedException(); // SQL query is needed
            var ingredientConverter = new NotImplementedException(); // SQL query is needed

            return await _context.Recipe.Where(r => r.Id == recipeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RecipeSteps>> GetRecipeStepsAsync(int id)
        {
            return await _context.RecipeSteps.Where(rs => rs.RecipeId == id).ToListAsync();
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

    }
}
