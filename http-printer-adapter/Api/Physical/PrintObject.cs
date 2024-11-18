using System.Drawing;

namespace http_printer_adapter.Config;

public class PrintObject
{
    Image Image { get; set; }

    public PrintObject(Image image)
    {
        Image = image;
    }
}