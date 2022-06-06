using Office_2.DataLayer.Models;
using Office_2.DataLayer.Services;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing;
using ZXing.Common;

namespace Office_2.DataLayer;

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

    public static void PrintIntoFile(string filePath, Request request, Status? newStatus = Status.InReview)
    {
        using var image = Print(request, newStatus);
        
        image.SaveAsJpeg(filePath);
    }
    
    public static Image Print(Request request, Status? newStatus = Status.InReview)
    {
        var (image, qrText) = GetQr(request, Width, Height, Margin);

        var size = GetTextSize(qrText.Length);
        
        WriteText(image, qrText, TextX, TextY, size, WrapLength);

        if (newStatus is not null)
        {
            request.Status = (Status)newStatus;
            RequestService.UpdateRequest(request);
        }
        
        return image;
    }

    private static int GetTextSize(int qrCodeLength)
    {
        return qrCodeLength >= BigLength ? TextSizeBigLength : TextSize;
    }

    private static void WriteText(Image image, string text, int x, int y, int size, int wrapLength)
    {
        var font = SystemFonts.CreateFont("Arial", size, FontStyle.Regular);

        TextOptions options = new(font)
        {
            Origin = new PointF(x, y),
            //TabWidth = 8, 
            WrappingLength = wrapLength, 
            WordBreaking = WordBreaking.BreakAll,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        
        image.Mutate(img=> img.DrawText(options, text, Color.Black));
    }
    
    public static (Image qrImage, string qrString) GetQr(Request request, int width, int height, int margin)
    {
        var qrString = ToStringForQr(request);

        var options = new EncodingOptions
        {
            Height = height, Width = width, Margin = margin,
        };
        
        options.Hints.Add(EncodeHintType.CHARACTER_SET, "utf-8");
        
        var barcodeWriter = new ZXing.ImageSharp.BarcodeWriter<Rgb24> { 
            Format = BarcodeFormat.QR_CODE, 
            Options = options
        };
        
        var image = barcodeWriter.Write(qrString);
        
        return (image, qrString);
    }

    private static string ToStringForQr(Request request)
    {
        var dict = ToDictionaryForQr(request);
        var values = dict.Select(p => PrepareRow(p.Key, p.Value));

        return string.Join(" ", values);
    }

    private static string PrepareRow(string key, string value)
    {
        return PrepareValue(key) + ":" + PrepareValue(value);
    }
    
    private static string PrepareValue(string value)
    {
        return value.Replace(' ', '_');
    }

    private static IDictionary<string, string> ToDictionaryForQr(Request request)
    {
        return new Dictionary<string, string>
        {
            ["Идентификатор"] = request.Id.ToString(),
            ["ФИО заявителя"] = request.Client.Name,
            ["ФИО руководителя"] = request.DirectorName,
            ["Адрес"] = request.Client.Address,
            ["Тематика"] = request.Subject,
            ["Содержание"] = request.Content,
            ["Резолюция"] = request.Resolution,
            ["Статус"] = request.StatusDescription,
            ["Примечание"] = request.Remark
        };
    }

}