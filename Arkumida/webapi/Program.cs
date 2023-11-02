using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using webapi.Constants;
using webapi.Dao;
using webapi.Dao.Abstract;
using webapi.Dao.Implementations;
using webapi.Dao.Models;
using webapi.Mappers.Abstract;
using webapi.Mappers.Implementations;
using webapi.Models.Email;
using webapi.Models.Settings;
using webapi.OpenSearch.Services.Abstract;
using webapi.OpenSearch.Services.Implementations;
using webapi.Services.Abstract;
using webapi.Services.Abstract.Email;
using webapi.Services.Abstract.Search;
using webapi.Services.Abstract.TextRenderers;
using webapi.Services.Abstract.TextsStatistics;
using webapi.Services.Implementations;
using webapi.Services.Implementations.Email;
using webapi.Services.Implementations.Hosted;
using webapi.Services.Implementations.Search;
using webapi.Services.Implementations.TextRenderers;
using webapi.Services.Implementations.TextsStatitstics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

#region DI

#region Scoped

builder.Services.AddScoped<ITagsDao, TagsDao>();
builder.Services.AddScoped<ITextsDao, TextsDao>();

builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<ITextsService, TextsService>();
builder.Services.AddScoped<ITextUtilsService, TextUtilsService>();

builder.Services.AddScoped<IFilesDao, FilesDao>();
builder.Services.AddScoped<IFilesService, FilesService>();

builder.Services.AddScoped<IAccountsService, AccountsService>();

builder.Services.AddScoped<IPlainTextRenderer, PlainTextRenderer>();
builder.Services.AddScoped<IRawTextRenderer, RawTextRenderer>();

builder.Services.AddScoped<IRenderedTextsDao, RenderedTextsDao>();
builder.Services.AddScoped<ITextsRenderingService, TextsRenderingService>();

builder.Services.AddScoped<IProfilesDao, ProfilesDao>();

builder.Services.AddScoped<IAvatarsDao, AvatarsDao>();

builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IEmailsGeneratorService, EmailsGeneratorService>();

builder.Services.AddScoped<IPrivateMessagesService, PrivateMessagesService>();
builder.Services.AddScoped<IPrivateMessagesDao, PrivateMessagesDao>();

builder.Services.AddScoped<ITextsSearchService, TextsSearchService>();

builder.Services.AddScoped<ITextsStatisticsDao, TextsStatisticsDao>();
builder.Services.AddScoped<ITextsStatisticsService, TextsStatisticsService>();

#endregion

#region Singletons

builder.Services.AddSingleton<ITagsMapper, TagsMapper>();
builder.Services.AddSingleton<ITextsSectionsVariantsMapper, TextsSectionsVariantsMapper>();
builder.Services.AddSingleton<ITextsSectionsMapper, TextsSectionsMapper>();
builder.Services.AddSingleton<ITextsMapper, TextsMapper>();
builder.Services.AddSingleton<IFilesMapper, FilesMapper>();
builder.Services.AddSingleton<ITextFilesMapper, TextFilesMapper>();
builder.Services.AddSingleton<ITextsPagesMapper, TextsPagesMapper>();
builder.Services.AddSingleton<ICreaturesMapper, CreaturesMapper>();
builder.Services.AddSingleton<IRenderedTextsMapper, RenderedTextsMapper>();
builder.Services.AddSingleton<IAvatarsMapper, AvatarsMapper>();
builder.Services.AddSingleton<ICreaturesWithProfilesMapper, CreaturesWithProfilesMapper>();
builder.Services.AddSingleton<IPrivateMessagesMapper, PrivateMessagesMapper>();
builder.Services.AddSingleton<ITextsStatisticsEventsMapper, TextsStatisticsEventsMapper>();

builder.Services.AddSingleton<IArkumidaOpenSearchClient, ArkumidaOpenSearchClient>();

#endregion

#region Hosted

builder.Services.AddHostedService<BuiltInUsersAndRolesCreator>();

#endregion

#endregion

#region Settings

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));
builder.Services.Configure<ImporterUserSettings>(builder.Configuration.GetSection(nameof(ImporterUserSettings)));
builder.Services.Configure<SiteInfoSettings>(builder.Configuration.GetSection(nameof(SiteInfoSettings)));
builder.Services.Configure<OpenSearchSettings>(builder.Configuration.GetSection(nameof(OpenSearchSettings)));

#endregion

builder.Services.AddControllers();

#region Compression
    // Compression
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true; // SECURITY RISK AHEAD! Read this fist before enabling: https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-6.0
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
                policy
                    .WithOrigins
                    (
                        "http://localhost:8080",
                        "https://arkumida.furtails.pw"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
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

#region Identity framework

    // Identity
    builder.Services.AddIdentity<CreatureDbo, IdentityRole<Guid>>()  
        .AddEntityFrameworkStores<MainDbContext>()  
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // User settings
        options.User.AllowedUserNameCharacters = string.Empty; // Any characters is allowed
        
        // Password settings
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequiredUniqueChars = 4;
    });

    // JWT settings
    var jwtSettings = builder
        .Configuration
        .GetSection(nameof(JwtSettings))
        .Get<JwtSettings>();

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
                ValidAudience = jwtSettings.ValidAudience,  
                ValidIssuer = jwtSettings.ValidIssuer,  
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))  
            };  
        }
    );

#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen
(
    sc =>
    {
        sc.SwaggerDoc("v1", new OpenApiInfo() { Title = "Arkumida API", Version = "v1" });
        
        sc.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = @"JWT Authorization token",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        
        sc.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,

                },
                new List<string>()
            }
        });
    }
);

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

app.UseResponseCompression();

app.MapControllers();

app.Run();
