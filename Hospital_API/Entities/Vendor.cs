namespace Hospital_API.Entities
{
    public class Vendor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public long PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int HospitalId { get; set; }
        public virtual Hospital? Hospital { get; set; }

        public virtual ICollection<PharmaceuticalCategory>? PharmaceuticalCategories { get; set; }
    }
}
