using Microsoft.AspNetCore.Mvc;
using YungChingExam.Helpers;
using YungChingExam.Service.interfaces;
using YungChingExam.ViewModel;

namespace YungChingExam.Controllers
{
    /// <summary>
    /// Share Relate API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ShareController : BaseController
    {
        private readonly ILogger<ShareController> _logger;
        private readonly JWTHelper _jWTHelper;
        private readonly IShareService _shareService;

        public ShareController(
            ILogger<ShareController> logger,
            JWTHelper jWTHelper,
            IShareService shareService)
        {
            _logger = logger;
            _jWTHelper = jWTHelper;
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
            var result = await _shareService.GetShipperList();

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
