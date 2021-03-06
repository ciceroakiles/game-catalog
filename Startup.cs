using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using game_catalog.Models;
using game_catalog.Repositories;
using game_catalog.Services;

namespace game_catalog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // (Use este método para adicionar serviços ao container)
        public void ConfigureServices(IServiceCollection services)
        {
            // Entity Framework
            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Database"));
            
            // Injeção de dependências
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameService, GameService>();
            
            services.AddControllers();
            
            // Configuração do Swagger
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET Core Web API com Swagger",
                    Description = "Isto é pra mostrar que é possível documentar uma ASP.NET Core Web API com Swagger.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Nome qualquer",
                        Url = new Uri("https://site.com")
                    }
                });
                // Incluir XML que reconhece as tags dos comentários
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                s.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // (Use este método para configurar o pipeline de requisições HTTP)
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

            // Configuração do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.RoutePrefix = "swagger";
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "API Exemplo");
            });
        }
    }
}
