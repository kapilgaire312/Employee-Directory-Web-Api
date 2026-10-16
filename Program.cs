var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//required by exception handling middleware to generate error response
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IEmployeeDirectory, EmployeeDirectoryInMemory>();

var app = builder.Build();

//will send the exception as proper response instead of showing stack trace to user.
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
