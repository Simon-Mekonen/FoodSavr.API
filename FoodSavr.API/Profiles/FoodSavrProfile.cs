using AutoMapper;

namespace FoodSavr.API.Profiles
{
    public class FoodSavrProfile : Profile
    {
        public FoodSavrProfile() 
        {
            CreateMap<Entities.Ingredient, Models.IngredientDto>();
            CreateMap<Entities.Recipe, Models.RecipeDto>();
        }
    }
}
