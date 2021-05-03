using API.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDTO
    {
        public UserRequestDTO User { get; set; }
        public CustomerDTO Customer { get; set; }
        public ManufacturerDTO Manufacturer { get; set; }
        public List<AddressDTO> Addresses { get; set; }
    }
}