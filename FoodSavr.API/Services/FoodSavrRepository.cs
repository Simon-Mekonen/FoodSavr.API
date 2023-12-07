using FoodSavr.API.Controllers;
using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using FoodSavr.API.Models.Recipe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace FoodSavr.API.Services
{
    public class FoodSavrRepository : IFoodSavrRepository
    {
        private readonly FoodSavrDbContext _dbContext;
        private readonly GetRecipeListController _recipeConnection;
        private readonly IIngredientConverterServices _IngredientConverterServices;

        public FoodSavrRepository(
            FoodSavrDbContext context,
            IIngredientConverterServices IngredientConverterServices) 
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
            _IngredientConverterServices = IngredientConverterServices ?? throw new ArgumentNullException(nameof(IngredientConverterServices));

        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

        // Ingredient
        public async Task<Ingredient> GetIngredientAsync(int id)
        {
            return await _dbContext.Ingredient.Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync()
        {
            return await _dbContext.Ingredient.OrderBy(i => i.Name).ToListAsync();
        }

        public async Task<(IEnumerable<Ingredient>, PaginationMetaData)> GetIngredientsAsync(
            string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _dbContext.Ingredient as IQueryable<Ingredient>;

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
            return await _dbContext.Ingredient.AnyAsync(i => i.Id == id);
        }

        public async Task<bool> IngredientExist(string name)
        {
            return await _dbContext.Ingredient.AnyAsync(i => i.Name.ToLower() == name.ToLower());
        }

        // Category
        public async Task<bool> CategoryExist(int id)
        {
            return await _dbContext.Category.AnyAsync(c => c.Id == id);
        }

        // Recipe
        public async Task<IEnumerable<RecipeBlobDto>> GetRecipesAsync(List<int> ingredientId)
        {
            var controller = new GetRecipeListController(_dbContext); //SIMON: remake to LINQ
            var result = controller.Index(ingredientId) as ViewResult;

            var recipeList = result.Model as List<RecipeBlobDto>;
            return recipeList.AsEnumerable();
        }

        public async Task<(
            Recipe,
            IEnumerable<RecipeSteps>,
            IEnumerable<RecipeIngredientDto>,
            IEnumerable<IngredientConverterDto>)>
            GetCompleteRecipeAsync(int recipeId, List<int> ingredients)
        {
            var recipe = await _dbContext.Recipe.Where(r => r.Id == recipeId).FirstOrDefaultAsync();
            var recipeSteps = await GetRecipeStepsAsync(recipeId);
            var recipeIngredient = await GetRecipeIngredientAsync(recipeId);
            var ingredientConverter = await _IngredientConverterServices.GetRecipeIngredientConverterAsync(recipeId, ingredients);
            
            return (recipe, recipeSteps, recipeIngredient, ingredientConverter);
        }

        private async Task<IEnumerable<RecipeIngredientDto>> GetRecipeIngredientAsync(int recipeId)
        {
            var query = from RI in _dbContext.RecipeIngredient
                        join M in _dbContext.Measurement on RI.MeasurementId equals M.Id into measurementGroup
                        from measurement in measurementGroup.DefaultIfEmpty()
                        join I in _dbContext.Ingredient on RI.IngredientId equals I.Id
                        where RI.RecipeId == recipeId
                        orderby RI.Id
                        select new RecipeIngredientDto()
                        {
                            IngredientId = RI.IngredientId,
                            Quantity = (int)RI.Quantity,
                            Measurement = measurement.Measure,
                            OriginalIngredient = I.Name
                        };

            var result = await query.ToListAsync();
            return result;
        }

        private async Task<IEnumerable<RecipeSteps>> GetRecipeStepsAsync(int id)
        {
            return await _dbContext.RecipeSteps.Where(rs => rs.RecipeId == id).ToListAsync();
        }


        public async Task<bool> RecipeExist(int id)
        {
            return await _dbContext.Recipe.AnyAsync(i => i.Id == id);
        }

        public async Task<Ingredient> CreateIngredientAsync(Ingredient ingredient) 
        {
            var ingredientToSave = new Ingredient(ingredient.CategoryId, ingredient.Name);
            await _dbContext.Ingredient.AddAsync(ingredientToSave);
            return ingredientToSave;
        }

    }
}
