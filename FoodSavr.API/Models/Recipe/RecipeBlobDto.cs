namespace FoodSavr.API.Models
{
    public class RecipeBlobDto
    {
        public int Id { get; set; }
        public int Matches { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImgLink { get; set; }
        public int Portions { get; set; }
        public int CookingTime { get; set; }
    }
}