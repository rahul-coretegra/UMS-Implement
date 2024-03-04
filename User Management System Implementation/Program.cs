using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.Repository;
using User_Management_System_Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using User_Management_System_Implementation.AuthorizationResources;
using Microsoft.AspNetCore.Identity.UI.Services;
using User_Management_System_Implementation.OutlookSmtpConfigurations;
using User_Management_System_Implementation.TwilioModule;
using User_Management_System_Implementation.AwsSnsConfigurations;
using User_Management_System_Implementation.TwilioConfigurations;
using User_Management_System_Implementation.ElasticMailConfigurations;
using User_Management_System_Implementation.PostMarkConfigurations;
using User_Management_System_Implementation.NexmoConfigurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddScoped<IDefaultConfigurationsRepository, DefaultConfigurationsRepository>();

var configurationService = builder.Services.BuildServiceProvider().GetService<IDefaultConfigurationsRepository>();

configurationService.ConfigureService(builder.Services);

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IOutlookSmtpRepository, OutlookSmtpRepository>();

builder.Services.AddScoped<IPostMarkRepository, PostMarkRepository>();
builder.Services.AddScoped<ITwilioRepository, TwilioRepository>();

builder.Services.AddScoped<IAwsRepository, AwsRepository>();

builder.Services.AddScoped<IElasticMailRepository, ElasticMailRepository>();
builder.Services.AddScoped<INexmoRepository, NexmoRepository>();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
    ConfigureSwaggerOptions>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var appSettings = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettings);
var appSetting = appSettings.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSetting.Secret);

builder.Services.ConfigureJwtAuthentication(key);
builder.Services.ConfigureCustomAuthorization();

builder.Services.AddScoped<IAuthorizationHandler, SupremeLevelAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, AuthorityLevelAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IntermediateLevelAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, SecondaryLevelAuthorizationHandler>();

builder.Services.AddScoped<IAuthorizationHandler, IsAccssAuthorizationHandler>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
