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
                    services.AddSingleton<IServiceBehavior, ServiceDebugBehavior>(serviceProvider =>
                    {
                        var debugBehavior = new ServiceDebugBehavior { IncludeExceptionDetailInFaults = true };
                        return debugBehavior;
                    });
                })
                .Configure(app =>
                {
                    var binding = new BasicHttpBinding();
                    app.UseServiceModel(builder =>
                    {
                        builder.AddService<EpsonTmService>();
                        builder.AddServiceEndpoint<EpsonTmService, IEpsonTmService>(binding, "/cgi-bin/epos/service.cgi");
                    });
                })
                .Build();

            // _host.Start();
            await _host.RunAsync();
            Console.WriteLine("CoreWCF SOAP Service is running...");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in EposPrint: {ex}");
        }
    }
}