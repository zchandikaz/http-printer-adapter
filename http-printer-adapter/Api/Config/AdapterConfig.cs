using System.Text.Json.Serialization;
using http_printer_adapter.Config;

namespace http_printer_adapter.Api.Config;

public record PhysicalPrinterConfig
{
    [JsonConstructor]
    public PhysicalPrinterConfig(string name, PhysicalPrinterType type, float width, float height)
    {
        Name = name;
        Type = type;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Physical printer name, will be used for send prints
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Printer type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PhysicalPrinterType Type { get; init; }

    /// <summary>
    /// Height in  Millimeter
    /// </summary>
    public float Width { get; init; }

    /// <summary>
    /// Width in  Millimeter
    /// </summary>
    public float Height { get; init; }

    public override string ToString()
    {
        return $"PhysicalPrinterConfig(Name: {Name}, Type: {Type}, Width: {Width}, Height: {Height})";
    }
}

public record HttpPrinterConfig
{
    [JsonConstructor]
    public HttpPrinterConfig(int port, string host, HttpPrinterType type)
    {
        this.Port = port;
        this.Host = host;
        this.Type = type;
    }

    /// <summary>
    /// Http port
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Http host
    /// </summary>
    public string Host { get; init; }

    /// <summary>
    /// Printer Type
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HttpPrinterType Type { get; init; }

    public override string ToString()
    {
        return $"HttpPrinterConfig(Port: {Port}, Type: {Type})";
    }
}

public record PrinterMapping
{
    [JsonConstructor]
    public PrinterMapping(PhysicalPrinterConfig physicalPrinter, HttpPrinterConfig httpPrinter)
    {
        this.PhysicalPrinter = physicalPrinter;
        this.HttpPrinter = httpPrinter;
    }

    public PhysicalPrinterConfig PhysicalPrinter { get; init; }

    public HttpPrinterConfig HttpPrinter { get; init; }

    public override string ToString()
    {
        return $"PrinterMapping(PhysicalPrinter: {PhysicalPrinter}, HttpPrinter: {HttpPrinter})";
    }
}

public record AdapterConfig
{
    [JsonConstructor]
    public AdapterConfig(List<PrinterMapping> printerMappings)
    {
        this.PrinterMappings = printerMappings;
    }

    public List<PrinterMapping> PrinterMappings { get; init; }

    public override string ToString()
    {
        return $"AdapterConfig(PrinterMappings: [{string.Join(", ", PrinterMappings)}])";
    }
}