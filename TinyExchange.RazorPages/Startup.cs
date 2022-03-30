using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TinyExchange.RazorPages.Database.Managers.Amount;
using TinyExchange.RazorPages.Database.Managers.Auth;
using TinyExchange.RazorPages.Database.Managers.SystemUser;
using TinyExchange.RazorPages.Infrastructure.Authentication;
using ApplicationContext = TinyExchange.RazorPages.Database.ApplicationContext;
namespace TinyExchange.RazorPages;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;
    
    public void ConfigureServices(IServiceCollection services)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddRazorPages();
        services.AddControllers();
        services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(GetConnectionString()));
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddScoped<IBlockingManager, BlockingManager>();
        services.AddScoped<IAmountManager, AmountManager>();
        services.AddScoped<IKycManager, KycManager>();

        services.AddAuthorization(options => options.AddPolicy(KycClaimSettings.PolicyName, 
            builder => builder.RequireClaim(KycClaimSettings.ClaimType, KycClaimSettings.ConfirmedKycClaimValue)));
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => options.LoginPath = "/log-in");
    }
    
    public void Configure(IWebHostEnvironment hostEnvironment, IApplicationBuilder builder)
    {
        if (hostEnvironment.IsDevelopment()) 
            builder.UseDeveloperExceptionPage();

        builder.UseStaticFiles();
        builder.UseRouting();
        builder.UseAuthentication();
        builder.UseAuthorization();
        builder.UseEndpoints(options =>
        {
            options.MapRazorPages();
            options.MapControllers();
        });
    }
    
    
    private string GetConnectionString()
    {
        var host = _configuration["host"] ?? throw new ArgumentNullException();
        var port = _configuration["port"] ?? throw new ArgumentNullException();
        var db = _configuration["db"] ?? throw new ArgumentNullException();
        var userName = _configuration["username"] ?? throw new ArgumentNullException();
        var password = _configuration["password"] ?? throw new ArgumentNullException();
        
        return $"Host={host};Port={port};Database={db};Username={userName};Password={password}";
    }
}