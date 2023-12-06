using FoodSavr.API.Entities;
using FoodSavr.API.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodSavr.API.DbContexts
{
    public class FoodSavrDbContext: DbContext
    {
        //add all the tables and set the keys/foreign keys connection
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Measurement> Measurement { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredient { get; set; }
        public DbSet<RecipeSteps> RecipeSteps { get; set; }

        public FoodSavrDbContext(DbContextOptions<FoodSavrDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
