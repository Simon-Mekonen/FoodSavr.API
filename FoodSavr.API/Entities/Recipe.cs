using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Entities
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [MaxLength(100)]
        [Required]
        [MinLength(3)]
        public string ImgLink { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }

        public Recipe(string name, string description, string imgLink, int portions, int cookingTime)
        {
            Name = name;
            Description = description;
            ImgLink = imgLink;
            Portions = portions;
            CookingTime = cookingTime;
        }
    }

}
