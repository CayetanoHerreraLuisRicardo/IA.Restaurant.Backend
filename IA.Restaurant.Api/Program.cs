using IA.Restaurant.Data;
using IA.Restaurant.Data.Helpers;
using IA.Restaurant.Logic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// get configuration from appsetings.json
var config = builder.Configuration;
// get provider (SQL SERVER)
string baseDeDatos = config.GetSection("Settings:Provider").Value;
// get connectionString (SQL SERVER)
string smConnectionString = config.GetConnection(baseDeDatos);
string timeout = config.GetSection("Settings:CommandTimeOut").Value;
builder.Services.AddDbContext<RestaurantContext>(options =>
          options.UseDatabase(smConnectionString, baseDeDatos,
         timeout == null ? 60 : Convert.ToInt32(timeout)), ServiceLifetime.Transient);
// add CORS policy
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        });
});
//add scopes IUnitOfWork, IGenericCrudLogic, IOrderLogic, RestaurantProfile
builder.Services.AddScopes();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();
app.UseCors(builder =>
        builder.SetIsOriginAllowed(host => true)
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials());
app.Run();
