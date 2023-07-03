namespace Hospital_API.Entities
{
    public class Pharmaceutical
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int PharmaceuticalCategoryId { get; set; }
        public PharmaceuticalCategory? PharmaceuticalCategory { get; set; }
    }
}
