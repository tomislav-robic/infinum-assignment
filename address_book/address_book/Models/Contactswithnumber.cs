using System;
using System.Collections.Generic;

#nullable disable

namespace address_book.Models
{
    public partial class Contactswithnumber
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Numbers { get; set; }
    }
}
