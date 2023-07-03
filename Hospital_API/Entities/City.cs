namespace Hospital_API.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int ProvinceId { get; set; }
        public virtual Province? Province { get; set; }
    }
}
