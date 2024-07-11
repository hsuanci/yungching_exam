using Microsoft.AspNetCore.Mvc;
using YungChingExam.Service.interfaces;
using YungChingExam.ViewModel;

namespace YungChingExam.Controllers
{
    /// <summary>
    /// Share Relate API
    /// </summary>
    [ApiController]
    public class ShareController : BaseAPIController
    {
        private readonly ILogger<ShareController> _logger;
        private readonly IShareService _shareService;

        public ShareController(
            ILogger<ShareController> logger,
            IShareService shareService)
        {
            _logger = logger;
            _shareService = shareService;
        }

        /// <summary>
        /// Get Shipper List
        /// </summary>
        /// <param name="discontinued"></param>
        /// <returns></returns>
        [HttpGet("Shippers")]
        [ProducesResponseType(typeof(List<ShipperViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetShipperList()
        {
            var result = await _shareService.GetShipperListAsync();

            if (!result.Any())
            {
                return NotFound();
            }

            var shipperViewModels = result.Select(x => new ShipperViewModel
            {
                ShipperId = x.ShipperId,
                CompanyName = x.CompanyName,
                Phone = x.Phone
            });

            return Ok(shipperViewModels);
        }
    }
}
