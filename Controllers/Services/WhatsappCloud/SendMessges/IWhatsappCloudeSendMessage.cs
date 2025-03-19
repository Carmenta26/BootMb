namespace WhatsappNetApi.Controllers.Services.WhatsappCloud.SendMessges
{
    public interface IWhatsappCloudeSendMessage
    {
        Task<bool> Excecute(object model);
    }
}
