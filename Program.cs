using Microsoft.EntityFrameworkCore;
using OnlineHelpDesk.Data;


namespace OnlineHelpDesk
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            /////database
            builder.Services.AddDbContext<HelpdeskContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbcs")));

            //class 5
            //session services 
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //CACHE
            builder.Services.AddDistributedMemoryCache();

            //session destroy timing 
            builder.Services.AddSession();


            var app = builder.Build();

            // session call
            app.UseSession();



            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
