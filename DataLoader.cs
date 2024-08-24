using LinqProject;
using Newtonsoft.Json;
using System.Xml.Linq;

public class DataLoader
{
    public static List<Car> LoadFromJson(string filePath)
    {
        var jsonData = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Car>>(jsonData);
    }

    public static List<Car> LoadFromXml(string filePath)
    {
        var xmlData = XDocument.Load(filePath);
        return xmlData.Descendants("Car").Select(c => new Car
        {
            Id = (int)c.Element("Id"),
            Make = (string)c.Element("Make"),
            Model = (string)c.Element("Model"),
            Year = (int)c.Element("Year"),
            Price = (double)c.Element("Price"),
            AgeRequirement = (int)c.Element("AgeRequirement")
        }).ToList();
    }

    public static void ConvertJsonToXml(List<Car> cars, string outputFilePath)
    {
        var xml = new XElement("Cars",
            cars.Select(car => new XElement("Car",
                new XElement("Id", car.Id),
                new XElement("Make", car.Make),
                new XElement("Model", car.Model),
                new XElement("Year", car.Year),
                new XElement("Price", car.Price),
                new XElement("AgeRequirement", car.AgeRequirement)
            ))
        );
        xml.Save(outputFilePath);
    }

    public static void ConvertXmlToJson(List<Car> cars, string outputFilePath)
    {
        var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
        File.WriteAllText(outputFilePath, json);
    }
}
