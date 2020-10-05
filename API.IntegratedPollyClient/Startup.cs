using API.IntegratedPollyClient.PollyTools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace API.IntegratedPollyClient
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

            services.AddHttpClient<IWeatherService, HttpWeatherService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("ApiUrl"));
            })
                .AddPolicyHandler(GetCircuitBreakerPolicy());
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

        private IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 2,
                    durationOfBreak: TimeSpan.FromSeconds(5),
                    onBreak: OnBreak,
                    onReset: OnReset,
                    onHalfOpen: OnHalfOpen);
        }

        private void OnHalfOpen()
        {
            Console.WriteLine("Circuit is Half Open");
        }

        private void OnReset()
        {
            Console.WriteLine("Circuit reset");

        }

        private void OnBreak(DelegateResult<HttpResponseMessage> responseDelegate, TimeSpan arg2)
        {
            Console.WriteLine("Circuit is Broken");
        }

    }
}
