﻿using FoodSavr.API.Models;
using FoodSavr.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace FoodSavr.API.Controllers
{
    [Route("api/foodsavr")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly ILogger<IngredientController> _logger;
        private readonly IMailService _mailService;
        private readonly IngredientDataStore _ingredientDataStore;
        private readonly IngredientCategoryDataStore _ingredientCategoryDataStore;

        public IngredientController(
            ILogger<IngredientController> logger,
            IMailService mailService,
            IngredientDataStore ingredientDataStore,
            IngredientCategoryDataStore ingredientCategoryDataStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            _ingredientDataStore = ingredientDataStore ?? throw new ArgumentNullException(nameof(ingredientDataStore));
            _ingredientCategoryDataStore = ingredientCategoryDataStore ?? throw new ArgumentNullException(nameof(ingredientCategoryDataStore));
        }

        [HttpGet(Name = "GetIngredients")]
        public ActionResult<IEnumerable<IngredientDataStore>> GetIngredients() 
        {
            var ingredientsToReturn = _ingredientDataStore.Ingredients;

            if (ingredientsToReturn == null)
            {
                return NotFound("could not find ingredients");
            }

            return Ok(ingredientsToReturn);
        }

        [HttpGet("{id}")]
        public ActionResult<IngredientDto> GetIngredient(int id)
        {
            try
            {
                //throw new Exception("Exception sample.");
                var ingredientToReturn = _ingredientDataStore.Ingredients.FirstOrDefault(i => i.Id == id);
                if (ingredientToReturn == null)
                {
                    _logger.LogInformation($"Ingredient with id {id} wasn't found");
                    return NotFound();
                }
                return Ok (ingredientToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting ingredient id {id}.",
                    ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("GetIngredientCount")]
        public ActionResult<int> GetIngredientCount()
        {
            int ingredientCount = _ingredientDataStore.NumberOfIngredients;
            if (ingredientCount == 0)
            {
                return NotFound();
            }
            return Ok(ingredientCount);
        }

        //Add verification for who can Post
        [HttpPost(Name = "CreateIngredient")]
        public ActionResult<IngredientDataStore> CreateIngredient(
            IngredientForCreationDto ingredient)
        {

            // Quit if ingredient exists
            var ingredientItem = _ingredientDataStore.FindIngredientItem(ingredient.Name);
            if (ingredientItem != null)
            {
                return BadRequest("ingredient exists"); //CHANGE TO MESSAGE WITH INGREDIENT ALREADY EXISTS
            }

            // Add category if not exists
            var ingredientCategoryItem = _ingredientCategoryDataStore.CreateNewCategory(ingredient.Category);

            // Add the ingredient
            var newIngredient = _ingredientDataStore.CreateNewIngredient(ingredient.Name, ingredientCategoryItem.Id);
            return CreatedAtRoute("GetIngredient", new { Id = newIngredient.Id}, newIngredient);
        }

        //Add verification for who can Put
        [HttpPut("{ingredientid}", Name = "UpdateIngredient")]
        public ActionResult UpdateIngredient(
            int ingredientId, 
            IngredientForUpdateDto ingredient)
        {
            //need to add check so ingredient name doesent exist already
            //need to check that category name doesent already exist
            //add these to functions on existing classes

            // find the ingredient
            var ingredientItem = _ingredientDataStore.FindIngredientItem(ingredientId);
            if (ingredientItem == null) 
            {
                return NotFound("Ingredient id was not found");
            }

            // Creating an ingredient category if it doesent exist
            var ingredientCategoryItem = _ingredientCategoryDataStore.CreateNewCategory(ingredient.Category);

            ingredientItem.Name = ingredient.Name.ToLower() ?? ingredientItem.Name;
            ingredientItem.IngredientCategoryId = ingredientCategoryItem.Id;

            return NoContent();
        }

        //Add verification for who can Delete
        [HttpDelete("{id}", Name = "DeleteIngredient")]
        public ActionResult DeleteIngredient(int id)
        {
            var ingredient = _ingredientDataStore.FindIngredientItem(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            
            _ingredientDataStore.Ingredients.Remove(ingredient);
            
            _mailService.Send(
                "Ingredient deleted.",
                $"Ingredient {ingredient.Name} with id {ingredient.Id} was deleted.");

            return NoContent();
        }
    }
}