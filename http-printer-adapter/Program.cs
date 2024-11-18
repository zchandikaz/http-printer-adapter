using System.Text.Json;
using http_printer_adapter.Api.Config;

var configJson = File.ReadAllText("E:\\dev\\vs\\http-printer-adapter\\http-printer-adapter\\config.json");

var adapterConfig = JsonSerializer.Deserialize<AdapterConfig>(configJson);


Console.WriteLine(adapterConfig); // Output: Person { FirstName 
