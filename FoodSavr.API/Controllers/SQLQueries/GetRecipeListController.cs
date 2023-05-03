using FoodSavr.API.DbContexts;
using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
            command.CommandText =
                "SELECT " +
                    "R.id, " +
                    "count(R.id) AS matches, " +
                    "R.name, " +
                    "R.description, " +
                    "R.imglink, " +
                    "R.portions, " +
                    "R.cookingtime AS time " +
                "FROM recipeingredient AS RIL " +
                "JOIN recipe AS R on RIL.recipeid = R.id " +
                "JOIN ingredient AS I on I.id = RIL.ingredientId " +
                "WHERE I.categoryid IN (" +
                        "SELECT I2.categoryid " +
                        "FROM ingredient AS I2 " +
                        "JOIN category AS C ON C.id = I2.categoryid " +
                        $"WHERE I2.id IN ({string.Join(",", ingredients)}) " +
                        "AND C.Id <> 0) /*Uncategorized*/ " +
                "GROUP BY R.id " +
                "ORDER BY count(R.id) DESC;";

            using (var reader = command.ExecuteReader())
            {
                var results = new List<RecipeBlobDto>();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var matches = reader.GetInt32(1);
                    var name = reader.GetString(2);
                    var description = reader.GetString(3);
                    var imgLink = reader.GetString(4);
                    var portions = reader.GetInt32(5);
                    var cookingTime = reader.GetInt32(6);

                    var recipe = new RecipeBlobDto()
                    {
                        Id = id,
                        Matches = matches,
                        Name = name,
                        Description = description,
                        ImgLink = imgLink,
                        Portions = portions,
                        CookingTime = cookingTime,
                    };
                    results.Add(recipe);
                }

                return View(results);

            }
        }
    }
}
