using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Core_WhatWasRead.App_Data;
using ASP.NET_Core_WhatWasRead.App_Data.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ASP.NET_Core_WhatWasRead
{
   public class Startup
   {
      private readonly IConfigurationRoot _config;

      public Startup()
      {
         IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json").SetBasePath(AppContext.BaseDirectory);
         _config = builder.Build();
      }
      // This method gets called by the runtime. Use this method to add services to the container.
      // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
      public void ConfigureServices(IServiceCollection services)
      {
         services.AddSingleton(_config);
         services.AddMvc();
         services.AddDbContext<WhatWasReadContext>();
         services.AddTransient<IRepository, EFRepository>();
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         app.UseRouting();
         app.UseStaticFiles();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "Default",
                   pattern: "/",
                   defaults: new { controller = "Books", action = "List" }); //page = 1, category = "all", accepts filter via querystring
            endpoints.MapControllerRoute(
                   name: "ControllerIdAction",
                   pattern: "{controller}/{action}/{id?}",
                   defaults: new { controller = "Books", action = "Index" }, //page = 1, category = "all", accepts filter via querystring
                   constraints: new { id = @"\d+" }
                   );
            endpoints.MapControllerRoute(
                   name: "CategoryPageRoute",
                   pattern: "books/list/{category}/page{page}",
                   defaults: new { controller = "Books", action = "List", category = "all", page = 1 }, //page = 1, category = "all", accepts filter via querystring
                   constraints: new { page = @"\d+" }
                   );
         });
      }
   }
}
