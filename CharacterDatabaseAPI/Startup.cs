using CharacterDatabaseAPI.Services;
using Amazon.Extensions.NETCore.Setup;

namespace CharacterDatabaseAPI;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    private readonly IConfiguration configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        AWSOptions awsOptions = configuration.GetAWSOptions();

        services.AddDefaultAWSOptions(awsOptions);

        services.AddScoped<ICharacterService, CharacterService>();

        // Add services to the container.
        services.AddControllers().AddJsonOptions(
                options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(opt =>
        {
            opt.MapControllers();
        });
    }
}
