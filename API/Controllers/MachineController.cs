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
    public class MachineController : BaseApiController
    {
        private readonly IMapper _mapper;

        public MachineController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        
        [HttpPost("AddMachine")]
        public async Task<ActionResult<MachineDTO>> AddMachine(MachineDTO requestDTO)
        {
            try
            {
                Machine machine = new Machine
                {
                    Name = requestDTO.Name,
                    ModelNo = requestDTO.ModelNo,
                    Capacity = requestDTO.Capacity,
                    SetupTime = requestDTO.SetupTime,
                    LocationId = requestDTO.LocationId,
                    CostPerUnit = requestDTO.CostPerUnit,
                    ManufacturingProcessId = requestDTO.ManufacturingProcessId
                };
                _context.Machines.Add(machine);
                await _context.SaveChangesAsync();
                return new MachineDTO
                {
                    Id = machine.Id
                };
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("UpdateUserMachine")]
        public async Task<ActionResult<bool>> UpdateUserMachine(MachineDTO machineDTO)
        {
            try
            {
                var machineInfo = await _context.Machines.FirstOrDefaultAsync(x => x.Id == machineDTO.Id);
                if (machineInfo != null)
                {
                    _context.Machines.Update(machineInfo);
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

        [HttpPost("GetUserMachines")]
        public async Task<ActionResult<PagingMachineResponse>> GetUserMachines([FromBody] PagingRequest paging)
        {
            var pagingResponse = new PagingMachineResponse()
            {
                Draw = paging.Draw
            };
            try
            {
                if (!paging.SearchCriteria.IsPageLoad)
                {
                    int locationId = 0;
                    if (!string.IsNullOrEmpty(paging.SearchCriteria?.Filter))
                    {
                        locationId = Convert.ToInt32(paging.SearchCriteria.Filter);
                    }
                    List<MachineDTO> query = null;
                    var userInfo = await GetUserInfo();
                    var machineDb = await _context.ManufacturingLocations.FirstOrDefaultAsync(x => x.ManufacturerId == userInfo.ManufacturerId);
                    machineDb.Machines = await _context.Machines.Where(x => x.LocationId == locationId).ToListAsync();
                    machineDb.Machines.ToList().ForEach(f =>
                    {
                        f.ManufacturingProcess = _context.ManufacturingProcesses.FirstOrDefault(x => x.Id == f.ManufacturingProcessId);
                    });
                    query = machineDb.Machines.Select(x => new MachineDTO
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ModelNo = x.ModelNo,
                        Capacity = x.Capacity,
                        SetupTime = x.SetupTime,
                        CostPerUnit = x.CostPerUnit,
                        ManufacturingProcess = new ManufacturingProcessDTO
                        {
                            Id = x.ManufacturingProcess?.Id ?? 0,
                            Name = x.ManufacturingProcess?.Name
                        }
                    }).ToList();
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

        
        [HttpPost("DeleteUserMachine")]
        public async Task<ActionResult<bool>> DeleteUserMachine(MachineDTO machineDTO)
        {
            try
            {
                var machineInfo = await _context.Machines.FirstOrDefaultAsync(x => x.Id == machineDTO.Id);
                if (machineInfo != null)
                {
                    _context.Machines.Remove(machineInfo);
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