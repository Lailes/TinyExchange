using TinyExchange.RazorPages;

CreateBuilder(args).Build().Run();

static IHostBuilder CreateBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(options => options.UseStartup<Startup>());