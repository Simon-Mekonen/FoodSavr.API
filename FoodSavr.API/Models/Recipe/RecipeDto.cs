﻿namespace FoodSavr.API.Models
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImgLink { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }
        public List<RecipeStepsDto>? Steps { get; set; }
        public List<RecipeIngredientDto>? Ingredients { get; set;}
    }
}