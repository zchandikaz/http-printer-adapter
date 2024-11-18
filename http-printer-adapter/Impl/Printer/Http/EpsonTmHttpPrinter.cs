using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Http;
using http_printer_adapter.Api.Physical;

namespace http_printer_adapter.Impl.Printer.Http;

public class EpsonTmHttpPrinter(HttpPrinterConfig httpPrinterConfig, AbstractPhysicalPrinter physicalPrinter) : AbstractHttpPrinter(httpPrinterConfig, physicalPrinter)
{
    public override void Start()
    {
        throw new NotImplementedException();
    }
}