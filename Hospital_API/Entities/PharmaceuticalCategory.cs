﻿namespace Hospital_API.Entities
{
    public class PharmaceuticalCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Active { get; set; }

        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }

        public ICollection<Pharmaceutical> Pharmaceuticals { get; set; }
    }
}
