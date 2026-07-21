using ELibraryAPI.API.Extensions;
using ELibraryAPI.API.Middlewares;
using ELibraryAPI.Application;
using ELibraryAPI.Application.Options;
using ELibraryAPI.Infrastructure;
using ELibraryAPI.Infrastructure.Options;
using ELibraryAPI.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("ELibraryAPI is starting up...");

    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId());

    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true);

    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddPersistanceServices(builder.Environment, builder.Configuration);

    builder.Services.AddResponseCompression(opts =>
    {
        opts.EnableForHttps = true;
        opts.Providers.Add<BrotliCompressionProvider>();
        opts.Providers.Add<GzipCompressionProvider>();
    });

    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

    string swaggerRoute = builder.Configuration["Swagger:RoutePrefix"]?.Trim('/') ?? "swagger";

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(opt =>
    {
        opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Elibrary API", Version = "v1" });
        opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Tokeni bura daxil edin (Bearer sözünə ehtiyac yoxdur)"
        });
        opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] { }
            }
        });
    });

    builder.Services.AddOptions<JwtOptions>().Bind(builder.Configuration.GetSection(JwtOptions.SectionName)).ValidateDataAnnotations().ValidateOnStart();
    builder.Services.AddOptions<SmtpOptions>().Bind(builder.Configuration.GetSection(SmtpOptions.SectionName)).ValidateDataAnnotations().ValidateOnStart();
    builder.Services.AddOptions<SeedOptions>().Bind(builder.Configuration.GetSection(SeedOptions.SectionName)).ValidateDataAnnotations().ValidateOnStart();

    var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
        ?? throw new InvalidOperationException("JwtOptions section is missing.");

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = jwtOptions.Audience,
            ValidIssuer = jwtOptions.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

    builder.Services.AddAuthorization();

    builder.Services.AddRateLimiter(options =>
    {
        options.AddFixedWindowLimiter("auth", opt =>
        {
            opt.PermitLimit = 5;
            opt.Window = TimeSpan.FromMinutes(1);
            opt.QueueLimit = 0;
        });
    });

    var app = builder.Build();

    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            string targetRoute = string.IsNullOrEmpty(swaggerRoute) ? "/swagger" : $"/{swaggerRoute}";
            context.Response.Redirect(targetRoute);
            return; 
        }
        await next();
    });

    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();
    app.UseResponseCompression();

    app.UseSwagger(c =>
    {
        c.RouteTemplate = swaggerRoute + "/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        string endpoint = string.IsNullOrEmpty(swaggerRoute) ? "/v1/swagger.json" : $"/{swaggerRoute}/v1/swagger.json";
        c.SwaggerEndpoint(endpoint, "Elibrary API v1");
        c.RoutePrefix = swaggerRoute;
    });

    await app.SeedDataAsync();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = async (context, report) =>
        {
            context.Response.ContentType = "application/json";
            var response = new
            {
                Status = report.Status.ToString(),
                Duration = report.TotalDuration,
                Checks = report.Entries.Select(e => new
                {
                    Component = e.Key,
                    Status = e.Value.Status.ToString(),
                    Description = e.Value.Description,
                    Duration = e.Value.Duration
                })
            };
            await context.Response.WriteAsJsonAsync(response);
        }
    });

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly.");
}
finally
{
    Log.CloseAndFlush();
}