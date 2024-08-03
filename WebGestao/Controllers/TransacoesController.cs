using Microsoft.AspNetCore.Mvc;

namespace WebGestao.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class TransacoesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
