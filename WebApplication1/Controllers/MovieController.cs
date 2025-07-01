using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : Controller
    {
        [HttpPost]
        public ActionResult<MovieInfo> AddMovie([FromBody] MovieInfo newMovie)
        {

            return Ok();
        }
    }
}
