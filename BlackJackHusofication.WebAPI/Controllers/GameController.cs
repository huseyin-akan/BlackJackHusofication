using BlackJackHusofication.Business.Managers;
using Microsoft.AspNetCore.Mvc;

namespace BlackJackHusofication.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController(BjRoomManager roomManager) : ControllerBase
{
    [HttpGet("GetAllRooms")]
    public IActionResult GetAllRooms()
    {
        return Ok(roomManager.GetRooms() );
    }
}