using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YungChingExam.Helpers;
using YungChingExam.Service.interfaces;
using YungChingExam.ViewModel;

namespace YungChingExam.Controllers
{
    /// <summary>
    /// Auth Relate API
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : BaseController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly JWTHelper _jWTHelper;
        private readonly IAuthService _authService;

        public AuthController(
            ILogger<OrderController> logger,
            JWTHelper jWTHelper,
            IAuthService authService)
        {
            _logger = logger;
            _jWTHelper = jWTHelper;
            _authService = authService;
        }

        /// <summary>
        /// Login Auth and Get JWT Token
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel vm)
        {
            var result = await _authService.AuthenticateEmployeeAsync(vm.Account, vm.Password);

            if (!result.IsAuthenticated)
            {
                return Unauthorized();
            }

            var token = _jWTHelper.GenerateJwtToken(result.Employee);

            return Ok(new AuthViewModel
            {
                Token = token,
            });
        }
    }
}
