using FoodSavr.API.Models;
using Microsoft.AspNetCore.Mvc;
namespace FoodSavr.API
{
    //
    [ApiController]
    [Route("api/foodsavr")]
    public class FoodSavrController : ControllerBase
    {
        [HttpGet]
        public JsonResult GetIngredients() 
        {
            return new JsonResult(IngredientDataStore.Current.Ingredients);
        }
        [HttpGet("{id}")]
        public JsonResult GetIngredient(int id)
        {
            return new JsonResult(
                IngredientDataStore.Current.Ingredients.FirstOrDefault(i => i.Id == id));
        }
    }
}
