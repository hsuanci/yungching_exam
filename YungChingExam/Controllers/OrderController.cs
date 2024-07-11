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
        /// Create Order
        /// </summary>
        /// <param name="vm">OrderCreateViewModel</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PostOrder([FromBody] OrderCreateViewModel vm)
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
    }
}
