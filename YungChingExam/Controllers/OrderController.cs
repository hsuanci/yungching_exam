using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YungChingExam.Data.DTOs;
using YungChingExam.Helpers;
using YungChingExam.Service.interfaces;
using YungChingExam.ViewModel;

namespace YungChingExam.Controllers
{
    [ApiController]
    public class OrderController : BaseAPIController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService
            )
        {
            _logger = logger;
            _orderService = orderService;
        }

        /// <summary>
        /// Get a single Order by ID
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderPageViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var orderDto = await _orderService.GetOrderByIdAsync(orderId);

            if (orderDto == null)
            {
                return NotFound();
            }

            var orderPageViewModel = new OrderPageViewModel
            {
                CustomerId = orderDto.CustomerId,
                CustomerName = orderDto.CustomerName,
                EmployeeId = orderDto.EmployeeId,
                EmployeeName = orderDto.EmployeeName,
                OrderDate = orderDto.OrderDate,
                RequiredDate = orderDto.RequiredDate,
                ShippedDate = orderDto.ShippedDate,
                ShipVia = orderDto.ShipVia,
                ShipperCompanyName = orderDto.ShipperCompanyName,
                Freight = orderDto.Freight,
                ShipName = orderDto.ShipName,
                ShipAddress = orderDto.ShipAddress,
                ShipCity = orderDto.ShipCity,
                ShipRegion = orderDto.ShipRegion,
                ShipPostalCode = orderDto.ShipPostalCode,
                ShipCountry = orderDto.ShipCountry,
                OrderDetails = orderDto.OrderDetails.Select(detail => new OrderDetailPageViewModel
                {
                    ProductId = detail.ProductId,
                    ProductName = detail.ProductName,
                    UnitPrice = detail.UnitPrice,
                    Quantity = detail.Quantity,
                    Discount = detail.Discount
                }).ToList()
            };

            return Ok(orderPageViewModel);
        }


        /// <summary>
        /// Get Orders
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(OrderPaginationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetOrders([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var orderPaginationDto = await _orderService.GetOrderListAsync(pageNumber, pageSize);

            return Ok(this.MapToOrderPageViewModel(orderPaginationDto));
        }

        /// <summary>
        /// Create Order
        /// </summary>
        /// <param name="vm">OrderCreateViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostOrder([FromBody] OrderViewModel vm)
        {
            // Get EmployeeId from JWT > Helper
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return Unauthorized();
            }

            var subClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (subClaim == null || !int.TryParse(subClaim.Value, out int employeeId))
            {
                return BadRequest("Cannot get sub information or sub is not a valid integer");
            }

            var orderDto = new OrderDto
            {
                CustomerId = vm.CustomerId,
                EmployeeId = employeeId,
                OrderDate = TimeHelper.GetCurrentDateTime(),
                RequiredDate = TimeHelper.GetCurrentDateTime(),
                ShipVia = vm.ShipVia,
                Freight = vm.Freight,
                ShipName = vm.ShipName,
                ShipAddress = vm.ShipAddress,
                ShipCity = vm.ShipCity,
                ShipRegion = vm.ShipRegion,
                ShipPostalCode = vm.ShipPostalCode,
                ShipCountry = vm.ShipCountry,
                OrderDetails = vm.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                })
                .ToList()
            };

            await _orderService.CreateOrderAsync(orderDto, vm.useCustomerCurrentAddressState);

            return Ok();
        }

        /// <summary>
        /// Update Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPut("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PutOrder([FromRoute] int orderId, [FromBody] OrderViewModel vm)
        {
            // Get EmployeeId from JWT > Helper
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity == null)
            {
                return Unauthorized();
            }

            var subClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (subClaim == null || !int.TryParse(subClaim.Value, out int employeeId))
            {
                return BadRequest("Cannot get sub information or sub is not a valid integer");
            }

            var orderDto = new OrderDto
            {
                CustomerId = vm.CustomerId,
                EmployeeId = employeeId,
                RequiredDate = TimeHelper.GetCurrentDateTime(),
                ShipVia = vm.ShipVia,
                Freight = vm.Freight,
                ShipName = vm.ShipName,
                ShipAddress = vm.ShipAddress,
                ShipCity = vm.ShipCity,
                ShipRegion = vm.ShipRegion,
                ShipPostalCode = vm.ShipPostalCode,
                ShipCountry = vm.ShipCountry,
                OrderDetails = vm.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                })
                .ToList()
            };

            await _orderService.UpdateOrderAsync(orderId, orderDto, vm.useCustomerCurrentAddressState);

            return Ok();
        }

        /// <summary>
        /// Delete Order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpDelete("{orderId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteOrder([FromRoute] int orderId)
        {
            await _orderService.DeleteOrderAsync(orderId);

            return Ok();
        }

        #region Private
        private OrderPaginationViewModel MapToOrderPageViewModel(OrderPaginationDto dto)
        {
            return new OrderPaginationViewModel
            {
                Orders = dto.Orders.Select(order => new OrderPageViewModel
                {
                    CustomerId = order.CustomerId,
                    CustomerName = order.CustomerName,
                    EmployeeId = order.EmployeeId,
                    EmployeeName = order.EmployeeName,
                    OrderDate = order.OrderDate,
                    RequiredDate = order.RequiredDate,
                    ShippedDate = order.ShippedDate,
                    ShipVia = order.ShipVia,
                    ShipperCompanyName = order.ShipperCompanyName,
                    Freight = order.Freight,
                    ShipName = order.ShipName,
                    ShipAddress = order.ShipAddress,
                    ShipCity = order.ShipCity,
                    ShipRegion = order.ShipRegion,
                    ShipPostalCode = order.ShipPostalCode,
                    ShipCountry = order.ShipCountry,
                    OrderDetails = order.OrderDetails.Select(detail => new OrderDetailPageViewModel
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.ProductName,
                        UnitPrice = detail.UnitPrice,
                        Quantity = detail.Quantity,
                        Discount = detail.Discount
                    }).ToList()
                }).ToList(),
                TotalCount = dto.TotalCount,
                PageSize = dto.PageSize,
                PageNumber = dto.PageNumber
            };
        }
        #endregion
    }
}
