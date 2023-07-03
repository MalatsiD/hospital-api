namespace Hospital_API.Entities
{
    public class Province
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int CountryId { get; set; }
        public virtual Country? Country { get; set; }

        public virtual ICollection<City>? Cities { get; set; }


    }
}
