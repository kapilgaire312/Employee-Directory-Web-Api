using EmployeeDirWeb.Data;
using EmployeeDirWeb.Exceptions;
using EmployeeDirWeb.Models;
using EmployeeDirWeb.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//required by exception handling middleware to generate error response
builder.Services.AddProblemDetails();

//add the global exceoption handlers as DI
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

//Add database context for dependency injection
builder.Services.AddDbContext<EmployeeDirectoryContext>(options =>
    options.UseNpgsql(builder.Configuration["DB_CONN_STRING"])
);

builder.Services.AddSingleton<List<EmployeeInfo>>();

// Inject the InMemory dependency to use in memory
// builder.Services.AddSingleton<IEmployeeDirectory, EmployeeDirectoryInMemory>();

//inject the EmployeeDirectoryDb to use postgres database
builder.Services.AddScoped<IEmployeeDirectory, EmployeeDirectoryDb>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

//will send the excepstatus tion as proper response instead of showing stack trace to user.
app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
