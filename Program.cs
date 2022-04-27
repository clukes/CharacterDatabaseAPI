using CharacterDatabaseAPI.WebScraper;

using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<CharacterDatabaseSettings>(
    builder.Configuration.GetSection("CharacterDatabase"));

builder.Services.AddSingleton<CharacterCollectionService>();

builder.Configuration.AddEnvironmentVariables();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();
// WebScraperProgram.ScraperRetrieve();
WebScraperProgram.ScraperDBSave(app.Services.GetService<CharacterCollectionService>()!);

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
//     // app.UseSwagger();
//     // app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers();

// app.Run();

