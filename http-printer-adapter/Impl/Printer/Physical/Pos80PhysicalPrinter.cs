using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Physical;
using http_printer_adapter.Config;

namespace http_printer_adapter.Impl.Printer.Http;

public class Pos80PhysicalPrinter(PhysicalPrinterConfig physicalPrinterConfig) : AbstractPhysicalPrinter(physicalPrinterConfig)
{
    public override void Print(PrintObject printObject)
    {
        Console.WriteLine("printed!");
    }
}