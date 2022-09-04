using TestAPI.Services;
using Hazelcast;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var options = new HazelcastOptions();
options.ClusterName = "dev";
options.ClientName = "net-client";
options.Networking.Addresses.Add("localhost:5701");

builder.Services.AddSingleton<IHazelcastService<string, int>, HazelcastService<string, int>>
    (service => new HazelcastService<string, int>(options, "login_attempts"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
