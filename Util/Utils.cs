namespace WhatsappNetApi.Util
{
    public class Utils : IUtil
    {

        public object TextMessage(string message , string number)
        {
            
            return new
            {
                      messaging_product = "whatsapp",
                      to = number,
                      type = "text",
                      text = new
                      {
                          body = message
                      } 
           };
        }
    }
}
