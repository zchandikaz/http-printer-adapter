using System.Text.Json;
using http_printer_adapter.Api.Config;
using http_printer_adapter.Config;

var configJson = File.ReadAllText("E:\\dev\\vs\\http-printer-adapter\\http-printer-adapter\\config.json");

var adapterConfig = JsonSerializer.Deserialize<AdapterConfig>(configJson);

if (adapterConfig != null)
{
    foreach (var adapterConfigPrinterMapping in adapterConfig.PrinterMappings)
    {
        var physicalPrinter = PrinterFactory.CreatePhysicalPrinter(adapterConfigPrinterMapping.PhysicalPrinter);
        var httpPrinter = PrinterFactory.CreateHttpPrinter(adapterConfigPrinterMapping.HttpPrinter, physicalPrinter);
        httpPrinter.Start();
    }

    Console.WriteLine(adapterConfig); // Output: Person { FirstName 
}