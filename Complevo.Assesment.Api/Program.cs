using System.Reflection;
using System.Text;
using Complevo.Assesment.Api.Services;
using Complevo.Assesment.Data;
using Complevo.Assesment.Services;
using Complevo.Assesment.Services.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var jwtKey = Encoding.UTF8.GetBytes(builder.Configuration["JWT"]);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var apiXmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiPath = Path.Combine(AppContext.BaseDirectory, apiXmlFilename);
    opt.IncludeXmlComments(apiPath);

    var serviceXmlFilename = $"{typeof(ProductDto).Assembly.GetName().Name}.xml";
    var servPath = Path.Combine(AppContext.BaseDirectory, serviceXmlFilename);
    opt.IncludeXmlComments(servPath);

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

});
builder.Services.AddAutoMapper(typeof(ProductDto).Assembly);
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddSingleton<TokenService>(x=> new TokenService(new SymmetricSecurityKey(jwtKey)));
var connectionString = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<ApplicationContext>(
    optBuilder => optBuilder.UseSqlServer(connectionString)
);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtKey),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//FOR DEMO PURPOSES ONLY,Auto creation of DB
using (var scope = app.Services.CreateScope())
{
    try
    {
        scope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        //First run, sql container needs time
        Thread.Sleep(5000);
        scope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.EnsureCreated();
    }

}


app.Run();
