using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        public AccountController(ITokenService tokenService, DataContext context) : base(context)
        {
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {

            if (await UserExists(registerDTO.User.Username)) return BadRequest("Username is taken");

            using var hmac = new HMACSHA512();
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = new User
                {
                    FirstName = registerDTO.User.FirstName,
                    LastName = registerDTO.User.LastName,
                    Salutation = registerDTO.User.Salutation,
                    Email = registerDTO.User.Email,
                    Username = registerDTO.User.Username.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.User.Password)),
                    PasswordSalt = hmac.Key
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                foreach (var addressDto in registerDTO.Addresses)
                {
                    Address address = new Address
                    {
                        Name = addressDto.Name,
                        Address1 = addressDto.Address1,
                        Address2 = addressDto.Address2,
                        City = addressDto.City,
                        County = addressDto.County,
                        Postcode = addressDto.Postcode,
                        CountryCode = addressDto.CountryCode,
                        Telephone = addressDto.Telephone,
                        Email = addressDto.Email,
                        Fax = addressDto.Fax,
                        UserId = user.Userid
                    };
                    _context.Addresses.Add(address);
                    await _context.SaveChangesAsync();
                    addressDto.DbId = address.Id;
                }

                Customer customer = new Customer
                {
                    Name = registerDTO.Customer.Name,
                    IndustryId = registerDTO.Customer.IndustryId,
                    AddressId = registerDTO.Addresses.FirstOrDefault(x => x.Id == registerDTO.Customer.AddressId).DbId,
                    VatRegistrationNo = registerDTO.Customer.VatRegistrationNo,
                    BillingAddressId = registerDTO.Addresses.FirstOrDefault(x => x.Id == registerDTO.Customer.BillingAddressId).DbId,
                    IsPurchaseCapacity = registerDTO.Customer.IsPurchaseCapacity,
                    IsSellCapacity = registerDTO.Customer.IsSellCapacity
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                int? manufacturerId = null;
                if (registerDTO.Manufacturer != null)
                {
                    registerDTO.Manufacturer.FeeRateId = registerDTO.Manufacturer.FeeRateId == 0 ? null : registerDTO.Manufacturer.FeeRateId;
                    Manufacturer manufacturer = new Manufacturer
                    {
                        Name = registerDTO.Manufacturer.Name,
                        IndustryId = registerDTO.Manufacturer.IndustryId,
                        AddressId = registerDTO.Addresses.FirstOrDefault(x => x.Id == registerDTO.Manufacturer.AddressId).DbId,
                        VatRegistrationNo = registerDTO.Manufacturer.VatRegistrationNo,
                        BillingAddressId = registerDTO.Addresses.FirstOrDefault(x => x.Id == registerDTO.Manufacturer.BillingAddressId).DbId,
                    };
                    _context.Manufacturers.Add(manufacturer);
                    await _context.SaveChangesAsync();
                    manufacturerId = manufacturer.Id;
                }
                var userInfo = await _context.Users.FirstOrDefaultAsync(u => u.Userid == user.Userid);
                userInfo.CustomerId = customer.Id;
                userInfo.ManufacturerId = manufacturerId;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new UserDTO
                {
                    Username = user.Username
                };
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, ex);
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            //return the only element of a sequence or default if sequence is empty
            var user = await _context.Users
                .SingleOrDefaultAsync(user => user.Username == loginDTO.Username);

            if (user == null)
            {
                return Unauthorized("Invalid Username");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid Password");
                }
            }

            return new UserDTO
            {
                Username = user.Username,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username == username.ToLower());
        }
    }
}