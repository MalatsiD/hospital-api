namespace Hospital_API.Entities
{
    public class HospitalAddress
    {
        public int HospitalId { get; set; }
        public virtual Hospital? Hospital { get; set; }
        public int AddressId { get; set; }
        public virtual Address? Address { get; set; }

        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
    }
}
