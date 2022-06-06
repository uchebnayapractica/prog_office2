using Office_2.DataLayer.Models;
using Office_2.DataLayer.Services;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing;

namespace Office_2.DataLayer;

public static class RequestScanner
{
    public static Request LoadFromFile(string filePath)
    {
        using var image = Image.Load(filePath) as Image<Rgb24>;

        if (image is null)
        {
            throw new Exception("Image is invalid");
        }
        
        return LoadFromImage(image);
    }

    public static Request LoadFromImage(Image<Rgb24> image)
    {
        var reader = new ZXing.ImageSharp.BarcodeReader<Rgb24>();
        reader.Options.Hints.Add(DecodeHintType.CHARACTER_SET, "utf-8");

        var result = reader.Decode(image);
        if (result is null)
        {
            throw new NullReferenceException("Result is null");
        }

        var text = result.Text;
        if (text is null)
        {
            throw new NullReferenceException("Text is null");
        }

        return LoadFromQrString(text);
    }

    public static Request LoadFromQrString(string data)
    {
        var rows = data.Split(' ');
        var values = new Dictionary<string, string>();

        foreach (var row in rows)
        {
            var (key, value) = ParseRow(row);

            values[key] = value;
        }

        return LoadFromQrDictionary(values);
    }

    private static (string key, string value) ParseRow(string row)
    {
        var rowData = row.Split(':', 2);

        var key = ParseValue(rowData[0]);
        var value = ParseValue(rowData[1]);

        return (key, value);
    }

    private static string ParseValue(string preparedValue)
    {
        return preparedValue.Replace('_', ' ');
    }

    public static Request LoadFromQrDictionary(IDictionary<string, string> dict)
    {
        CheckQrDictionary(dict);

        var id = int.Parse(dict["Идентификатор"]);
        
        var clientName = dict["ФИО заявителя"];
        var clientAddress = dict["Адрес"];
        var client = ClientService.GetOrCreateClientByNameAndAddress(clientName, clientAddress);
        
        var status = EnumExtension.GetValueFromDescription<Status>(dict["Статус"]);

        var request = new Request
        {
            Id = id,

            DirectorName = dict["ФИО руководителя"],
            Subject = dict["Тематика"],
            Content = dict["Содержание"],
            Resolution = dict["Резолюция"],
            Status = status,
            Remark = dict["Примечание"]
        };

        if (RequestService.Exists(request))
        {
            RequestService.UpdateRequest(request, client);
        }
        else
        {
            RequestService.InsertRequest(request, client);
        }

        return request;
    }

    private static void CheckQrDictionary(IDictionary<string, string> dict)
    {
        string[] requiredParams =
        {
            "Идентификатор", "ФИО заявителя", "ФИО руководителя", "Адрес", "Тематика", "Содержание", "Резолюция",
            "Статус", "Примечание"
        };
        foreach (var param in requiredParams)
        {
            if (!dict.ContainsKey(param))
            {
                throw new ArgumentException($"No \"{param}\" parameter in dictionary");
            }
        }
    }
}