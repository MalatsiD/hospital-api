namespace Hospital_API.Entities
{
    public class HospitalAddress
    {
        public int HospitalId { get; set; }
        public Hospital Hospital { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
    }
}
