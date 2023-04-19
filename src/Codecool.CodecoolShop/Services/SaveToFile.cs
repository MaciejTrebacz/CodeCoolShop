using System.IO;
using System.Text.Json;

namespace Codecool.CodecoolShop.Services
{
    public class SaveToFile
    {
        public static void ToJson(object obj, string path, string fileName)
        {
            string jsonString = JsonSerializer.Serialize(obj);
            path = path + fileName + ".json";
            File.WriteAllText(path, jsonString);
        }
    }
}
