using Newtonsoft.Json;
using System.IO;

namespace Codecool.CodecoolShop.Services
{
    public class SaveToFile
    {
        public static void ToJson(object obj, string path)
        {
            if (!File.Exists(path))
            {
                using (var file = File.CreateText(path))
                {
                    var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
                    file.Write(json);
                }
            }
        }
    }
}
