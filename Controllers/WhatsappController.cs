using Microsoft.AspNetCore.Mvc;
using WhatsappNetApi.Controllers.Models.WhatsAppCloud;

namespace WhatsappNetApi.Controllers
{

    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : Controller
    {
        [HttpGet("test")]
        public IActionResult Index()
        {
            return Ok("Un ejemplo"); 
        }

        [HttpGet]
        public IActionResult VeifyToken()
        {
            String AccessToken = "SSJDKHFJLSDFHDHFKL";
            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if (challenge != null && token != null && token == AccessToken)
            {

                return Ok(challenge);
            }
            else {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResivedMesagge([FromBody] WhatsAppCloudModel body) 
        {
            try
            {
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages?[0];
                if (Message != null) {
                    var userNumber = Message.From;
                    var UserText = GetUserText(Message);
                }
                

                return Ok("EVENT_RECEIVED");
            }
            catch (Exception ex)
            {
                return Ok("EVENT_RECEIVED");
            }
        }

        private String GetUserText(Message message)
        {
            string TypeMessage = message.Type;

            if (TypeMessage.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            }

            else if (TypeMessage.ToUpper() == "INTERACTIVE")
            {
                string InteractiveType = message.Interactive.Type;
                if (InteractiveType.ToUpper() == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                }
                else if (InteractiveType.ToUpper() == "BUTTON_REPLY")
                {
                    return message.Interactive.Button_Reply.Title;
                }
                else
                {
                    return string.Empty;
                }
            }

            else
            {
                return string.Empty;
            }
        }
    }
}
