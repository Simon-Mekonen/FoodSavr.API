using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Entities
{
    public class RecipeIngredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int? Quantity { get; set; }
        public int? MeasurementId { get; set; }
        public int IngredientId { get; set; }

        [ForeignKey("RecipeId")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey("MeasurementId")]
        public virtual Measurement? Measurement { get; set; }
        [ForeignKey("IngredientId")]
        public virtual Ingredient Ingredient { get; set; }

        public RecipeIngredient()
        {
        }
        public RecipeIngredient(int recipeId, int? quantity, int? measurementId, int ingredientId)
        {
            RecipeId = recipeId;
            Quantity = quantity;
            MeasurementId = measurementId;
            IngredientId = ingredientId;
        }
    }

}
