namespace KMC_API.Model
{
    using System.ComponentModel.DataAnnotations;
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}