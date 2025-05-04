using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WhatsappNetApi.Controllers.Models.WhatsAppCloud;
using WhatsappNetApi.Controllers.Services.WhatsappCloud.SendMessges;
using WhatsappNetApi.Util;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatsappNetApi.Controllers
{
    [ApiController]
    [Route("api/whatsapp")]
    public class WhatsappController : Controller
    {
        private readonly IWhatsappCloudSendMessage _whatsappCloudSendMessage;
        private readonly IUtil _util;
        public WhatsappController(IWhatsappCloudSendMessage whatsappCloudeSendMessage, IUtil util)
        {
            _whatsappCloudSendMessage = whatsappCloudeSendMessage;
            _util = util;
        }

        [HttpGet("test")]
        public async Task<IActionResult> Index()
        {

            var data =
               new
               {
                   messaging_product = "whatsapp",
                   to = "526441687811",
                   type = "text",
                   text = new
                   {
                       body = "Este es un mensaje de prueba"
                   }
               };


            var result = await _whatsappCloudSendMessage.Execute(data);


            return Ok("ok sample");
        }

        [HttpGet]
        public IActionResult VeifyToken()
        {
            string AccessToken = "SSJDKHFJLSDFHDHFKL";

            var token = Request.Query["hub.verify_token"].ToString();
            var challenge = Request.Query["hub.challenge"].ToString();

            if (challenge != null && token != null && token == AccessToken)
            {
                return Ok(challenge);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReceivedMessage([FromBody] WhatsAppCloudModel body)
        {
            try
            {
                var Message = body.Entry[0]?.Changes[0]?.Value?.Messages[0];
                if (Message != null)
                {
                    var userNumber = Message.From.Length == 13?Message.From.Remove(2,1):Message.From;
                    var userText = GetUserText(Message);

                    List<object> listObjectMessage = new List<object>();

                    
                    if (userText.ToUpper().Contains("HOLA"))
                    {
                        var objectMessage = _util.TextMessage("Hola, ¿cómo te puedo ayudar? 😃", userNumber);
                        listObjectMessage.Add(objectMessage);

                        var objectMessage2 = _util.TextMessage("Responderé todas tus preguntas 😃", userNumber);
                        listObjectMessage.Add(objectMessage2);

                        var objectMessage3 = _util.ImageMessage("https://biostoragecloud.blob.core.windows.net/resource-udemy-whatsapp-node/image_whatsapp.png", userNumber);
                        listObjectMessage.Add(objectMessage3);

                    }
                    else if (userText.ToUpper().Contains("GRACIAS") || userText.ToUpper().Contains("AGRADECID"))
                    {
                        var objectMessage = _util.TextMessage("Gracias a ti por escribirme. 😃", userNumber);
                        listObjectMessage.Add(objectMessage);
                    }
                    else if (userText.ToUpper().Contains("ADIOS") || userText.ToUpper().Contains("YA ME VOY"))
                    {
                        var objectMessage = _util.TextMessage("Ve con cuidado. 😃", userNumber);
                        listObjectMessage.Add(objectMessage);
                    }
                    else
                    {
                        var objectMessage = _util.TextMessage("Lo siento, no puedo entenderte. 😔", userNumber);
                        listObjectMessage.Add(objectMessage);
                    }

                    foreach (var item in listObjectMessage)
                    {
                        await _whatsappCloudSendMessage.Execute(item);
                    }
                }

                return Ok("EVENT_RECEIVED");
            }
            catch (Exception ex)
            {
                return Ok("EVENT_RECEIVED");
            }
        }

        private string GetUserText(Message message)
        {
            string TypeMessage = message.Type;

            if (TypeMessage.ToUpper() == "TEXT")
            {
                return message.Text.Body;
            }
            else if (TypeMessage.ToUpper() == "INTERACTIVE")
            {
                string interactiveType = message.Interactive.Type;

                if (interactiveType.ToUpper() == "LIST_REPLY")
                {
                    return message.Interactive.List_Reply.Title;
                }
                else if (interactiveType.ToUpper() == "BUTTON_REPLY")
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