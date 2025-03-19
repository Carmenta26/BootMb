using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;


namespace WhatsappNetApi.Controllers.Services.WhatsappCloud.SendMessges
{
    public class WhatsappCloudeSendMessage : IWhatsappCloudeSendMessage
    {
        public async Task<bool> Excecute(object model) 
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));

            using (var content = new ByteArrayContent(byteData))
            {
                string endpoint = "https://graph.facebook.com";
                string phoneNumberId = "535973906274084";
                string accessToken = "EAAIGzGLqZAE8BO7Vs4UD37EDvEyngq8LIjga4KGj7tcUkd56CmijkZCS2sjX0GCh1qAmI9wlGBHruaC8Mpz4FqhIOtyCypUakchH4MCZBPxegZB6hYBlNldHiM4ZA7zQGnB1ZCaYhZBuVwm0C6wiptFvk1mUvkGZAXulqJuVqbd0XkxxNGY5jJsTzrlmeAXMb5LZBjAZDZD";
                string uri = $"{endpoint}/v22.0/{phoneNumberId}messages";

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode) 
                {
                    return true; 
                }
                return false;
            }

        }
    }
}
