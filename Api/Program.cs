using Api.Data;
using Api.Models;
using Api.Services.DBService;
using Api.Services.OrderService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDbContext<DBContextModel>(options =>
                options.UseSqlServer("Server=.\\SQLEXPRESS;Database=MyDatabase;Integrated Security=True;TrustServerCertificate=true;"));

        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IDbService, DbService>();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Initialize the seed data
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DBContextModel>();
                context.Database.Migrate();
                SeedData.Initialize(services);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}