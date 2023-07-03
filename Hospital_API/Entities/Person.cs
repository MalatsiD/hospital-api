namespace Hospital_API.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set;}
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public long PhoneNumber { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int TitleId { get; set; }
        public virtual Title? Title { get; set; }
        public int GenderId { get; set; }
        public virtual Gender? Gender { get; set; }

        public virtual Employee? Employee { get; set; }
        public virtual Patient? Patient { get; set; }
        public virtual ICollection<PersonAddress>? Addresses { get; set; }
    }
}
