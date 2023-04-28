using AutoMapper;

namespace FoodSavr.API.Profiles
{
    public class FoodSavrProfile : Profile
    {
        public FoodSavrProfile() 
        {
            CreateMap<Entities.Ingredient, Models.IngredientDto>();
            CreateMap<Entities.Ingredient, Models.IngredientForCreationDto>();

            CreateMap<Entities.Recipe, Models.RecipeDto>();
        }
    }
}
