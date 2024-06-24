using EcommerceApp.Api.ExtensionMethods;
using EcommerceApp.DAL;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EcommerceApp.Api.CustomFilters;
using Serilog;
using EcommerceApp.Api.Formatters;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddRepository();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true; // return 406 status code if clients negotiate for media type the server does not support

    config.OutputFormatters.Add(new CsvOutputFormatter());

    // config.Filters.Add(); // add global action filters

    config.CacheProfiles.Add("ExpireIn300s", new CacheProfile()
    {
        Duration = 300
    });
})
.AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
})
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
})
.AddXmlDataContractSerializerFormatters()
.AddApplicationPart(typeof(Program).Assembly);

// Add Filters
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();


builder.Services.AddHttpContextAccessor();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("X-Pagination");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

// Api Versioning
builder.Services.AddApiVersioningConfiguration();

// Caching
builder.Services.AddCaching();

// Rate Limiting
builder.Services.AddRateLimiting();

// Entity Framework Core Identity
builder.Services.AddIdentity();
// this must be placed after AddIdentity
builder.Services.AddJWTAuthentication(builder.Configuration);

var app = builder.Build();

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter(IServiceProvider serviceProvider)
{
    var mvcOptions = serviceProvider.GetRequiredService<IOptions<MvcOptions>>().Value;
    var formatter = mvcOptions.InputFormatters.OfType<NewtonsoftJsonPatchInputFormatter>().FirstOrDefault();
    if (formatter == null)
    {
        throw new InvalidOperationException("NewtonsoftJsonPatchInputFormatter is not registered.");
    }
    return formatter;
}

var jsonPatchInputFormatter = GetJsonPatchInputFormatter(app.Services);
app.Services.GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters.Insert(0, jsonPatchInputFormatter);


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCustomExceptionHandler();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();
app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseOutputCache();

app.MapControllers();



app.Run();
