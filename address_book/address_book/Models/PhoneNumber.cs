using System;
using System.Collections.Generic;

#nullable disable

namespace address_book.Models
{
    public partial class PhoneNumber
    {
        public int Id { get; set; }
        public int ContactsId { get; set; }
        public string Number { get; set; }

        public virtual Contact Contacts { get; set; }
    }
}
