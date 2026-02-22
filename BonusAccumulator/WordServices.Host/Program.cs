using CardboxDataLayer;
using WordServices.Host.Configuration;
using WordServices.Host.Endpoints;
using WordServices.Host.Middleware;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddWordServicesForWeb(builder.Configuration);
builder.Services.AddCardboxDataLayer(builder.Configuration);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("WordServicesApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7200");
});

builder.Services.AddScoped(sp =>
{
    IHttpClientFactory factory = sp.GetRequiredService<IHttpClientFactory>();
    return factory.CreateClient("WordServicesApi");
});

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("DevCors", policy =>
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });
}

WebApplication app = builder.Build();

// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
//     app.UseCors("DevCors");
// }

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapHealthEndpoints();
app.MapAnagramEndpoints();
app.MapQuizEndpoints();
app.MapCardboxEndpoints();

app.MapRazorComponents<WordServices.Host.Components.App>()
    .AddInteractiveServerRenderMode();

app.Run();
