using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Viewa.Db;
using Viewa.Services;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Viewa
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
            RegisterServices(services);
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Viewa API", Version = "v1" });
            });

            ConfigureCors(services);

            var securityKey = Configuration.GetSection("TokenSecurityKey").Value;
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                .AddJwtBearer("JwtBearer", jwtBearerOptions => 
                {
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5)
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "Viewa.API");
                c.RoutePrefix = "swagger";
            });
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();

            //var corsOptions = app.ApplicationServices.GetService<IOptions<CorsOptions>>();
            //var options = corsOptions.Value;

            //options.AddPolicy("CorsPolicy",
            //    builder => builder.WithOrigins(Configuration.GetSection("CorsUrl").Value).AllowAnyHeader());




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        protected void ConfigureCors(IServiceCollection services, string origin = null)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => 
                {
                    builder.AllowAnyMethod().AllowAnyHeader();
                    if(string.IsNullOrEmpty(origin))
                    {
                        builder.AllowAnyOrigin();
                    }
                    else
                    {
                        builder.WithOrigins(origin);
                    }
                });
            });
        }
        private void RegisterServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ViewaContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEventService, EventService>();

        }
    }
}
