using FinanceApp.Core.Abstractions;
using FinanceApp.Data.DataContext;
using FinanceApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddDbContext<FinanceAppDbContext>(
        options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(nameof(FinanceAppDbContext)));
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
