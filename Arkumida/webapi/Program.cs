using webapi.Services.Abstract;
using webapi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

#region Scoped

builder.Services.AddScoped<ITagsService, TagsService>();

#endregion

builder.Services.AddControllers();

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
