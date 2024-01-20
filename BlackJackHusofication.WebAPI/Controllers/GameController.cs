using BlackJackHusofication.Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackHusofication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly GameManager _gameManager;

        public GameController(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        [HttpGet]
        public async Task<IActionResult> StartGame([FromQuery] int roundCount)
        {
            await _gameManager.StartNewGame();
            await _gameManager.PlayRounds(roundCount);
            return Ok();
        }
    }
}
