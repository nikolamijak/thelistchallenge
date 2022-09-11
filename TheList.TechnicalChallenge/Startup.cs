using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Reflection;
using System.Text.Json;
using TheList.TechnicalChallenge.Behaviours;
using TheList.TechnicalChallenge.Exceptions;
using TheList.TechnicalChallenge.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace TheList.TechnicalChallenge
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
            services.AddControllers();
            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    IEnumerable<ValidationProblemDetails> errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .Select(e => new ValidationProblemDetails(actionContext.ModelState));
                    var errorsFlattened = errors.SelectMany(x => x.Errors).ToDictionary(x=>x.Key, x=>x.Value);
                   
                    throw new CustomValidationException(errorsFlattened);
                };
            });
            services
                .AddFluentValidators()
                .AddMediator()
                .AddRepository(Configuration);
               
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages(async (StatusCodeContext context) =>
            {
                context.HttpContext.Response.ContentType = "application/vnd.api+json";
                var response = new ExceptionResponse(new { code = "error", reason = context.HttpContext.Response.StatusCode.ToString() },
                    (HttpStatusCode)context.HttpContext.Response.StatusCode);
                await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response.Response));
            });

            app.UseMiddleware<ExceptionMiddleware>()
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }


}
