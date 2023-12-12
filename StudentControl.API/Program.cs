using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using System.Text.Json.Serialization;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
builder.Services.AddMvc( options => options.SuppressAsyncSuffixInActionNames = false);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StudentControl.Infrastructure.Context>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("TestDB")));



//builder.Services.AddAutoMapper(typeof(StudentControl.DTO.MapperProfile));

builder.Services.AddAutoMapper(typeof(StudentControl.DTO.MapperProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors( x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
