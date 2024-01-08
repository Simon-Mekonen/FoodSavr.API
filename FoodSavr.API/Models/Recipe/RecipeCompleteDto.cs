using AutoMapper;
using FoodSavr.API.Models.Recipe;

namespace FoodSavr.API.Models
{
    public class RecipeCompleteDto
    {
        public RecipeDto Recipe { get; set; }
        public List<RecipeStepsDto> RecipeSteps { get; set; }
        public IEnumerable<RecipeIngredientDto> RecipeIngredient { get; set; }
        public IEnumerable<IngredientConverterDto> IngredientConverter { get; set; }

        public RecipeCompleteDto(RecipeDto recipe, List<RecipeStepsDto> recipeSteps, IEnumerable<RecipeIngredientDto> recipeIngredient, IEnumerable<IngredientConverterDto> ingredientConverter)
        {
            this.Recipe = recipe;
            this.RecipeSteps = recipeSteps;
            this.RecipeIngredient = recipeIngredient;
            this.IngredientConverter = ingredientConverter;
        }        
    }
}
