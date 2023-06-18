using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using webapi.Dao;
using webapi.Dao.Abstract;
using webapi.Dao.Implementations;
using webapi.Mappers.Abstract;
using webapi.Mappers.Implementations;
using webapi.Services.Abstract;
using webapi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

#region DI

#region Scoped

builder.Services.AddScoped<ITagsDao, TagsDao>();
builder.Services.AddScoped<ITextsDao, TextsDao>();

builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<ITextsService, TextsService>();

#endregion

#region Singletons

builder.Services.AddSingleton<ITagsMapper, TagsMapper>();
builder.Services.AddSingleton<ITextsSectionsVariantsMapper, TextsSectionsVariantsMapper>();
builder.Services.AddSingleton<ITextsSectionsMapper, TextsSectionsMapper>();
builder.Services.AddSingleton<ITextsMapper, TextsMapper>();

#endregion

#endregion

builder.Services.AddControllers();

#region Compression
    // Compression
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = false; // Do not turn on, security risk: https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-6.0
        options.Providers.Add<BrotliCompressionProvider>(); // Brotli is widespread
        options.Providers.Add<GzipCompressionProvider>(); // GZIP as fallback
    });
            
    builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.SmallestSize;
    });

    builder.Services.Configure<GzipCompressionProviderOptions>(options =>
    {
        options.Level = CompressionLevel.SmallestSize;
    });
#endregion

#region CORS
    // CORS
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy
        (
            policy =>
            {
                policy.WithOrigins
                    (
                        "http://localhost:8080",
                        "https://arkumida.furtails.pw"
                    );
            }
        );
    });
#endregion

#region  DB Contexts

    // Main
    builder.Services.AddDbContext<MainDbContext>
    (
        options
            =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MainConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)), ServiceLifetime.Transient
    );

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
