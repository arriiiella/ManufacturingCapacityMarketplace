using API.DTOs;
using API.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Address, AddressDTO>();
            CreateMap<Machine, MachineDTO>();
            CreateMap<ManufacturingLocation, ManufacturerLocationDTO>();
            CreateMap<ManufacturingProcess, ManufacturingProcessDTO>();
            CreateMap<SpareCapacity, SpareCapacityViewDTO>();
            CreateMap<OrderWithDetails, OrderWithDetailsDTO>();
            CreateMap<Models.Order, OrderDTO>();
            CreateMap<OrderLine, OrderLineDTO>();
        }
    }
}
