using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Physical;

namespace http_printer_adapter.Api.Http;

public abstract class AbstractHttpPrinter(HttpPrinterConfig httpPrinterConfig, AbstractPhysicalPrinter physicalPrinter)
{
    protected AbstractPhysicalPrinter PhysicalPrinter = physicalPrinter;

    public abstract Task Start();
}