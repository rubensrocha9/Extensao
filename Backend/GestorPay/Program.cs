using GestorPay.Context;
using GestorPay.Helper;
using GestorPay.Helper.Notification;
using GestorPay.Models.Helper;
using GestorPay.Models.Service;
using GestorPay.Models.Service.Interface;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEmployeePositionService, EmployeePositionService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ISpendingManagerService, SpendingManagerService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IEmployeePermissionService, EmployeePermissionService>();
builder.Services.AddScoped<HelperValidation, HelperValidation>();
builder.Services.AddScoped<EmailBody, EmailBody>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestorPayContext>(op => op.UseLazyLoadingProxies().UseSqlServer(connection));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("_AngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:7099")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception != null && exception is CustomException customException)
        {
            context.Response.StatusCode = (int)customException.StatusCode;
            context.Response.ContentType = "application/json";

            var responseData = new
            {
                message = customException.Message,
                statusCode = customException.StatusCode
            };

            await context.Response.WriteAsJsonAsync(responseData);
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var responseData = new
            {
                message = "Internal Server Error",
                exception = exception?.ToString()
            };

            await context.Response.WriteAsJsonAsync(responseData);
        }
    });
});


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("_AngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();