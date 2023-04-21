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

    }
}
