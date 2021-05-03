using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class LocationController : BaseApiController
    {
        private readonly IMapper _mapper;

        public LocationController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        [HttpPost("DeleteUserManufacturingLocation")]
        public async Task<ActionResult<bool>> DeleteUserManufacturingLocation(ManufacturerLocationDTO manufacturerLocationDTO)
        {
            try
            {
                var manufacturingLocation = await _context.ManufacturingLocations.FirstOrDefaultAsync(x => x.Id == manufacturerLocationDTO.Id);
                if (manufacturingLocation != null)
                {
                    _context.ManufacturingLocations.Remove(manufacturingLocation);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("GetUserManufacturingLocation")]
        public async Task<ActionResult<PagingManufacuringLocationResponse>> GetUserManufacturingLocation([FromBody] PagingRequest paging)
        {
            var pagingResponse = new PagingManufacuringLocationResponse()
            {
                Draw = paging.Draw
            };

            try
            {
                if (!paging.SearchCriteria.IsPageLoad)
                {
                    List<ManufacturerLocationDTO> query = null;
                    var userInfo = await GetUserInfo();
                    if (!string.IsNullOrEmpty(paging.SearchCriteria.Filter))
                    {
                        query = _mapper.Map<List<ManufacturerLocationDTO>>(_context.ManufacturingLocations.Where(x => x.ManufacturerId == userInfo.ManufacturerId).ToList());
                    }
                    else
                    {
                        query = _mapper.Map<List<ManufacturerLocationDTO>>(_context.ManufacturingLocations.Include(x => x.Address).Where(x => x.ManufacturerId == userInfo.ManufacturerId).ToList());
                    }

                    var recordsTotal = query.Count();
                    pagingResponse.Data = query.Skip(paging.Start).Take(paging.Length).ToArray();
                    pagingResponse.RecordsTotal = recordsTotal;
                    pagingResponse.RecordsFiltered = recordsTotal;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok(pagingResponse);
        }

        [HttpPost("GetUserManufacturingLocationForDropDown")]
        public async Task<ActionResult<List<ManufacturerLocationDTO>>> GetUserManufacturingLocationForDropDown()
        {
            try
            {
                var userInfo = await GetUserInfo();
                return _mapper.Map<List<ManufacturerLocationDTO>>(await _context.ManufacturingLocations.Include(x => x.Address).Where(x => x.ManufacturerId == userInfo.ManufacturerId).ToListAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("AddManufacturerLocation")]
        public async Task<ActionResult<ManufacturerLocationDTO>> AddManufacturerLocation(ManufacturerLocationDTO requestDTO)
        {
            try
            {

                var userInfo = await GetUserInfo();
                ManufacturingLocation manufacturingLocation = new ManufacturingLocation
                {
                    Name = requestDTO.Name,
                    ManufacturerId = userInfo.ManufacturerId ?? 0,
                    AddressId = requestDTO.AddressId
                };
                _context.ManufacturingLocations.Add(manufacturingLocation);
                await _context.SaveChangesAsync();
                return new ManufacturerLocationDTO
                {
                    Id = manufacturingLocation.Id
                };
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        
        [HttpPut("UpdateUserManufacturingLocation")]
        public async Task<ActionResult<bool>> UpdateUserManufacturingLocation(ManufacturerLocationDTO manufacturerLocationDTO)
        {
            try
            {
                var locationInfo = await _context.ManufacturingLocations.FirstOrDefaultAsync(x => x.Id == manufacturerLocationDTO.Id);
                if (locationInfo != null)
                {
                    _context.ManufacturingLocations.Update(locationInfo);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return NotFound();

            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}