using ConsoleApp3;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyRabbitMqService.BL.Interfaces;
using MyRabbitMqService.BL.Services;
using MyRabbitMqService.DL;
using MyRabbitMqService.Models;
using Producer;

namespace MyRabbitMqService
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
            
            services.Configure<MongoDbConfiguration>(Configuration.GetSection(nameof(MongoDbConfiguration)));
            services.AddSingleton<IRabbitMqService, RabbitMqService>();
            services.AddSingleton<IPersonRepository, PersonRepository>();
            services.AddSingleton<IKafkaProducer, KafkaProducer>();

            services.AddHostedService<CachePublisher>();
            services.AddHostedService<KafkaConsumer>();


            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyRabbitMqService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyRabbitMqService v1"));
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
