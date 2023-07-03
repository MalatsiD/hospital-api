namespace Hospital_API.Entities
{
    public class PersonAddress
    {
        public int PersonId { get; set; }
        public Person? Person { get; set; }
        public int AddressId { get; set; }
        public Address? Address { get; set; }

        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
    }
}
