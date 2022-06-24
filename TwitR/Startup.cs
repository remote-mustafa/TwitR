using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitR.Hubs;
using TwitR.Models.Concrete;
using TwitR.RabbitMQ;
using TwitR.RabbitMQ.Abstract;
using TwitR.RabbitMQ.Concrete;
using TwitR.Repositories.Abstract;
using TwitR.Repositories.Concrete.Dapper;
using TwitR.Repositories.Concrete.Dapper.Context;

namespace TwitR
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
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.AddSingleton<DapperContext>();

            services.AddScoped<IEntityRepository<User>, UserRepository>();
            services.AddScoped<IEntityRepository<Message>, MessageRepository>();
            services.AddScoped<IEntityRepository<Tweet>,TweetRepository>();
            services.AddScoped<ITwitRCommand,RabbitHandler>();


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TweetHub>("/TweetHub");

                endpoints.MapControllerRoute(
                    name: "tweetapi",
                    pattern: "api/{controller=Tweets}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Tweet}/{action=Index}/{id?}");
            });
        }
    }
}
