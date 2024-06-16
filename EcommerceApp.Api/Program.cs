using EcommerceApp.Api.ExtensionMethods;
using EcommerceApp.DAL;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EcommerceApp.Api.CustomFilters;
using EcommerceApp.Api.HATEOAS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddLogger();
builder.Services.AddRepository();
builder.Services.AddAutoMapper(typeof(Program));

NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson().Services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>().First();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true; // return 406 status code if clients negotiate for media type the server does not support
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());

    // config.Filters.Add(); // add global action filters
})
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters()
    .AddCSVFormatter()
    .AddApplicationPart(typeof(Program).Assembly);

// Add Custom Media Type
builder.Services.AddCustomMediaTypes();

// Add Filters
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();

// 
builder.Services.AddScoped<ILinkService, LinkService>();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCustomExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();