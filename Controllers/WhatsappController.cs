﻿using Microsoft.AspNetCore.Mvc;
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
        private readonly IWhatsappCloudeSendMessage _whatsappCloudeSendMessage;
        private readonly IUtil _util;
        public WhatsappController(IWhatsappCloudeSendMessage whatsappCloudeSendMessage, IUtil util)
        {
            _whatsappCloudeSendMessage = whatsappCloudeSendMessage;
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


            var result = await _whatsappCloudeSendMessage.Excecute(data);
            return Ok("Un ejemplo"); 
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
                if (Message != null)
                {
                    var userNumber = Message.From;
                    var UserText = GetUserText(Message);

                    object objectMessage = null;

                    switch (UserText.ToUpper())
                    {
                        case "TEXT":
                            objectMessage = _util.TextMessage("Este es un ejemplo de texto", userNumber);
                                break;

                        default:
                            objectMessage = _util.TextMessage("Lo siento no puedo entenderte", userNumber);
                            break;
                    }

                    if (objectMessage != null)
                    {
                        await _whatsappCloudeSendMessage.Excecute(objectMessage);
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
