using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        public readonly DataContext _context;
        public BaseApiController(DataContext context)
        {
            _context = context;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        protected Guid GetUserId()
        {
            var d = this.User.Claims.ToList();
            return Guid.Parse(this.User.Claims.First(i => i.Type == "sid").Value);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<User> GetUserInfo()
        {
            var userId = GetUserId();
            return await _context.Users.FirstOrDefaultAsync(user => user.Userid == userId);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<User> GetUserWithCustomerAndManufactureInfo()
        {
            return await _context.Users.Include(x => x.Customer).Include(c => c.Manufacturer).FirstOrDefaultAsync(x => x.Userid == GetUserId());
        }

    }
}