﻿using System;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using dotnet_azure_fhir_web_api.IServices;
using dotnet_azure_fhir_web_api.Services;
using dotnet_azure_fhir_web_api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;

namespace dotnet_azure_fhir_web_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            AuthenticationConfig config = AuthenticationConfig.ReadFromJsonFile("appsettings.json");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddHttpClient("protectedapi", options =>
            {
                options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IProtectedWebApiCallerService, ProtectedWebApiCallerService>();
            services.AddTransient<IResourceFetchService, ResourceFetchService>();
            services.AddTransient<IPatientService, PatientService>();
            services.AddTransient<IObservationService, ObservationService>();
            services.AddTransient<IMedicationService, MedicationService>();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            { 
                app.UseHsts();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            app.UseMvc();


        }
    }
}
