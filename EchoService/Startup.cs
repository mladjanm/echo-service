using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EchoService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async context => {
                var uri = context.Request.Path.ToString();
                var ip = string.Empty;
                if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                    ip = context.Request.Headers["X-Forwarded-For"].ToString();
                else if (context.Request.Headers.ContainsKey("x-forwarded-for"))
                    ip = context.Request.Headers["x-forwarded-for"];
                else
                    ip = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("Content-Type", "application/json");
                    return Task.CompletedTask;
                });
                await context.Response.WriteAsync($"{{\"ip\": \"{ip}\", \"uri\":\"{uri}\"}}");
               
            });

            //app.UseRouting();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});
        }
    }
}
