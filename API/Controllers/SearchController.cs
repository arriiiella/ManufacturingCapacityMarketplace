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
    public class SearchController : BaseApiController
    {
        private readonly IMapper _mapper;

        public SearchController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        
        [HttpGet("GetSearchViewCities")]
        public async Task<ActionResult<List<string>>> GetSearchViewCities()
        {
            var cities = await _context.SpareCapacities.Select(c => c.City).Distinct().ToListAsync();
            return cities;
        }

        [HttpPost("FilterSpareCapacity")]
        public async Task<ActionResult<SpareCapacitiesResponse>> FilterSpareCapacity([FromBody] SpareCapacityPagingRequest paging)
        {
            var pagingResponse = new SpareCapacitiesResponse()
            {
                Draw = paging.Draw
            };
            try
            {
                if (!paging.SearchCriteriaSpareCapacity.IsPageLoad)
                {
                    if (paging.SearchCriteriaSpareCapacity.IndustryId == null && paging.SearchCriteriaSpareCapacity.ProcessId == null && paging.SearchCriteriaSpareCapacity.City == null && paging.SearchCriteriaSpareCapacity.StartDate == null && paging.SearchCriteriaSpareCapacity.EndDate == null)
                    {
                        return pagingResponse;
                    }
                    List<SpareCapacityViewDTO> query = null;
                    var result = _context.SpareCapacities.AsQueryable();
                    if (paging.SearchCriteriaSpareCapacity.IndustryId.HasValue && paging.SearchCriteriaSpareCapacity?.IndustryId.Value > 0)
                    {
                        result = result.Where(x => x.IndustryId == paging.SearchCriteriaSpareCapacity.IndustryId.Value);
                    }
                    if (paging.SearchCriteriaSpareCapacity.ProcessId.HasValue && paging.SearchCriteriaSpareCapacity?.ProcessId.Value > 0)
                    {
                        result = result.Where(x => x.ProcessId == paging.SearchCriteriaSpareCapacity.ProcessId.Value);
                    }
                    if (!string.IsNullOrEmpty(paging.SearchCriteriaSpareCapacity.City))
                    {
                        result = result.Where(x => x.City == paging.SearchCriteriaSpareCapacity.City);
                    }
                    if (!string.IsNullOrEmpty(paging.SearchCriteriaSpareCapacity.StartDate) || !string.IsNullOrEmpty(paging.SearchCriteriaSpareCapacity.EndDate))
                    {
                        string[] validformats = new[] { "MM/dd/yyyy", "M/dd/yyyy", "M/d/yyyy", "dd/MM/yyyy" };
                        CultureInfo provider = new CultureInfo("en-GB");
                        var sDate = DateTime.ParseExact(paging.SearchCriteriaSpareCapacity.StartDate, validformats, provider);
                        var eDate = DateTime.ParseExact(paging.SearchCriteriaSpareCapacity.EndDate, validformats, provider);

                        result = result.Where(x => x.Date >= sDate && x.Date <= eDate);
                    }

                    query = _mapper.Map<List<SpareCapacityViewDTO>>(result);
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
    }
}