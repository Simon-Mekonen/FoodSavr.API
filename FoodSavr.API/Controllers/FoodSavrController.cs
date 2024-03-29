﻿using AutoMapper;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using FoodSavr.API.Models.Recipe;
using FoodSavr.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FoodSavr.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/foodsavr")]
    public class FoodSavrController : ControllerBase
    {
        private readonly ILogger<FoodSavrController> _logger;
        private readonly IMailService _mailService;
        private readonly IFoodSavrRepository _FoodSavrRepository;
        private readonly IMapper _mapper;
        const int maxIngredientPageSize = 100;

        public FoodSavrController(
            ILogger<FoodSavrController> logger,
            IMailService mailService,
            IFoodSavrRepository FoodSavrRepository,
            IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _FoodSavrRepository = FoodSavrRepository ?? 
                throw new ArgumentNullException(nameof(FoodSavrRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(_mapper));
        }

        [Route("ingredients")]
        [HttpGet("GetIngredients")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
        {
            try
            {
                var ingredientEntities = await _FoodSavrRepository.GetIngredientsAsync();
                return Ok(_mapper.Map<IEnumerable<IngredientDto>>(ingredientEntities));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to fetch GetIngredients. Error: {ex}");
                return NotFound();
            }
        }

        //// For pagination
        //[Route("ingredients")]
        //[HttpGet("GetIngredients")]
        //public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients(
        //    string? searchQuery,
        //    int pageNumber = 1,
        //    int pageSize = 100) 
        //{
        //    try
        //    {
        //        if (pageSize > maxIngredientPageSize) 
        //        {
        //            pageSize = maxIngredientPageSize;
        //        }

        //        var (ingredientEntities, paginationMetadata) = await _FoodSavrRepository
        //            .GetIngredientsAsync(searchQuery, pageNumber, pageSize);

        //        Response.Headers.Add("X-Pagination",
        //        JsonSerializer.Serialize(paginationMetadata));

        //        return Ok(_mapper.Map<IEnumerable<IngredientDto>>(ingredientEntities));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation($"Unable to fetch GetIngredients. Error: {ex}");
        //        return NotFound();
        //    }
        //}

        [Route("ingredient/{id}")]
        [HttpGet("{id}", Name = "GetIngredient")]
        public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
        {
            try
            {
                if (!await _FoodSavrRepository.IngredientExist(id))
                {
                    _logger.LogInformation($"Ingredient with id {id} was not found");
                    return NotFound();
                }
                var ingredientEntity = await _FoodSavrRepository.GetIngredientAsync(id);
                return Ok(_mapper.Map<IngredientDto>(ingredientEntity));
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to fetch ingredient with id {id}. Error: {ex}");
                return NotFound();
            }
        }

        [Route("recipematches")]
        [HttpGet(Name = "RecipeBlob")]
        public async Task<ActionResult<IEnumerable<RecipeBlobDto>>> GetRecipeMatches([FromBody] List<int> ingredients)
        {
            try
            {
                if (ingredients.Count == 0)
                {
                    ingredients = new List<int>{1,2,3,4,5,6};
                }

                var recipes = await _FoodSavrRepository.GetRecipesAsync(ingredients);
                return Ok(recipes);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error {ex}");
                return NotFound();
            }
        }

        // Create an overload where you can give ingredients/recipeId in url or body
        [Route("recipe/{recipeId}")]
        [HttpGet("{recipeId}", Name = "GetRecipe")]
        public async Task<ActionResult<RecipeCompleteDto>> GetRecipe(int recipeId, List<int> ingredients)
        {
            try
            {
                if(!await _FoodSavrRepository.RecipeExist(recipeId))
                {
                    _logger.LogInformation($"Recipe with id {recipeId} was not found");
                    return NotFound();
                }
                var (recipe, recipeSteps, recipeIngredient, ingredientConverter) = await _FoodSavrRepository.GetCompleteRecipeAsync(recipeId, ingredients);

                var recipeComplete = new RecipeCompleteDto(
                    _mapper.Map<RecipeDto>(recipe),
                    _mapper.Map<List<RecipeStepsDto>>(recipeSteps),
                    recipeIngredient, //SIMON: CHANGE TO USING DTO/ENTITY?
                    ingredientConverter //SIMON: CHANGE TO USING DTO/ENTITY?
                    );

                return Ok(recipeComplete);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Unable to fetch recipe with id {recipeId}. Error: {ex}");
                return NotFound();
            }
        }

        //SIMON: Add verification for who can Post
        [Route("createingredient")]
        [HttpPost("PostIngredient")]
        public async Task<ActionResult<IngredientDto>> CreateIngredient(
            IngredientForCreationDto ingredient)
        {
            try
            {
                if (await _FoodSavrRepository.IngredientExist(ingredient.Name.Trim()))
                {
                    _logger.LogInformation($"Ingredient {ingredient.Name} already exists");
                    return NotFound("ingredient already exists");
                }

                if(!await _FoodSavrRepository.CategoryExist(ingredient.CategoryId))
                {
                    _logger.LogInformation($"Category with Id {ingredient.CategoryId} does not exist");
                    return NotFound($"category with Id {ingredient.CategoryId} does not exist");
                }

                var finalIngredient = 
                    await _FoodSavrRepository.CreateIngredientAsync(
                    _mapper.Map<Ingredient>(ingredient));

                await _FoodSavrRepository.SaveChangesAsync();

                var createdIngredientToReturn = _mapper.Map<IngredientDto>(finalIngredient);

                return CreatedAtRoute("GetIngredient", new { createdIngredientToReturn.Id }, createdIngredientToReturn);

            }
            catch (Exception ex) 
            { 
                _logger.LogInformation(
                    $"Unable to save ingredient {ingredient.Name}. " +
                    $"Error: {ex}"); return NotFound(); 
            }

        }

        // ADD PARTIAL UPDATE of Recipe.

        /* ADD new recipe
         * Send in a format (form in website?)
         * 1. Creates the recipe in database with the information
         * 2. Return the id for recipe
         * 3. Creates the steps table with recipe id
         * 4. Creates the table in ingredients with recipe id
         * 5. adds the measurements (and creates measurements that doesent exist)
         * 6. adds the ingredients in the recipe, if category doesent exist it will add into category db.
         * 
         * -- use a stored procedure maybe?
         */

        ////Add verification for who can Put
        //[Route("UpdateIngredient")]
        //[HttpPut("{ingredientid}", Name = "UpdateIngredient")]
        //public ActionResult UpdateIngredient(
        //    int ingredientId,
        //    IngredientForUpdateDto ingredient)
        //{
        //    //need to add check so ingredient name doesent exist already
        //    //need to check that category name doesent already exist
        //    //add these to functions on existing classes

        //    // find the ingredient
        //    var ingredientItem = _ingredientDataStore.FindIngredientItem(ingredientId);
        //    if (ingredientItem == null)
        //    {
        //        return NotFound("Ingredient id was not found");
        //    }

        //    // Creating an ingredient category if it doesent exist
        //    var ingredientCategoryItem = _ingredientCategoryDataStore.CreateNewCategory(ingredient.Category);

        //    ingredientItem.Name = ingredient.Name.ToLower() ?? ingredientItem.Name;
        //    ingredientItem.IngredientCategoryId = ingredientCategoryItem.Id;

        //    return NoContent();
        //}

        ////Add verification for who can Delete
        //[HttpDelete("{id}", Name = "DeleteIngredient")]
        //public ActionResult DeleteIngredient(int id)
        //{
        //    var ingredient = _ingredientDataStore.FindIngredientItem(id);
        //    if (ingredient == null)
        //    {
        //        return NotFound();
        //    }

        //    _ingredientDataStore.Ingredients.Remove(ingredient);

        //    _mailService.Send(
        //        "Ingredient deleted.",
        //        $"Ingredient {ingredient.Name} with id {ingredient.Id} was deleted.");

        //    return NoContent();
        //}
    }
}
