using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using http_printer_adapter.Api.Config;
using http_printer_adapter.Api.Physical;
using http_printer_adapter.Config;

namespace http_printer_adapter.Impl.Printer.Http;

public class Pos80PhysicalPrinter(PhysicalPrinterConfig physicalPrinterConfig)
    : AbstractPhysicalPrinter(physicalPrinterConfig)
{
    public override void Print(PrintObject printObject)
    {
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrinterSettings.PrinterName = physicalPrinterConfig.Name; // Set the custom printer name here
        printDocument.PrintPage += new PrintPageEventHandler((sender, e) =>
        {
            Image image = printObject.Image;

            // Target width in mm
            float targetWidthMM = physicalPrinterConfig.Width;

            // Convert mm to pixels (assuming 96 DPI)
            float dpi = (float)(physicalPrinterConfig?.ExtraProperties?["dpi"] ?? 96f);
            float mmPerInch = (float)(physicalPrinterConfig?.ExtraProperties?["mmPerInch"] ?? 25.4f);

            float targetWidthPixels = (targetWidthMM / mmPerInch) * dpi;

            // Calculate the aspect ratio and new height
            float aspectRatio = (float)image.Height / image.Width;
            int targetHeightPixels = (int)(targetWidthPixels * aspectRatio);

            // Resize the original image
            Bitmap resizedImage = new Bitmap(image, new Size((int)targetWidthPixels, targetHeightPixels));

            // // Create a new bitmap with the same dimensions as the resized image
            // Bitmap invertedImage = new Bitmap(resizedImage.Width, resizedImage.Height);
            //
            // // Create a graphics object from the bitmap
            // using (Graphics g = Graphics.FromImage(invertedImage))
            // {
            //     // Create color matrix to invert colors
            //     ColorMatrix colorMatrix = new ColorMatrix(
            //         new float[][]
            //         {
            //             new float[] { -1, 0, 0, 0, 0 },
            //             new float[] { 0, -1, 0, 0, 0 },
            //             new float[] { 0, 0, -1, 0, 0 },
            //             new float[] { 0, 0, 0, 1, 0 },
            //             new float[] { 1, 1, 1, 0, 1 }
            //         });
            //
            //     // Create image attributes and set the color matrix
            //     ImageAttributes attributes = new ImageAttributes();
            //     attributes.SetColorMatrix(colorMatrix);
            //
            //     // Draw the resized image on the new bitmap, applying the color matrix
            //     g.DrawImage(resizedImage, new Rectangle(0, 0, resizedImage.Width, resizedImage.Height),
            //         0, 0, resizedImage.Width, resizedImage.Height, GraphicsUnit.Pixel, attributes);
            // }

            // Draw the inverted image on the PrintPageEventArgs
            Point location = new Point(0, 0);
            e.Graphics.DrawImage(resizedImage, location);
        });
        printDocument.Print();
    }
}