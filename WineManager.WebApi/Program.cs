using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.HttpLogging;
using WineManager.WebApi.Repositories;
using WineManager.DataContext.Sqlite;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args); //Create a new web application builder

// Add services to the container.
builder.Services.AddWineManagerContext(); //Registers the database context to enable acces to the database

//Add a controller for writing the names of supported media types
builder.Services.AddControllers(options =>
{
    Console.WriteLine("Default output formatters:");
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;

        if (mediaFormatter == null)
        {
            Console.WriteLine($"{formatter.GetType().Name}");
        }

        else
        {
            Console.WriteLine("{0}, Media types: {1}",
                arg0: mediaFormatter.GetType().Name,
                arg1: string.Join(",",
                mediaFormatter.SupportedMediaTypes));
        }
    }
})
    .AddXmlDataContractSerializerFormatters()
    .AddXmlSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMemoryCache>(new MemoryCache(new MemoryCacheOptions())); //Enabling caching data in memory
builder.Services.AddScoped<IWineRepository, WineRepository>(); //Register the wine repository
builder.Services.AddScoped<IProducerRepository, ProducerRepository>(); //Register the producer repository

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", builder =>
        builder.WithOrigins("https://localhost:5141;http://localhost:5140") // MVC ports
               .AllowAnyMethod()
               .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            "Wine Manager Service API Version 1");

        c.SupportedSubmitMethods(new[]
        {
            SubmitMethod.Get, SubmitMethod.Post,
            SubmitMethod.Put, SubmitMethod.Delete
        });
    });
}

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseHttpLogging();

app.UseAuthorization();

app.MapControllers();

app.Run();
