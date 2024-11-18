using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using CoreWCF;
using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Physical;
using http_printer_adapter.Config;

namespace test_rider_app.api;

[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
public class EpsonTmService(HttpPrinterConfig httpPrinterConfig, AbstractPhysicalPrinter physicalPrinter)
    : IEpsonTmService
{
    private HttpPrinterConfig httpPrinterConfig = httpPrinterConfig;
    private AbstractPhysicalPrinter physicalPrinter = physicalPrinter;

    public EposPrintResponse EposPrint(EposPrintRequest request)
    {
        var response = new EposPrintResponse
        {
            Success = true, // set based on your logic
            Code = "", // set based on your logic
            Status = "252641302", // set based on your logic
            Battery = 0 // set based on your logic
        };
        try
        {
            // Process the request here.
            // Implement your logic and populate the response.

            // var response = new EposPrintResponse
            // {
            //     Success = true, // set based on your logic
            //     Code = "", // set based on your logic
            //     Status = "252641302", // set based on your logic
            //     Battery = 0 // set based on your logic
            // };

            // Console.WriteLine(request.ToString());

            Image image=  Base64ToImage(request.Image.Value, request.Image.Width, request.Image.Height);
            
            physicalPrinter.Print(new PrintObject(image));

            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in EposPrint: {ex.ToString()}");
        }

        return response;
    }

    private static Image Base64ToImage(string base64String, int width, int height, string mode = "mono", string align = "left")
    {
        try
        {
            // Decode base64 string to byte array
            byte[] imageData = Convert.FromBase64String(base64String);

            // Create a bitmap object
            Bitmap image = new Bitmap(width, height);

            // Determine mode (mono or palette-based)
            if (mode == "mono")
            {
                // Mono mode: 1-bit per pixel
                SetMonoImage(image, imageData);
            }
            else
            {
                // Palette mode: 16 colors (indexed color)
                SetPaletteImage(image, imageData);
            }

            // Optional: Align handling (not implemented in this example)
            if (align != "left")
            {
                Console.WriteLine($"Alignment '{align}' is requested, but not implemented. Default to 'left'.");
            }

            return image;
        }
        catch (Exception ex)
        {
            throw new Exception($"Exception in Base64ToImage", ex);
        }
    }

    private static void SetMonoImage(Bitmap image, byte[] imageData)
    {
        int index = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                // Set pixel based on the imageData (1-bit pixel depth)
                bool isSet = (imageData[index / 8] & (1 << (7 - (index % 8)))) != 0;
                image.SetPixel(x, y, isSet ? Color.Black : Color.White);
                index++;
            }
        }
    }

    private static void SetPaletteImage(Bitmap image, byte[] imageData)
    {
        Color[] palette =
        {
            Color.Black, Color.Red, Color.Green, Color.Blue,
            Color.Yellow, Color.Magenta, Color.Cyan, Color.White,
            Color.Gray, Color.Brown, Color.Purple, Color.Orange,
            Color.Pink, Color.LightBlue, Color.LightGreen, Color.LightGray
        };

        int index = 0;
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                int colorIndex = imageData[index++] % palette.Length;
                image.SetPixel(x, y, palette[colorIndex]);
            }
        }
    }
}