using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController: ControllerBase
    {
        [HttpGet]
        public bool Get()
        {
            return true;
        }
    }
}