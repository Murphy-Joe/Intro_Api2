using AutoMapper;
using LearningResourcesApi.Data;
using LearningResourcesApi.MappingProfiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(pol =>
    {
        pol.AllowAnyHeader();
        pol.AllowAnyMethod();
        pol.AllowAnyOrigin();
    });
});

builder.Services.AddDbContext<LearningResourcesDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("learning-resources"));
});
//@"server=.\sqlexpress;database=learning_dev;integrated security=true"

var mapperConfig = new MapperConfiguration(pol =>
{
    pol.AddProfile<LearningResourcesProfile>();
});

var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton<IMapper>(mapper);
builder.Services.AddSingleton(mapperConfig);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
