using API.BasicPolly.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.BasicPolly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Message2Controller : ControllerBase
    {
        private IMessageServiceV2 _messageService;

        public Message2Controller(IMessageServiceV2 messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("hello")]
        public async Task<IActionResult> GetHello()
        {
            var result = await _messageService.GetHelloMessage();
            return Ok(result);
        }

        [HttpGet("goodbye")]
        public async Task<IActionResult> GetGoodbye()
        {
            var result = await _messageService.GetGoodbyeMessage();
            return Ok(result);
        }
    }
}
