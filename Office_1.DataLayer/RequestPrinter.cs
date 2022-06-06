using Office_1.DataLayer.Models;
using Office_1.DataLayer.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace Office_1.DataLayer;

public static class RequestPrinter
{
    private const int Width = 794;
    private const int Height = 1123;

    private const int Margin = 30;

    private const int TextSize = 20;
    private const int TextX = 10;
    private const int TextY = 10;
    private const int WrapLength = Width - TextX;

    private const int TextSizeBigLength = 10;
    private const int BigLength = 700;

    public static void PrintIntoFile(string filePath, Request request, bool changeStatusInfoIntoInReview = false)
    {
        using var image = Print(request, changeStatusInfoIntoInReview);

        image.SaveAsJpeg(filePath);
    }

    public static Image Print(Request request, bool changeStatusIntoInReview = false)
    {
        var (image, qrText) = request.GetQr(Width, Height, Margin);

        var size = qrText.Length >= BigLength ? TextSizeBigLength : TextSize;

        WriteText(image, qrText, TextX, TextY, size, WrapLength);

        if (changeStatusIntoInReview)
        {
            request.Status = Status.InReview;
            RequestService.UpdateRequest(request);
        }

        return image;
    }

    private static void WriteText(Image image, string text, int x, int y, int size, int wrapLength)
    {
        var font = SystemFonts.CreateFont("Arial", size, FontStyle.Regular);

        TextOptions options = new(font)
        {
            Origin = new PointF(x, y),
            TabWidth = 8,
            WrappingLength = wrapLength,
            WordBreaking = WordBreaking.BreakAll,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        image.Mutate(img => img.DrawText(options, text, Color.Black));
    }

}