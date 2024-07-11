using Microsoft.AspNetCore.Mvc;
using YungChingExam.Service.interfaces;
using YungChingExam.ViewModel;

namespace YungChingExam.Controllers
{
    /// <summary>
    /// Product Relate API
    /// </summary>
    [ApiController]
    public class ProductController : BaseAPIController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(
            ILogger<ProductController> logger,
            IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        /// <summary>
        /// Get Product List
        /// </summary>
        /// <param name="discontinued"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetProducts([FromQuery] bool discontinued = false)
        {
            var result = await _productService.GetProductListAsync(discontinued);

            if (!result.Any())
            {
                return NotFound();
            }

            var productViewModels = result.Select(x => new ProductViewModel
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                QuantityPerUnit = x.QuantityPerUnit,
                UnitPrice = x.UnitPrice,
                UnitsInStock = x.UnitsInStock,
                UnitsOnOrder = x.UnitsOnOrder
            });

            return Ok(productViewModels);
        }
    }
}
