using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Business.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy => 
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials()
              .SetIsOriginAllowed(origin => true))
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddScoped<GameManager>();
builder.Services.AddScoped<ILogManager, LoggerManager>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseEndpoints(endpoints => { endpoints.MapHub<BlackJackHub>("/blackjackhub"); });
app.MapHub<BlackJackHub>("/blackjackhub");

app.Run();