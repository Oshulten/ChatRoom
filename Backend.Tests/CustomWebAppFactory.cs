using Backend.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.SqlEdge;

namespace Backend.Tests;

public class CustomWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly SqlEdgeContainer _sqlContainer = new SqlEdgeBuilder().Build();
    public ChatroomDatabaseContext GetContext()
    {
        using var serviceScope = this.Services.CreateAsyncScope();
        return (serviceScope.ServiceProvider.GetService<ChatroomDatabaseContext>()!);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var updatedConnectionString = _sqlContainer.GetConnectionString().Replace("Database=master", "Database=testing");
        builder.UseSetting("ConnectionStrings:DefaultConnection", updatedConnectionString);

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ChatroomDatabaseContext>>();
            services.RemoveAll<ChatroomDatabaseContext>();
            services.AddDbContext<ChatroomDatabaseContext>(options =>
            {
                options.UseSqlServer(_sqlContainer.GetConnectionString());
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
        using var serviceScope = this.Services.CreateAsyncScope();
        var service = (serviceScope.ServiceProvider.GetService<ChatroomDatabaseContext>()!);
        service.Database.EnsureCreated();
    }

    public new Task DisposeAsync() => _sqlContainer.DisposeAsync().AsTask();
}