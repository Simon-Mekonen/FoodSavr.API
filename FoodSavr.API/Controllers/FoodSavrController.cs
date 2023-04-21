using FoodSavr.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace FoodSavr.API
{
    [ApiController]
    [Route("api/foodsavr")]
    public class FoodSavrController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<IngredientDataStore>> GetIngredients() 
        {
            var ingredientsToReturn = IngredientDataStore.Current.Ingredients;

            if (ingredientsToReturn == null)
                return NotFound();

            return Ok (ingredientsToReturn);
        }

        [HttpGet("{id}")]
        public ActionResult<IngredientDto> GetIngredient(int id)
        {
            var ingredientToReturn = IngredientDataStore.Current.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredientToReturn == null) 
                return NotFound();
            
            return Ok (ingredientToReturn);
        }

        [HttpGet("ingredientcount")]
        public ActionResult<int> GetIngredientCount()
        {
            int ingredientCount = IngredientDataStore.Current.NumberOfIngredients;
            if (ingredientCount == 0)
            {
                return NotFound();
            }
            return Ok(ingredientCount);
        }

        [HttpPost("ingredientCreate")]
        public ActionResult<IngredientDataStore> CreateIngredient(
            int categoryId,
            string ingredientName,
            IngredientForCreationDto ingredient)
        {
            var ingredientCategory = IngredientCategoryDataStore.Current.IngredientCategories.FirstOrDefault(c => c.Id == categoryId);

            var ingredientExists = IngredientDataStore.Current.Ingredients.FirstOrDefault(
                i => i.Name.ToLower() ==  ingredientName.ToLower());


            if (ingredientExists != null)
            {
                return NotFound(); //CHANGE TO MESSAGE WITH INGREDIENT ALREADY EXISTS
            }

            return Ok(ingredientExists);
        }

    }
}
