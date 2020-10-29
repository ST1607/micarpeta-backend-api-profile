using AutoMapper;
using MiCarpeta.Application;
using MiCarpeta.Application.AutoMapper;
using MiCarpeta.Domain;
using MiCarpeta.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiCarpeta.Profile
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<ICiudadanoApplicationService, CiudadanoApplicationService>();
            services.AddScoped<ICiudadanoDomainService, CiudadanoDomainService>();
            services.AddScoped<ICiudadanoRepository, CiudadanoRepository>();

            AutoMapperConfig.Initialize();
            services.AddSingleton(Mapper.Configuration);
            services.AddSingleton<IMapper>(sp =>
              new Mapper(sp.GetRequiredService<AutoMapper.IConfigurationProvider>(), sp.GetService));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
