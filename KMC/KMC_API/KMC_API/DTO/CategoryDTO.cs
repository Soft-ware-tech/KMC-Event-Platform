namespace KMC_API.DTO
{
    public class CategoryReadDTO
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }

    public class CategoryCreateDTO
    {
        public string Name { get; set; }
    }
}