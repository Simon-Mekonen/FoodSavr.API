using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FoodSavr.API.Controllers
{
    public class GetRecipeListController : Controller
    {
        private readonly FoodSavrContext _foodSavrContext;

        public GetRecipeListController(FoodSavrContext foodSavrContext)
        {
            _foodSavrContext = foodSavrContext;
        }

        public IActionResult Index(List<int> ingredients)
        {
            var connection = _foodSavrContext.Database.GetDbConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Ingredient;";

            //command.CommandText = 
            //    "SELECT * FROM Ingredient" +
            //    $"WHERE id IN ({ingredients})";

            using (var reader = command.ExecuteReader())
            {
                var results = new List<IngredientDto>();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var categoryId = reader.GetInt32(1);
                    var name = reader.GetString(2);

                    var model = new IngredientDto { Id = id, Name = name , CategoryId = categoryId};
                    results.Add(model);
                }

            return View(results);

            }
        }
    }
}

/* SELECT R.id, count(R.id) AS matches, R.name, R.description, R.imglink, R.portions, R.cookingtime AS time
  FROM recipeingredient AS RIL
  JOIN recipe AS R on RIL.recipeid = R.id
  JOIN ingredient AS I on I.id = RIL.ingredientid
  
  WHERE I.categoryid IN (
  	SELECT I2.categoryid
  		FROM ingredient AS I2
  		JOIN category AS C ON C.id = I2.categoryid
  		WHERE I2.id IN (?????) -- add the necessary questionmark
  		AND C.Id <> 0) --Uncategorized  
  
  GROUP BY R.id
  ORDER BY count(R.id) DESC; */