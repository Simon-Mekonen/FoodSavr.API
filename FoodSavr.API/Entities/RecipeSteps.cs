using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Entities
{
    public class RecipeSteps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int Row { get; set; }
        [Required]
        public string Text { get; set; }
        [ForeignKey("RecipeId")]

        public virtual Recipe Recipe { get; set; }

        public RecipeSteps(int recipeId, int row, string text)
        {
            RecipeId = recipeId;
            Row = row;
            Text = text;
        }
    }

}
