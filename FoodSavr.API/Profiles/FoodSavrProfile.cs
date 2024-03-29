﻿using AutoMapper;
using FoodSavr.API.Entities;

namespace FoodSavr.API.Profiles
{
    public class FoodSavrProfile : Profile
    {
        public FoodSavrProfile() 
        {
            CreateMap<Entities.Ingredient, Models.IngredientDto>();
            CreateMap<Models.IngredientDto, Entities.Ingredient>();

            CreateMap<Entities.Ingredient, Models.IngredientForCreationDto>();
            CreateMap<Models.IngredientForCreationDto, Entities.Ingredient>();

            CreateMap<Entities.Recipe, Models.RecipeDto>();
            CreateMap<Entities.RecipeSteps, Models.RecipeStepsDto>(MemberList.Destination);
        }
    }
}
