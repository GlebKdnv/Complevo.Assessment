using System.Reflection;
using Complevo.Assesment.Data;
using Complevo.Assesment.Services;
using Complevo.Assesment.Services.Dto;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var apiXmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var apiPath = Path.Combine(AppContext.BaseDirectory, apiXmlFilename);
    opt.IncludeXmlComments(apiPath);

    var serviceXmlFilename = $"{typeof(ProductDto).Assembly.GetName().Name}.xml";
    var servPath = Path.Combine(AppContext.BaseDirectory, serviceXmlFilename);
    opt.IncludeXmlComments(servPath);

});

builder.Services.AddAutoMapper(typeof(ProductDto).Assembly);

builder.Services.AddScoped<ProductService>();

var connectionString = builder.Configuration.GetConnectionString("DbConnection");
builder.Services.AddDbContext<ProductContext>(
    optBuilder=>optBuilder.UseSqlServer(connectionString)
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//FOR DEMO PURPOSES ONLY,Auto creation of DB
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ProductContext>().Database.EnsureCreated();
}
    

app.Run();
