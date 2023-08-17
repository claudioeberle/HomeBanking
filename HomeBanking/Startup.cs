using HomeBanking.Controllers;
using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HomeBanking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //Add dateabese context
            services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HomeBankingConnection")));
            //Add ClientRepository to services
            services.AddScoped<IClientRepository, ClientRepository>();
            //Add AccountRepository to services
            services.AddScoped<IAccountRepository, AccountRepository>();
            //Add AccountsController
            services.AddScoped<AccountsController>();
            //Add CardRepository to services
            services.AddScoped<ICardRepository, CardRepository>();
            //Add CardsController
            services.AddScoped<CardsController>();
            // add TransactionRepository
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            //Add ClientLoanRepository
            services.AddScoped<IClientLoanRepository, ClientLoanRepository>();
            //add loanRepository
            services.AddScoped<ILoanRepository, LoanRepository>();
            //Add JsonSerializer options to services
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);
            //authentication
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.LoginPath = new PathString("/index.html");
            });
            //authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin"));

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            //must use authentication
            app.UseAuthentication();
            
            //must use authorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller=games}/{action = Get}"
                );
                
            });
        }
    }
}
