using GoogleDrive.Models;
using GoogleDrive.Models.DataManager;
using GoogleDrive.Models.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleDrive
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
            services.AddDbContext<ApplicationContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:GoogleDrive"]));


            services.AddAuthentication().AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = "211586344064-tjflcjg63s1s1dbm94p3o3o502vcfc55.apps.googleusercontent.com";
                googleOptions.ClientSecret = "M-qDAGgDgqhTsrs0g-Ox-7lp";
                googleOptions.Scope.Add("https://www.googleapis.com/auth/drive.readonly");
                googleOptions.SaveTokens = true;
            });


            services.AddScoped<IDataRepository<TrainingFiles>, TrainingFileManager>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
