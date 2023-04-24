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

        [HttpPost("CreateIngredient")]
        public ActionResult<IngredientDataStore> CreateIngredient(
            IngredientForCreationDto ingredient)
        {

            // Quit if ingredient exists
            bool ingredientExists = IngredientDataStore.IngredientExists(ingredient.Name);
            if (ingredientExists)
            {
                return BadRequest("ingredient exists"); //CHANGE TO MESSAGE WITH INGREDIENT ALREADY EXISTS
            }

            // Add category if not exists
            var ingredientCategoryItem = IngredientCategoryDataStore.Current.IngredientCategories.FirstOrDefault(c => c.Name.ToLower() == ingredient.Category.ToLower());
            if (ingredientCategoryItem == null)
            {
                ingredientCategoryItem = new IngredientCategoryDto()
                {
                    Name = ingredient.Category,
                    Id = IngredientCategoryDataStore.Current.MaxCategoryId + 1,
                };
                IngredientCategoryDataStore.Current.IngredientCategories.Add(ingredientCategoryItem);
            }

            // Add the ingredient
            var newIngredient = new IngredientDto()
            {
                Id = IngredientDataStore.Current.MaxIngredientId + 1,
                Name = ingredient.Name.ToLower().Trim(),
                IngredientCategoryId = ingredientCategoryItem.Id
            };
            IngredientDataStore.Current.Ingredients.Add(newIngredient);

            return CreatedAtRoute("GetIngredient", new { Id = newIngredient.Id}, newIngredient);
        }

        [HttpPut]
        //need to add check so ingredient name doesent exist already
        //need to check that category name doesent already exist
        //add these to functions on existing classes
        public ActionResult UpdateIngredient(IngredientForUpdateDto ingredient)
        {
            // find the ingredient
            var ingredientItem = IngredientDataStore.Current.Ingredients.FirstOrDefault(i => i.Id == ingredient.Id);
            if (ingredientItem == null) 
            {
                return NotFound("Ingredient id was not found");
            }

            var ingredientCategoryItem = IngredientCategoryDataStore.Current.IngredientCategories.FirstOrDefault(c => c.Name.ToLower() == ingredient.Category.ToLower());

            if (ingredientCategoryItem == null)
            {
                ingredientCategoryItem = new IngredientCategoryDto()
                {
                    Name = ingredient.Category,
                    Id = IngredientCategoryDataStore.Current.MaxCategoryId + 1,
                };
                IngredientCategoryDataStore.Current.IngredientCategories.Add(ingredientCategoryItem);
            }

            ingredientItem.Name = ingredient.Name ?? ingredientItem.Name;
            ingredientItem.IngredientCategoryId = ingredientCategoryItem.Id;

            return CreatedAtRoute("GetIngredient", new { Id = ingredient.Id }, ingredientItem);
        }
    }
}
