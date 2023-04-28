using AutoMapper;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using FoodSavr.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodSavr.API.Controllers
{
    [Route("api/foodsavr")]
    [ApiController]
    public class FoodSavrController : ControllerBase
    {
        private readonly ILogger<FoodSavrController> _logger;
        private readonly IMailService _mailService;
        private readonly IFoodSavrRepository _FoodSavrRepository;
        private readonly IMapper _mapper;

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

        [Route("ingredient")]
        [HttpGet(Name = "GetIngredients")]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients() 
        {
            var ingredientEntities = await _FoodSavrRepository.GetIngredientsAsync();
            return Ok(_mapper.Map<IEnumerable<IngredientDto>>(ingredientEntities));
        }

        [Route("ingredient/{id}")]
        [HttpGet("{id}", Name = "GetIngredient")]
        public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
        {
            if (!await _FoodSavrRepository.IngredientExist(id))
            {
                _logger.LogInformation($"Ingredient with id {id} was not found");
                return NotFound();
            }
            var ingredientEntity = await _FoodSavrRepository.GetIngredientAsync(id);
            return Ok(_mapper.Map<IngredientDto>(ingredientEntity));
        }

        [Route("recipe")]
        [HttpGet("GetRecipes")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var recipes = await _FoodSavrRepository.GetRecipesAsync();
            return Ok(recipes);
        }
        [Route("recipe/{id}")]
        [HttpGet("{id}", Name = "GetRecipe")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            if(!await _FoodSavrRepository.RecipeExist(id))
            {
                _logger.LogInformation($"Recipe with id {id} was not found");
                return NotFound();
            }
            var recipe = await _FoodSavrRepository.GetRecipeAsync(id);
            return Ok(_mapper.Map<RecipeDto>(recipe));
        }

        //Add verification for who can Post
        [Route("CreateIngredient")]
        [HttpPost("PostIngredient")]
        public async Task<ActionResult<Ingredient>> CreateIngredient(
            IngredientForCreationDto ingredient)
        {
            if (await _FoodSavrRepository.IngredientExist(ingredient.Name.Trim()))
            {
                _logger.LogInformation($"Ingredient {ingredient.Name} already exists");
                return NotFound("ingredient already exists");
            }

            // Find category and save it, if it doesent exist; create it

            // Add category if not exists

            // Add the ingredient
            var finalIngredient = _mapper.Map<Ingredient>(ingredient);

            await _FoodSavrRepository.CreateIngredientAsync(finalIngredient);

            await _FoodSavrRepository.SaveChangesAsync();

            var createdIngredientToReturn = _mapper.Map<IngredientDto>(finalIngredient);

            return CreatedAtRoute("GetIngredient", 
                new 
                { 
                    createdIngredientToReturn.Id 
                }, createdIngredientToReturn);
        }

        ////Add verification for who can Put
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
