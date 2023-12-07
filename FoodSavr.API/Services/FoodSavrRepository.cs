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
        private readonly string _uncategorized = "uncategorized";

        public FoodSavrRepository(FoodSavrDbContext context) 
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _dbContext.SaveChangesAsync() >= 0);
        }

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

        public async Task<bool> CategoryExist(int id)
        {
            return await _dbContext.Category.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<RecipeBlobDto>> GetRecipesAsync(List<int> ingredientId)
        {
            var controller = new GetRecipeListController(_dbContext);
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
            var ingredientConverter = await GetRecipeIngredientConverterAsync(recipeId, ingredients);

            return (recipe, recipeSteps, recipeIngredient, ingredientConverter);
        }

        private async Task<IEnumerable<IngredientConverterDto>> GetRecipeIngredientConverterAsync(int recipeId, List<int> ingredients)
        {

            // First Step
            var nestedQuery = from I in _dbContext.Ingredient
                              join C in _dbContext.Category on I.CategoryId equals C.Id
                              where ingredients.Contains(I.Id)
                              && C.Name != _uncategorized
                              select I.CategoryId;

            var categoryIdQuery = await nestedQuery.ToListAsync();

            var query = from RI in _dbContext.RecipeIngredient
                        join I in _dbContext.Ingredient on RI.IngredientId equals I.Id
                        where categoryIdQuery.Contains(I.CategoryId)
                        && RI.RecipeId == recipeId
                        select new IngredientConverterDto()
                        {
                            CategoryId = I.CategoryId,
                            IngredientId = RI.IngredientId,
                            OriginalIngredient = I.Name,
                            ReplacementIngredient = string.Empty
                        };

            var ingredientsToCheck = await query.ToListAsync();

            // Second Step
            var secondQuery = from I in _dbContext.Ingredient
                              where ingredients.Contains(I.Id)
                              select new
                              {
                                  CategoryId = I.CategoryId,
                                  ReplacementIngredient = I.Name,
                                  IngredientId = I.Id
                              };

            var replacements = await secondQuery.ToListAsync();
            // Third Step
            // Match data from Converter and update ReplacementIngredient

            IEnumerable<IngredientConverterDto> converter = new List<IngredientConverterDto>();

            foreach (var ingredient in ingredientsToCheck)
            {
                var replacement = replacements.FirstOrDefault(r => r.CategoryId == ingredient.CategoryId);

                if (replacement != null && ingredient.IngredientId != replacement.IngredientId)
                {
                    ingredient.ReplacementIngredient = replacement.ReplacementIngredient;
                    converter = converter.Append(ingredient).ToArray();
                }
            }

            return converter;
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

        public async Task<IEnumerable<RecipeSteps>> GetRecipeStepsAsync(int id)
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
