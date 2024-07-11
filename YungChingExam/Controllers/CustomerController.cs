using Microsoft.AspNetCore.Mvc;
using YungChingExam.Helpers;
using YungChingExam.Service.interfaces;
using YungChingExam.ViewModel;

namespace YungChingExam.Controllers
{
    /// <summary>
    /// Customer Relate API
    /// </summary>
    [ApiController]
    public class CustomerController : BaseAPIController
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(
            ILogger<CustomerController> logger,
            ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        /// <summary>
        /// Get Customer List
        /// </summary>
        /// <param name="discontinued"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCustomers()
        {
            var result = await _customerService.GetCustomerListAsync();

            if (!result.Any())
            {
                return NotFound();
            }

            var customerViewModels = result
                .Select(x => new CustomerViewModel
                {
                    Fax = x.Fax,
                    ContactName = x.ContactName,
                    CustomerId = x.CustomerId,
                    CompanyName = x.CompanyName,
                    ContactTitle = x.ContactTitle,
                    Address = x.Address,
                    City = x.City,
                    Region = x.Region,
                    PostalCode = x.PostalCode,
                    Country = x.Country,
                    Phone = x.Phone
                });

            return Ok(customerViewModels);
        }
    }
}
