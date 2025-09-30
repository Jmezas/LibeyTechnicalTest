using LibeyTechnicalTestAPI.Middleware;
using LibeyTechnicalTestDomain.EFCore;
using LibeyTechnicalTestDomain.EFCore.Mappers;
using Microsoft.EntityFrameworkCore;
IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").AddEnvironmentVariables().Build();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("localCors",
    builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();
DIExtensions.AddConfigurations(builder.Services);
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(config.GetConnectionString("LibeyTechnicalTest"),
    sqlServerOptionsAction: builder =>
    {
        builder.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(5), errorNumbersToAdd: null);
    });
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ResponseMiddleware>();
app.UseCors("localCors");
app.UseRouting();
app.MapControllers();
app.Run();