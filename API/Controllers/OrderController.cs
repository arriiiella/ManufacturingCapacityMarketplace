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
    public class OrderController : BaseApiController
    {
        private readonly IMapper _mapper;

        public OrderController(DataContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }
        

        [HttpPost("GetOrderConfirmationDetails")]
        public async Task<ActionResult<ILookup<int, SpareCapacity>>> GetOrderConfirmationDetails(OrderConfirmationRequestDTO request)
        {
            try
            {
                var ordersInfo = LoadOrdersByCapacityEntryNo(request.CapacityEntryNumbers);
                if (ordersInfo == null)
                {
                    return BadRequest("Orders not found!");
                }
                var result = ordersInfo.ToLookup(y => y.ManufacturerId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpPost("GetCustomerOrders")]
        public async Task<ActionResult<OrdersPagingResponse>> GetCustomerOrders ([FromBody] PagingRequest paging)
        {
            var pagingResponse = new OrdersPagingResponse()
            {
                Draw = paging.Draw
            };
            try
            {
                if (!paging.SearchCriteria.IsPageLoad)
                {
                    List<OrderDTO> query = null;
                    var userInfo = await GetUserInfo();
                    var result = _context.Orders.Include(x => x.OrderLines).Where(o => o.CustomerId == userInfo.CustomerId).AsQueryable();
                    await result.ForEachAsync(x =>
                    {
                        foreach (var item in x.OrderLines)
                        {
                            item.OrderNoNavigation = null;
                        }
                    });
                    query = _mapper.Map<List<OrderDTO>>(result);
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

        [HttpPost("GetManufacturerOrders")]
        public async Task<ActionResult<OrdersPagingResponse>> GetManufacturerOrders([FromBody] PagingRequest paging)
        {
            var pagingResponse = new OrdersPagingResponse()
            {
                Draw = paging.Draw
            };
            try
            {
                if (!paging.SearchCriteria.IsPageLoad)
                {
                    List<OrderDTO> query = null;
                    var userInfo = await GetUserInfo();
                    var result = _context.Orders.Include(x => x.OrderLines).Where(o => o.ManufacturerId == userInfo.ManufacturerId).AsQueryable();
                    await result.ForEachAsync(x =>
                    {
                        foreach (var item in x.OrderLines)
                        {
                            item.OrderNoNavigation = null;
                        }
                    });
                    query = _mapper.Map<List<OrderDTO>>(result);
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

        [HttpPost("GetOrderByOrderNo")]
        public async Task<ActionResult<List<Models.Order>>> GetOrderByOrderNo(OrderDTO orderDTO)
        {
            try
            {
                var orders = await _context.OrderWithDetails.Where(o => o.OrderNumber == orderDTO.OrderNo).ToListAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("CreateOrder")]
        public async Task<ActionResult<bool>> CreateOrder(OrderConfirmationRequestDTO request)
        {
            try
            {
                var ordersInfo = LoadOrdersByCapacityEntryNo(request.CapacityEntryNumbers).ToLookup(x => x.ManufacturerId);
                if (ordersInfo == null)
                {
                    return BadRequest("Orders not found!");
                }
                var userInfo = await GetUserWithCustomerAndManufactureInfo();

                foreach (var orderInfo in ordersInfo)
                {
                    int orderLineNo = 1;
                    var order = new Models.Order
                    {
                        CustomerId = userInfo.Customer.Id,
                        CustomerName = userInfo?.Customer?.Name,
                        OrderDate = DateTime.Now,
                        ManufacturerId = orderInfo.Key,
                        ManufacturerName = orderInfo.FirstOrDefault(x => x.ManufacturerId == orderInfo.Key).ManufacturerName,
                        OrderedByName = userInfo?.Customer?.Name,
                        Fulfilled = false
                    };
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    foreach (var orderIn in orderInfo)
                    {
                        var orderLine = new OrderLine()
                        {
                            OrderNo = order.OrderNo,
                            LineNo = orderLineNo++,
                            LocationId = orderIn.LocationId,
                            MachineId = orderIn.MachineId,
                            Capacity = orderIn.Capacity,
                            CostPerUnit = orderIn.CostPerUnit,
                            LineAmount = orderIn.CapacityCost,
                            CapacityEntryNo = orderIn.CapacityEntryNo
                        };
                        _context.OrderLines.Add(orderLine);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IQueryable<SpareCapacity> LoadOrdersByCapacityEntryNo(string requestCapacityEntryNumbers)
        {
            try
            {
                var capacityEntryNumbers = requestCapacityEntryNumbers.Split(",").Select(int.Parse);
                return _context.SpareCapacities.Where(x => capacityEntryNumbers.Contains(x.CapacityEntryNo));
            }
            catch
            {
                return null;
            }
        }
    }
}