using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IMapper _mapper;

        public UserController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // api/contacts/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
        }

    
        [HttpGet("GetUserAddresses")]
        [AllowAnonymous]
        public async Task<ActionResult<List<AddressDTO>>> GetUserAddresses()
        {
            try
            {
                var userInfo = await GetUserInfo();
                return _mapper.Map<List<AddressDTO>>(await _context.Addresses.Where(x => x.UserId == userInfo.Userid).ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("GetUserManufacturingProcessForDropDown")]
        public async Task<ActionResult<List<ManufacturingProcessDTO>>> GetUserManufacturingProcessForDropDown()
        {
            try
            {
                var userInfo = await GetUserInfo();
                return _mapper.Map<List<ManufacturingProcessDTO>>(await _context.ManufacturingProcesses.ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        
        [HttpPost("AddAddress")]
        public async Task<ActionResult<AddressDTO>> AddAddress(AddressDTO addressDTO)
        {
            try
            {
                Address address = new Address
                {
                    Name = addressDTO.Name,
                    Address1 = addressDTO.Address1,
                    Address2 = addressDTO.Address2,
                    City = addressDTO.City,
                    County = addressDTO.County,
                    Postcode = addressDTO.Postcode,
                    CountryCode = addressDTO.CountryCode,
                    Telephone = addressDTO.Telephone,
                    Email = addressDTO.Email,
                    Fax = addressDTO.Fax,
                    UserId = GetUserId()
                };
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();
                return new AddressDTO
                {
                    Id = address.Id,
                    Name = address.Name
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("GetLoggedInUserInfo")]
        public async Task<ActionResult<User>> GetLoggedInUserInfo()
        {
            var userInfo = await GetUserWithCustomerAndManufactureInfo();
            return userInfo;
        }
    }
        
}