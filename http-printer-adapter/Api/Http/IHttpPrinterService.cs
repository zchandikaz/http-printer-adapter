namespace http_printer_adapter.Config;

public abstract class AbstractHttpPrinterService
{
    private IPhysicalPrinter _physicalPrinter;
    
    protected AbstractHttpPrinterService(IPhysicalPrinter physicalPrinter)
    {
        _physicalPrinter = physicalPrinter;
    }
    
    public abstract void start();
}