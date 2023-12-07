using FoodSavr.API.DbContexts;
using FoodSavr.API.Models.Recipe;
using Microsoft.EntityFrameworkCore;

namespace FoodSavr.API.Services
{
    public class IngredientConverterServices : IIngredientConverterServices
    {
        private readonly FoodSavrDbContext _dbContext;
        private readonly string _uncategorized = "uncategorized";

        public IngredientConverterServices(FoodSavrDbContext context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<IngredientConverterDto>> GetRecipeIngredientConverterAsync(int recipeId, List<int> ingredients)
        {

            var ingredientsToCheck = await GetIngredientsToCheckAsync(recipeId, ingredients);

            var replacementIngredients = await GetIngredientReplacements(ingredients);


            IEnumerable<IngredientConverterDto> converter = new List<IngredientConverterDto>();

            foreach (var ingredient in ingredientsToCheck)
            {
                var replacement = replacementIngredients.FirstOrDefault(r => r.CategoryId == ingredient.CategoryId);

                if (replacement != null && ingredient.IngredientId != replacement.IngredientId)
                {
                    ingredient.ReplacementIngredient = replacement.ReplacementIngredient;
                    converter = converter.Append(ingredient).ToArray();
                }
            }

            return converter;
        }

        private async Task<IEnumerable<IngredientConverterDto>> GetIngredientsToCheckAsync(int recipeId, List<int> ingredients)
        {
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

            return ingredientsToCheck;
        }

        private async Task<IEnumerable<IngredientConverterDto>> GetIngredientReplacements(List<int> ingredients)
        {
            var secondQuery = from I in _dbContext.Ingredient
                              where ingredients.Contains(I.Id)
                              select new IngredientConverterDto()
                              {
                                  CategoryId = I.CategoryId,
                                  ReplacementIngredient = I.Name,
                                  IngredientId = I.Id
                              };

            var replacements = await secondQuery.ToListAsync();
            return replacements;
        }
    }
}
