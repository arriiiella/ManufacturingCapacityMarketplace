using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LookUpController : BaseApiController
    {
        private readonly IMapper _mapper;

        public LookUpController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        
        [HttpGet("GetIndustries")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Industry>>> GetIndustries()
        {
            var list = await _context.Industries.ToListAsync();
            foreach (var item in list)
            {
                item.Manufacturers = null;
                item.ManufacturingProcesses = null;
            }
            return list;
        }

        [HttpGet("GetCountries")]
        [AllowAnonymous]
        public async Task<ActionResult<List<CountryCode>>> GetCountries()
        {
            return await _context.CountryCodes.ToListAsync();
        }
    }
}