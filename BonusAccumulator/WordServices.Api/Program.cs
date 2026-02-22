using CardboxDataLayer;
using WordServices.Api.Configuration;
using WordServices.Api.Endpoints;
using WordServices.Api.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddWordServicesForWeb(builder.Configuration);
builder.Services.AddCardboxDataLayer(builder.Configuration);

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? ["https://localhost:7300", "http://localhost:5300"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("WebFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader());
});

WebApplication app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseCors("WebFrontend");
app.UseMiddleware<GlobalExceptionMiddleware>();

app.MapHealthEndpoints();
app.MapAnagramEndpoints();
app.MapQuizEndpoints();
app.MapCardboxEndpoints();

app.Run();
