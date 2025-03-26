using WhatsappNetApi.Controllers.Services.WhatsappCloud.SendMessges;
using WhatsappNetApi.Util;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<IWhatsappCloudeSendMessage, WhatsappCloudeSendMessage>();
builder.Services.AddSingleton<IUtil, Utils>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
