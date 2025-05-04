namespace WhatsappNetApi.Controllers.Services.WhatsappCloud.SendMessges
{
    public interface IWhatsappCloudSendMessage
    {
        Task<bool> Execute(object model);
    }
}
