using BlackJackHusofication.Business;
using BlackJackHusofication.Business.Managers;
using BlackJackHusofication.Business.Services.Abstracts;
using BlackJackHusofication.Business.Services.Concretes;
using BlackJackHusofication.Business.SignalR;
using BlackJackHusofication.WebAPI.MinimalEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .AllowCredentials()
                 .SetIsOriginAllowed(origin => true)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddSingleton<BjSimulationManager>();
builder.Services.AddSingleton<BjRoomManager>();
builder.Services.AddSingleton<IGameLogger, SimulationLogsService>();

BusinessServiceRegistraiton.AddBusinessDependencies(builder.Services);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

MiniamalEndpointRegistration.Register(app); //Minimal APIs

//app.UseEndpoints(endpoints => { endpoints.MapHub<BlackJackHub>("/blackjackhub"); });
app.MapHub<BlackJackGameHub>("/bj-game");
app.MapHub<BlackJackSimulHub>("/bj-simul");

app.MapControllers();

app.Run();