using http_printer_adapter.Api.Config;
using http_printer_adapter.Config;

namespace http_printer_adapter.Api.Physical;

public abstract class AbstractPhysicalPrinter(PhysicalPrinterConfig physicalPrinterConfig)
{
    protected PhysicalPrinterConfig PhysicalPrinterConfig = physicalPrinterConfig;

    public abstract void Print(PrintObject printObject);
}