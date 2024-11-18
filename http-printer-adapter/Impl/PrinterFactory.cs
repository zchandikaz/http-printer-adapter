using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Http;
using http_printer_adapter.Api.Physical;
using http_printer_adapter.Impl.Printer.Http;

namespace http_printer_adapter.Config;

public static class PrinterFactory
{
    public static AbstractPhysicalPrinter CreatePhysicalPrinter(PhysicalPrinterConfig physicalPrinterConfig)
    {
        switch (physicalPrinterConfig.Type)
        {
            case PhysicalPrinterType.POS_80:
                return new Pos80PhysicalPrinter(physicalPrinterConfig);
            default:
                throw new NotImplementedException();
        }
    }

    public static AbstractHttpPrinter CreateHttpPrinter(HttpPrinterConfig httpPrinterConfig, AbstractPhysicalPrinter physicalPrinter)
    {
        switch (httpPrinterConfig.Type)
        {
            case HttpPrinterType.EPSON_TM:
                return new EpsonTmHttpPrinter(httpPrinterConfig, physicalPrinter);
            default:
                throw new NotImplementedException();
        }
    }
}