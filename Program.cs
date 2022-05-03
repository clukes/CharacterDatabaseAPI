using System.Reflection;
using Microsoft.AspNetCore.Mvc;

using CharacterDatabaseAPI.WebScraper;
using CharacterDatabaseAPI.Models;
using CharacterDatabaseAPI.Services;
using CharacterDatabaseAPI.Controllers;

// CollectionService<T> MakeService<T>(string collectionName, IServiceProvider serviceProvider) where T : IDocumentModel {
//     var provider = serviceProvider.GetRequiredService<MongoCollectionProvider>();
//     return new CollectionService<T>(collectionName, provider);
// }

Type[] entityTypes = new[] { 
    typeof(CharacterCollection), 
    typeof(Character), 
    typeof(CategoryValue) 
    };
TypeInfo[] closedControllerTypes = entityTypes
    .Select(et => typeof(GenericController<>).MakeGenericType(et))
    .Select(cct => cct.GetTypeInfo())
    .ToArray();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc()
    .ConfigureApplicationPartManager(apm =>
        apm.ApplicationParts.Add(new GenericControllerApplicationPart(closedControllerTypes)));

builder.Services.Configure<CharacterDatabaseSettings>(
    builder.Configuration.GetSection("CharacterDatabase"));

builder.Services.AddSingleton<MongoCollectionProvider>();

// Add services to the container.
// builder.Services.AddSingleton(sp => MakeService<Character>("StarWarsCharacters", sp));
// builder.Services.AddSingleton(sp => MakeService<CharacterCollection>("CharacterCollections", sp));
// builder.Services.AddSingleton(sp => MakeService<CategoryValue>("Species", sp));
// builder.Services.AddSingleton(sp => MakeService<CategoryValue>("Category", sp));

builder.Services.AddControllers().AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        
builder.Configuration.AddEnvironmentVariables();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
Console.WriteLine(string.Join(",", app.Services.GetServices<object>().Select(x => x.GetType())));
WebScraperProgram.ScraperDBSave(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

List<string> controllerNames = new List<string>();
Assembly.GetCallingAssembly().GetTypes().Where(
            type => type.IsSubclassOf(typeof(ControllerBase))).ToList().ForEach(
            type => controllerNames.Add(type.Name));
        Console.WriteLine(string.Join(",",controllerNames));

app.Run();
