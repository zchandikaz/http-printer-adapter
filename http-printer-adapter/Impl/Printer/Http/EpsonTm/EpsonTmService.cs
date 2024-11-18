using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using CoreWCF;

namespace test_rider_app.api;

[ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)] 
public class EpsonTmService : IEpsonTmService
{
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
            
            Console.WriteLine(request.ToString());
            
            // Base64ToImage(request.Image.Value, request.Image.Width, request.Image.Height, "output_image.png");
            printDoc();

            return response;
        }catch(Exception ex)
        {
            Console.WriteLine($"Exception in EposPrint: {ex.ToString()}");
        }

        return response;
    }
    
        private void printDoc()
    {
        PrintDocument printDocument = new PrintDocument();
        printDocument.PrinterSettings.PrinterName = "POS-80-Series"; // Set the custom printer name here
        printDocument.PrintPage += new PrintPageEventHandler(PrintImage);
        printDocument.Print();
    }

    private void PrintImage(object sender, PrintPageEventArgs e)
    {
        Image image = Image.FromFile("D:\\Work\\dev\\infact\\printer-adapter\\output_image.png");

        // Target width in mm
        float targetWidthMM = 80f;

        // Convert mm to pixels (assuming 96 DPI)
        float dpi = 96f;
        float mmPerInch = 25.4f;
        float targetWidthPixels = (targetWidthMM / mmPerInch) * dpi;

        // Calculate the aspect ratio and new height
        float aspectRatio = (float)image.Height / image.Width;
        int targetHeightPixels = (int)(targetWidthPixels * aspectRatio);

        // Resize the original image
        Bitmap resizedImage = new Bitmap(image, new Size((int)targetWidthPixels, targetHeightPixels));

        // Create a new bitmap with the same dimensions as the resized image
        Bitmap invertedImage = new Bitmap(resizedImage.Width, resizedImage.Height);

        // Create a graphics object from the bitmap
        using (Graphics g = Graphics.FromImage(invertedImage))
        {
            // Create color matrix to invert colors
            ColorMatrix colorMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {-1,  0,  0,  0, 0},
                    new float[] { 0, -1,  0,  0, 0},
                    new float[] { 0,  0, -1,  0, 0},
                    new float[] { 0,  0,  0,  1, 0},
                    new float[] { 1,  1,  1,  0, 1}
                });

            // Create image attributes and set the color matrix
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            // Draw the resized image on the new bitmap, applying the color matrix
            g.DrawImage(resizedImage, new Rectangle(0, 0, resizedImage.Width, resizedImage.Height),
                0, 0, resizedImage.Width, resizedImage.Height, GraphicsUnit.Pixel, attributes);
        }

        // Draw the inverted image on the PrintPageEventArgs
        Point location = new Point(0, 0);
        e.Graphics.DrawImage(invertedImage, location);
}
    
    public void Base64ToImage(string base64String, int width, int height, string filePath, string mode = "left", string align = "center", string color = "color_1")
    {
        // Convert base64 string to byte array
        byte[] imageBytes = Convert.FromBase64String(base64String);
        
        // Load the byte array into a MemoryStream
        using (var ms = new MemoryStream(imageBytes))
        {
            // Create an image from the byte array
            Image image = Image.FromStream(ms);
            
            // Resize the image
            Bitmap resizedImage = new Bitmap(image, new Size(width, height));
            
            // Additional logic to handle 'mode', 'align', and 'color'
            // For simplicity, we are assuming center alignment and color_1 as default for complexity
            
            // Save the final image to a file
            resizedImage.Save(filePath, ImageFormat.Png);
        }
    }
}
