using BlackJackHusofication.Business.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackHusofication.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController(BjRoomManager roomManager) : ControllerBase
{
    [HttpPost("CreateRoom")] //TODO-HUS buna gerek olmama ihtimali yuksek
    public IActionResult CreateRoom([FromBody] string roomName, int roomId)
    {
        roomManager.CreateRoom(roomName, roomId);
        return Ok();
    }

    [HttpGet("GetAllRooms")]
    public IActionResult GetAllRooms()
    {
        return Ok(roomManager.GetRooms() );
    }
}