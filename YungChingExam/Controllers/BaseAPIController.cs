using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YungChingExam.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public abstract class BaseAPIController : ControllerBase
    {
    }
}
