using CardsApi.Mapper;
using CardsApi.Model.Requests;
using CardsApi.Providers;
using CardsApi.Repositories;
using CardsApi.Services;
using CardsApi.Utils;
using CardsApi.Validators;
using CorrelationId;
using CorrelationId.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IStreamReaderProvider, StreamReaderProvider>();
builder.Services.AddScoped<ICsvImporter, CsvImporter>();
builder.Services.AddScoped<ICsvActionRulesParser, CsvActionRulesParser>();

builder.Services.AddSingleton<IActionRulesRepository, InMemoryActionRulesRepository>();
builder.Services.AddSingleton<ICardProvider, CardProvider>();

builder.Services.AddScoped<IActionsProvider, ActionsProvider>();
builder.Services.AddScoped<IActionEligibilityService, ActionEligibilityService>();

builder.Services.AddTransient<AbstractValidator<CardActionsRequest>, CardActionsRequestValidator>();

builder.Services.AddValidatorsFromAssemblyContaining<CardActionsRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDefaultCorrelationId(options =>
{
    options.AddToLoggingScope = true;
    options.EnforceHeader = false;
    options.RequestHeader = "correlation-id";
    options.ResponseHeader = "correlation-id";
});

builder.Services.AddAutoMapper(typeof(ApiMapperProfile));

var app = builder.Build();

// Action rules import
using (var scope = app.Services.CreateScope())
{
    var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
    var cancellationToken = lifetime.ApplicationStopping;

    var csvImporter = scope.ServiceProvider.GetRequiredService<ICsvImporter>();
    await csvImporter.ImportCsvAsync("actionRules.csv", cancellationToken);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCorrelationId();

app.UseAuthorization();

app.MapControllers();

app.Run();
