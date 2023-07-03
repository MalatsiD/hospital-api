namespace Hospital_API.Entities
{
    public class Address
    {
        public int Id { get; set; }        
        public string? AddressDetail { get; set; } //street Address/PO BOX/ Private Bag
        public int ZipCode { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public int AddressTypeId { get; set; }
        public AddressType AddressType { get; set; }

        public ICollection<HospitalAddress> Hospitals { get; set; }
        public ICollection<PersonAddress> People { get; set; }
    }
}
