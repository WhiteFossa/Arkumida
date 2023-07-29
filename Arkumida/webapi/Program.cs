using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using webapi.Constants;
using webapi.Dao;
using webapi.Dao.Abstract;
using webapi.Dao.Implementations;
using webapi.Mappers.Abstract;
using webapi.Mappers.Implementations;
using webapi.Models.Identity;
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

builder.Services.AddScoped<IFilesDao, FilesDao>();
builder.Services.AddScoped<IFilesService, FilesService>();

builder.Services.AddScoped<IAccountsService, AccountsService>();

#endregion

#region Singletons

builder.Services.AddSingleton<ITagsMapper, TagsMapper>();
builder.Services.AddSingleton<ITextsSectionsVariantsMapper, TextsSectionsVariantsMapper>();
builder.Services.AddSingleton<ITextsSectionsMapper, TextsSectionsMapper>();
builder.Services.AddSingleton<ITextsMapper, TextsMapper>();
builder.Services.AddSingleton<IFilesMapper, FilesMapper>();
builder.Services.AddSingleton<ITextFilesMapper, TextFilesMapper>();
builder.Services.AddSingleton<ITextsPagesMapper, TextsPagesMapper>();
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

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

    // Security
    builder.Services.AddDbContext<SecurityDbContext>
    (
        options
            =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MainConnection")), ServiceLifetime.Transient
    );

    // Main
    builder.Services.AddDbContext<MainDbContext>
    (
        options
            =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("MainConnection"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)), ServiceLifetime.Transient
    );

#endregion

#region Identity framework

    // Identity
    builder.Services.AddIdentity<User, IdentityRole>()  
        .AddEntityFrameworkStores<SecurityDbContext>()  
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 4;
                
        // User settings
        options.User.RequireUniqueEmail = true;
    });

    // Adding Authentication  
    builder.Services.AddAuthentication(options =>  
        {  
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  
        })  
      
        // Adding Jwt Bearer  
        .AddJwtBearer(options =>  
        {  
            options.SaveToken = true;  
            options.RequireHttpsMetadata = false;  
            options.TokenValidationParameters = new TokenValidationParameters()  
            {  
                ValidateIssuer = true,  
                ValidateAudience = true,  
                ValidAudience = builder.Configuration[GlobalConstants.JwtValidAudienceSettingName],  
                ValidIssuer = builder.Configuration[GlobalConstants.JwtValidIssuerSettingName],  
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[GlobalConstants.JwtSecretSettingName]))  
            };  
        }
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

app.UseAuthentication();

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
