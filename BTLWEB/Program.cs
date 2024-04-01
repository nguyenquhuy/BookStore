using BTLWEB;
using BTLWEB.ViewModel;


namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    // Initialize Roles and Users. This one will create a user with an Admin-role aswell as a separate User-role. See UserRoleInitializer.cs in Models
                    UserRoleInitializer.InitializaeAsync(services).Wait();
                    // Seed book data
                    //SeedData.Initialize(services);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occured while attempting to seed the database");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
