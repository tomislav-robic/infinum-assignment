using System;
using System.Collections.Generic;

#nullable disable

namespace address_book.Models
{
    public partial class Contact
    {
        public Contact()
        {
            PhoneNumbers = new HashSet<PhoneNumber>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
