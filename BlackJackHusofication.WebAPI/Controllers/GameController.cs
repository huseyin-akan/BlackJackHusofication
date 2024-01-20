using BlackJackHusofication.Business.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackHusofication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController(BjSimulationManager gameManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> StartGame([FromQuery] int roundCount)
        {
            await gameManager.StartNewGame();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PlayRounds([FromQuery] int roundCount)
        {
            await gameManager.PlayRounds(roundCount);
            return Ok();
        }
    }
}
