using EcommerceApp.Api.ExtensionMethods;
using EcommerceApp.DAL;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EcommerceApp.Api.CustomFilters;
using Serilog;
using EcommerceApp.Api.Formatters;

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
    .AddNewtonsoftJson()
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
app.UseAuthorization();

app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseOutputCache();

app.MapControllers();

app.Run();
