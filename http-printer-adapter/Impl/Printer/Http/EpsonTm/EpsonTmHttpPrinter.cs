using System.Drawing.Imaging;
using CoreWCF;
using test_rider_app.api;

using CoreWCF.Configuration;
using CoreWCF.Description;
using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Http;
using http_printer_adapter.Api.Physical;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace http_printer_adapter.Impl.Printer.Http.EpsonTm;

public class EpsonTmHttpPrinter(HttpPrinterConfig httpPrinterConfig, AbstractPhysicalPrinter physicalPrinter) : AbstractHttpPrinter(httpPrinterConfig, physicalPrinter)
{
    public override async Task Start()
    {
        try
        {
            IWebHost _host;
            string baseAddress = $"http://{httpPrinterConfig.Host}:{httpPrinterConfig.Port}";

            _host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls(baseAddress)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices(services =>
                {
                    services.AddServiceModelServices();
                    
                    services.AddSingleton<HttpPrinterConfig>(httpPrinterConfig);
                    services.AddSingleton<AbstractPhysicalPrinter>(physicalPrinter);                   
                    
                    services.AddSingleton<IServiceBehavior, ServiceDebugBehavior>(serviceProvider =>
                    {
                        var debugBehavior = new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true };
                        return debugBehavior;
                    });
                    
                    services.AddSingleton<EpsonTmService>();
                })
                .Configure(app =>
                {
                    app.UseMiddleware<ContentTypeTransformMiddleware>();
                    
                    var binding = new BasicHttpBinding();
                    app.UseServiceModel(builder =>
                    {
                        builder.AddService<EpsonTmService>();
                        builder.AddServiceEndpoint<EpsonTmService, IEpsonTmService>(binding, "/cgi-bin/epos/service.cgi");
                    });
                })
                .Build();
            
            await _host.RunAsync();
            Console.WriteLine("CoreWCF SOAP Service is running...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in EposPrint: {ex}");
        }
    }
}

public class ContentTypeTransformMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.ContentType = "text/xml; charset=utf-8";
        await next(context);
    }
}