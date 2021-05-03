using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CapacityController : BaseApiController 
    {
        private readonly IMapper _mapper;

        public CapacityController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        
        [HttpPost("AddMachineCapacityEntry")]
        public async Task<ActionResult<MachineCapacityEntryDTO>> AddMachine(MachineCapacityEntryDTO requestDTO)
        {
            try
            {
                string[] validformats = new[] { "MM/dd/yyyy", "M/dd/yyyy", "M/d/yyyy", "dd/MM/yyyy" };
                CultureInfo provider = new CultureInfo("en-GB");

                MachineCapacityEntry entry = new MachineCapacityEntry
                {
                    MachineId = requestDTO.MachineId,
                    Date = DateTime.ParseExact(requestDTO.Date, validformats, provider),
                    StartTime = TimeSpan.Parse(requestDTO.StartTime),
                    Capacity = requestDTO.Capacity
                };
                _context.MachineCapacityEntries.Add(entry);
                await _context.SaveChangesAsync();
                return new MachineCapacityEntryDTO
                {
                    EntryNo = entry.EntryNo
                };
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("GetSpareCapacityView")]
        public async Task<ActionResult<SpareCapacitiesResponse>> GetSpareCapacityView([FromBody] PagingRequest paging)
        {
            var pagingResponse = new SpareCapacitiesResponse()
            {
                Draw = paging.Draw
            };
            try
            {
                if (!paging.SearchCriteria.IsPageLoad)
                {
                    int locationId = 0;
                    int machineId = 0;
                    if (!string.IsNullOrEmpty(paging.SearchCriteria?.Filter))
                    {
                        var ids = paging.SearchCriteria.Filter.Split('|');
                        locationId = Convert.ToInt32(ids[0]);
                        machineId = Convert.ToInt32(ids[1]);
                    }
                    List<SpareCapacityViewDTO> query = null;
                    var userInfo = await GetUserInfo();
                    query = _mapper.Map<List<SpareCapacityViewDTO>>(await _context.SpareCapacities.Where(x => x.ManufacturerId == userInfo.ManufacturerId &&
                    x.LocationId == locationId && x.MachineId == machineId).ToListAsync());

                    var recordsTotal = query.Count();
                    pagingResponse.Data = query.Skip(paging.Start).Take(paging.Length).ToArray();
                    pagingResponse.RecordsTotal = recordsTotal;
                    pagingResponse.RecordsFiltered = recordsTotal;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
            return Ok(pagingResponse);
        }

        [HttpPost("GetCapacityPageInfo")]
        public async Task<ActionResult<SpareCapacityViewDTO>> GetCapacityPageInfo(SpareCapacityViewDTO requestDTO)
        {
            try
            {
                var userInfo = await GetUserInfo();
                return new SpareCapacityViewDTO
                {
                    ManufacturerName = _context.Manufacturers.FirstOrDefault(x => x.Id == userInfo.ManufacturerId)?.Name,
                    LocationName = _context.ManufacturingLocations.FirstOrDefault(x => x.Id == requestDTO.LocationId)?.Name,
                    MachineName = _context.Machines.FirstOrDefault(x => x.Id == requestDTO.MachineId)?.Name,
                };
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("DeleteCapacityEntry")]
        public async Task<ActionResult<bool>> DeleteCapacityEntry(MachineCapacityEntryDTO machineDTO)
        {
            try
            {
                var capacityEntry = await _context.MachineCapacityEntries.FirstOrDefaultAsync(x => x.EntryNo == machineDTO.EntryNo);
                if (capacityEntry != null)
                {
                    _context.MachineCapacityEntries.Remove(capacityEntry);
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
    }
}