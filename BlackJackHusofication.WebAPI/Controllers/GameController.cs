using BlackJackHusofication.Business.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackHusofication.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController(BjSimulationManager _gameManager, BjRoomManager _roomManager) : ControllerBase
{
    [HttpGet("StartGame")]
    public async Task<IActionResult> StartGame([FromQuery] int roundCount)
    {
        await _gameManager.StartNewGame();
        return Ok();
    }

    [HttpPost("PlayRounds")]
    public async Task<IActionResult> PlayRounds([FromQuery] int roundCount)
    {
        await _gameManager.PlayRounds(roundCount);
        return Ok();
    }

    [HttpPost("CreateRoom")] //TODO-HUS buna gerek olmama ihtimali yuksek
    public IActionResult CreateRoom([FromBody] string roomName, int roomId)
    {
        _roomManager.CreateRoom(roomName, roomId);
        return Ok();
    }

    [HttpGet("GetAllRooms")]
    public IActionResult GetAllRooms()
    {
        return Ok(_roomManager.GetRooms() );
    }
}