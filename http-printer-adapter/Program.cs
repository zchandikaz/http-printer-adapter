using System.Text.Json;
using http_printer_adapter.Api.Config;
using http_printer_adapter.Config;

string configPath = args.Length > 0 ? args[0] : "config.json";
var configJson = File.ReadAllText(configPath);

var adapterConfig = JsonSerializer.Deserialize<AdapterConfig>(configJson);

List<Task> tasks = new();
if (adapterConfig != null)
{
    foreach (var adapterConfigPrinterMapping in adapterConfig.PrinterMappings)
    {
        var physicalPrinter = PrinterFactory.CreatePhysicalPrinter(adapterConfigPrinterMapping.PhysicalPrinter);
        var httpPrinter = PrinterFactory.CreateHttpPrinter(adapterConfigPrinterMapping.HttpPrinter, physicalPrinter);
        Task task = httpPrinter.Start();
        tasks.Add(task);
    }
}

foreach (var task in tasks)
{
    task.Wait();
}