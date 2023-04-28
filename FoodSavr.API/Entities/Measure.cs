using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodSavr.API.Entities
{
    public class Measurement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(20)]
        [Required]
        [MinLength(3)]
        public string Measure { get; set; }

        public Measurement(string measure)
        {
            Measure = measure;
        }
    }

}
