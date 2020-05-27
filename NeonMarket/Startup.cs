using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NeonMarket.Models;

namespace NeonMarket
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSignalR();

            string connection = Configuration.GetConnectionString("SQLServerConnection");
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 5;   // минимальна€ длина
                opts.Password.RequireNonAlphanumeric = false;   // требуютс€ ли не алфавитно-цифровые символы
                opts.Password.RequireLowercase = false; // требуютс€ ли символы в нижнем регистре
                opts.Password.RequireUppercase = false; // требуютс€ ли символы в верхнем регистре
                opts.Password.RequireDigit = false; // требуютс€ ли цифры


                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcd....@"; // допустимые символы


            })
              .AddEntityFrameworkStores<DatabaseContext>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })

            .AddJwtBearer(options =>
            {

                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // укзывает, будет ли валидироватьс€ издатель при валидации токена
                    ValidateIssuer = true,
                    // строка, представл€юща€ издател€
                    ValidIssuer = Constants.ISSUER,

                    // будет ли валидироватьс€ потребитель токена
                    ValidateAudience = true,
                    // установка потребител€ токена
                    ValidAudience = Constants.AUDIENCE,
                    // будет ли валидироватьс€ врем€ существовани€
                    ValidateLifetime = false,

                    // установка ключа безопасности
                    IssuerSigningKey = Constants.GetSymmetricSecurityKey(),
                    // валидаци€ ключа безопасности
                    ValidateIssuerSigningKey = true,
                };
            });

            services.AddControllersWithViews();
        }

      
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
