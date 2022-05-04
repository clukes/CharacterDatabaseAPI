using CharacterDatabaseAPI.Services;
using Amazon.Extensions.NETCore.Setup;

namespace CharacterDatabaseAPI;
public class CharacterDatabaseAPIProgram
{
    public static WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Get the AWS profile information from configuration providers
        AWSOptions awsOptions = builder.Configuration.GetAWSOptions();

        // Configure AWS service clients to use these credentials
        builder.Services.AddDefaultAWSOptions(awsOptions);

        builder.Services.AddScoped<ICharacterService, CharacterService>();

        // Add services to the container.

        builder.Services.AddControllers().AddJsonOptions(
                options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        builder.Configuration.AddEnvironmentVariables();


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        return builder;
    }

    public static WebApplication BuildApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

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
        return app;
    }

    public static void RunApp(WebApplication app) => app.Run();

    public static void Main(string[] args)
    {
        var builder = CreateBuilder(args);
        var app = BuildApp(builder);
        RunApp(app);
    }
}
