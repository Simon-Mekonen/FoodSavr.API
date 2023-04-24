using FoodSavr.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

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

        [HttpGet("{id}", Name = "GetIngredient")]
        public ActionResult<IngredientDto> GetIngredient(int id)
        {
            var ingredientToReturn = IngredientDataStore.Current.Ingredients.FirstOrDefault(i => i.Id == id);
            if (ingredientToReturn == null) 
                return NotFound();
            
            return Ok (ingredientToReturn);
        }

        [HttpGet("GetIngredientCount")]
        public ActionResult<int> GetIngredientCount()
        {
            int ingredientCount = IngredientDataStore.Current.NumberOfIngredients;
            if (ingredientCount == 0)
            {
                return NotFound();
            }
            return Ok(ingredientCount);
        }

        [HttpPost("PostIngredient")]
        public ActionResult<IngredientDataStore> CreateIngredient(
            string? categoryName,
            string ingredientName,
            IngredientDto ingredient)
        {
            // Quit if ingredient exists
            bool ingredientExists = IngredientDataStore.IngredientExists(ingredientName);
            if (ingredientExists)
            {
                return NotFound("ingredient exists"); //CHANGE TO MESSAGE WITH INGREDIENT ALREADY EXISTS
            }

            // Add category if not exists
            var ingredientCategoryItem = IngredientCategoryDataStore.Current.IngredientCategories.FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
            
            if (ingredientCategoryItem == null)
            {
                ingredientCategoryItem = new IngredientCategoryDto()
                {
                    Name = categoryName,
                    Id = IngredientCategoryDataStore.Current.MaxCategoryId + 1,
                };
                IngredientCategoryDataStore.Current.IngredientCategories.Add(ingredientCategoryItem);
            }

            // Add the ingredient
            var newIngredient = new IngredientDto()
            {
                Id = IngredientDataStore.Current.MaxIngredientId + 1,
                Name = ingredientName,
                IngredientCategoryId = ingredientCategoryItem.Id
            };
            IngredientDataStore.Current.Ingredients.Add(newIngredient);
            return CreatedAtRoute("GetIngredient", 
                new
                {
                    id = newIngredient.Id,
                }, 
                newIngredient);
        }

    }
}
